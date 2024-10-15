using Microsoft.AspNetCore.Mvc;
using SelectPdf;

namespace CarServiceApp.PdfConverter.Abstract
{
    public interface IHtmlToPdfHelper
    {
        Task ConvertHtml2PdfResponseAsync(Controller controller, string viewName, object model, PdfPageSize sizePage = PdfPageSize.A4, PdfPageOrientation orientationPage = PdfPageOrientation.Portrait);
        Task ConvertHtml2PdfResponseAsync(Controller controller, string viewName, object model, bool isPaging, string htmlTableHeader, PdfPageSize sizePage = PdfPageSize.A4, PdfPageOrientation orientationPage = PdfPageOrientation.Portrait);
    }
}
