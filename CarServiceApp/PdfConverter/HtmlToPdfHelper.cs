using CarServiceApp.PdfConverter.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SelectPdf;
using System.Drawing;

namespace CarServiceApp.PdfConverter
{
    public class HtmlToPdfHelper: IHtmlToPdfHelper
    {
        private static async Task<string> RenderRazorViewToStringAsync(Controller controller, string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = controller.HttpContext.RequestServices };
            var actionContext = new ActionContext(httpContext, httpContext.GetRouteData(), controller.ControllerContext.ActionDescriptor);

            using (var stringWriter = new StringWriter())
            {
                var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                var viewResult = viewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model };
                var tempDataSerializer = new CustomTempDataSerializerHelper();
                var tempDataProvider = new SessionStateTempDataProvider(tempDataSerializer);
                var tempData = new TempDataDictionary(httpContext, tempDataProvider);
                var viewContext = new ViewContext(actionContext, viewResult.View, viewData, tempData, stringWriter, new HtmlHelperOptions());

                await viewResult.View.RenderAsync(viewContext);
                return stringWriter.ToString();
            }
        }

        // Рендеринг и конвертация HTML в PDF с обратной отсылкой на запрос
        public async Task ConvertHtml2PdfResponseAsync(Controller controller, string viewName, object model, PdfPageSize sizePage = PdfPageSize.A4, PdfPageOrientation orientationPage = PdfPageOrientation.Portrait)
        {
            var htmlString = await RenderRazorViewToStringAsync(controller, viewName, model);

            var webPageWidth = orientationPage == PdfPageOrientation.Landscape ? 1450 : 1150;
            const int webPageHeight = 0;

            // Создание экземпляра объекта конвертера HTML в PDF
            var converter = new HtmlToPdf
            {
                Options =
                {
                    // Установка параметров конвертера
                    WebPageWidth = webPageWidth,
                    WebPageHeight = webPageHeight,
                    WebPageFixedSize = false,
                    AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly,
                    AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment,
                    MarginLeft = 10,
                    MarginRight=10,
                    MarginTop=20,
                    MarginBottom=20,
                    PdfPageSize = sizePage,
                    PdfPageOrientation=orientationPage,
                }
            };

            // Создание нового PDF-документа путем преобразования HTML-строки
            var urlCurrent = controller?.Request?.Path.ToString();
            var doc = converter.ConvertHtmlString(htmlString, urlCurrent);
            // Сохранение PDF-документа
            //doc.Save(controller.HttpContext.Response.Body);
                        await Task.Run(() => doc.Save(controller.HttpContext.Response.Body));
                        doc.Close();
        }

        // Рендеринг и конвертация HTML в PDF с обратной отсылкой на запрос с пагинацией и заголовками таблиц в шапке
        public async Task ConvertHtml2PdfResponseAsync(Controller controller, string viewName, object model, bool isPaging, string htmlTableHeader, PdfPageSize sizePage = PdfPageSize.A4, PdfPageOrientation orientationPage = PdfPageOrientation.Portrait)
        {
            var htmlString = await RenderRazorViewToStringAsync(controller, viewName, model);

            var webPageWidth = orientationPage == PdfPageOrientation.Landscape ? 1450 : 1150;
            const int webPageHeight = 0;

            // Создание экземпляра объекта конвертера HTML в PDF
            var converter = new HtmlToPdf
            {
                Options =
                {
                    // Установка параметров конвертера
                    WebPageWidth = webPageWidth,
                    WebPageHeight = webPageHeight,
                    WebPageFixedSize = false,
                    AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly,
                    AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment,
                    MarginLeft = 10,
                    MarginRight=10,
                    MarginTop=20,
                    MarginBottom=20,
                    PdfPageSize = sizePage,
                    PdfPageOrientation=orientationPage
                }
            };

            if (isPaging)
            {
                // Настройка подвала
                converter.Options.DisplayFooter = true;
                converter.Footer.DisplayOnFirstPage = true;
                converter.Footer.DisplayOnOddPages = true;
                converter.Footer.DisplayOnEvenPages = true;
                converter.Footer.Height = 30;
                // Добавление номеров страниц с помощью объекта PdfTextSection
                var text = new PdfTextSection(
                    5, 
                    5,
                    "Страница: {page_number} из {total_pages}",
                    font: new Font("Arial", 8))
                {
                    HorizontalAlign = PdfTextHorizontalAlign.Center
                };
                converter.Footer.Add(text);
            }
            // Настройка заголовка
            converter.Options.DisplayHeader = true;
            converter.Header.DisplayOnFirstPage = false;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            converter.Header.Height = 30;
            // Добавление HTML-контента в заголовок
            var headerHtml = new PdfHtmlSection(htmlTableHeader, null)
            {
                WebPageWidth = orientationPage == PdfPageOrientation.Landscape ? 1450 : 1150,
                AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment,
                AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly
            };
            converter.Header.Add(headerHtml);
            // Создание нового PDF-документа путем преобразования HTML-строки
            var doc = converter.ConvertHtmlString(htmlString);
            // Сохранение PDF-документа
            //doc.Save(controller.HttpContext.Response.Body);
            await Task.Run(() => doc.Save(controller.HttpContext.Response.Body));
            doc.Close();
        }
    }
}
