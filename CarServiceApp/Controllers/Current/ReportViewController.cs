using CarServiceApp.Domain.Extension;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Repository;
using CarServiceApp.Domain.Service.Abstract;
using CarServiceApp.Models;
using CarServiceApp.PdfConverter.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace CarServiceApp.Controllers
{
    public class ReportViewController : Controller
    {
        private readonly DateTime currentDate = DateTime.Now;
        private readonly int currentYear = DateTime.Now.Year;
        private readonly int currentMonth = DateTime.Now.Month;
        private readonly Dictionary<int, string> monthsWithoutDefaultValue;
        private readonly Dictionary<int, string> months;
        private readonly Dictionary<int, int> years;
        private const int DefaultPageSize = 6;
        private const int TotalSumCount = 5;
        private const int DefaultSortRow = 3;

        private readonly ICommonService _commonService;
        private readonly IDiagramService _diagramService;
        private readonly IReportService _reportService;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IHtmlToPdfHelper _htmlToPdfHelper;
        private readonly IOrderService _orderService;
        private readonly IClientService _clientService;

        public ReportViewController(
            ICommonService commonService,
            IReportService reportService,
            ICompositeViewEngine viewEngine,
            IHtmlToPdfHelper htmlToPdfHelper,
            IDiagramService diagramService,
            IOrderService orderService,
            IClientService clientService)
        {
            _htmlToPdfHelper = htmlToPdfHelper ?? throw new ArgumentNullException(nameof(htmlToPdfHelper));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
            _viewEngine = viewEngine ?? throw new ArgumentNullException(nameof(viewEngine));
            _diagramService = diagramService ?? throw new ArgumentNullException(nameof(diagramService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            monthsWithoutDefaultValue = _commonService.Months(false);
            months = _commonService.Months();
            years = _commonService.Years();
        }

        public async Task<IActionResult> AnalysePartsTrade(
            byte typePeriodId,
            DateTime? startDate,
            DateTime? endDate,
            int? page)
        {
            var parameters = PrepareAnalysisParameters(typePeriodId, startDate, endDate);

            page ??= 1;
            IEnumerable<ReportResultModel> model;

            if (ModelState.IsValid)
            {
                model = await _reportService.AnalysePartsTrade(parameters.TypePeriodId, parameters.EndDate, parameters.StartDate);
                parameters.GridItem = CommonRepositoryAsync.GetAllWithPages(model, page ?? 0, DefaultPageSize);

                return View(parameters);
            }

            return BadRequest(ModelState);
        }

        private ChoosingParamsAnalysis PrepareAnalysisParameters(byte typePeriodId, DateTime? startDate, DateTime? endDate)
        {
            return new ChoosingParamsAnalysis
            {
                TypePeriodId = typePeriodId,
                StartDate = startDate ?? currentDate,
                EndDate = endDate ?? currentDate,
            };
        }

        public async Task<IActionResult> AnalyseServicesTrade(
            byte typePeriodId,
            DateTime? startDate,
            DateTime? endDate,
            int? page)
        {
            var parameters = PrepareAnalysisParameters(typePeriodId, startDate, endDate);
            IEnumerable<ReportResultModel> model;

            if (ModelState.IsValid)
            {
                model = await _reportService.AnalyseServicesTrade(parameters.TypePeriodId, parameters.EndDate, parameters.StartDate);
                parameters.GridItem = CommonRepositoryAsync.GetAllWithPages(model, page ?? 0, DefaultPageSize);

                return View(parameters);
            }

            return BadRequest(ModelState);
        }

        public async Task<IActionResult> ReportByMonthForEmployee(int? year, int? month)
        {
            year ??= currentYear;
            month ??= currentMonth;

            ViewBag.SelectListYear = years.ToSelectList("Value", "Text");
            ViewBag.SelectListMonth = monthsWithoutDefaultValue.ToSelectList("Value", "Text");

            var report = (await _reportService.ReportByExecServiceByMonthForEmployeesAsync(year.Value, month.Value)).ToList();

            return View(report);
        }

        public async Task PrintReportByMonthForEmployee(int? year, int? month)
        {
            year ??= currentYear;
            month ??= currentMonth;

            var list = (await _reportService.ReportByExecServiceByMonthForEmployeesAsync(year, month)).ToList();

            var report = new PrintReportModel<ReportByExecServiceByMonthForEmployee>
            {
                SelectedDate = new DateTime(year.Value, month.Value, 1),
                List = list,
                FullName = GetUserFullName()
            };

            await _htmlToPdfHelper.ConvertHtml2PdfResponseAsync(this, "ReportView/PrintReportByMonthForEmployee", report);
        }

        private static string GetUserFullName()
        {
            //    var user = identityContext.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            //    if (user != null)
            //        ViewBag.NamePrinterMan = autoServiceDb.Employees.Find(user.PersonnelNumber).FullName.ConvertToShortName();

            return string.Empty;
        }

        public async Task<IActionResult> ReportOrders(int? page, [FromForm] ReportModel model)
        {
            page ??= 1;
            model.Year ??= currentYear;
            model.Month ??= currentMonth;
            model.IsAsc ??= false;
            model.SortRow ??= DefaultSortRow;
            model.Years = years.ToSelectList("Value", "Text");
            model.Months = monthsWithoutDefaultValue.ToSelectList("Value", "Text");

            List<ReportByOrderModel> report = await _reportService.ReportByExecOrdersAsync(model.Year, model.Month, model.SortRow, model.IsAsc);

            if ((int)Math.Ceiling((decimal)report.Count / DefaultPageSize) == page)
            {
                ViewBag.listMoney = SetResults(report);
            }

            var result = CommonRepositoryAsync.GetAllWithPages(report, page.Value, DefaultPageSize);
            model.GridItem = result;

            return View(model);
        }

        private static List<decimal> SetResults(List<ReportByOrderModel> items)
        {
            var result = new List<decimal>(TotalSumCount)
                {
                    Math.Round(items.Sum(s => s.CostService ?? 0)),
                    Math.Round(items.Sum(s => s.TaxAddedValue ?? 0)),
                    Math.Round(items.Sum(s => s.PriceService ?? 0)),
                    Math.Round(items.Sum(s => s.TotallPriceParts ?? 0)),
                    Math.Round(items.Sum(s => s.TotallPriceOrder ?? 0))
                };

            return result;
        }

        public async Task PrintReportOrders(int? year, int? month, int? sortRow, bool? isAsc)
        {
            year ??= currentYear;
            month ??= currentMonth;
            isAsc ??= false;
            sortRow ??= DefaultSortRow;

            List<ReportByOrderModel> list = await _reportService.ReportByExecOrdersAsync(year, month, sortRow, isAsc);

            var report = new PrintReportModel<ReportByOrderModel>
            {
                SelectedDate = new DateTime(year.Value, month.Value, 1),
                List = list,
                TotalMoneys = SetResults(list),
                FullName = GetUserFullName()
            };

            await _htmlToPdfHelper.ConvertHtml2PdfResponseAsync(this, "ReportView/PrintReportOrders", report, true, "", SelectPdf.PdfPageSize.A4, SelectPdf.PdfPageOrientation.Landscape);
        }

        public async Task<IActionResult> ReportByProviders(int? page, [FromForm] ReportModel model)
        {
            page ??= 1;
            model.Year ??= currentYear;
            model.Month ??= currentMonth;
            model.Years = years.ToSelectList("Value", "Text");
            model.Months = months.ToSelectList("Value", "Text");

            var report = await AmountMoneyByProvidersAsync(model.Month, model.Year);

            var result = CommonRepositoryAsync.GetAllWithPages(report, page.Value, DefaultPageSize);
            model.GridItem = result;

            return View(model);
        }

        private async Task<List<AmountMoneyByProvider>> AmountMoneyByProvidersAsync(int? month, int? year)
        {
            var report = month == 0
               ? (await _reportService.AmountMoneyByProvidersAsync()).Where(f => f.Year == year).ToList()
               : (await _reportService.AmountMoneyByProvidersAsync()).Where(f => f.Year == year && f.Month == month).ToList();

            return report;
        }

        public async Task PrintReportByProviders(int? year, int? month)
        {
            year ??= currentYear;
            month ??= currentMonth;

            var list = await AmountMoneyByProvidersAsync(month, year);

            var report = new PrintReportModel<AmountMoneyByProvider>
            {
                SelectedDate = new DateTime(year.Value, month.Value, 1),
                List = list,
                SelectedYear = year,
                SeletedMonthName = month.Value != 0 ? new DateTime(year.Value, month.Value, 1).ToString("MMMM") : null,
                FullName = GetUserFullName(),
                Months = months
            };

            await _htmlToPdfHelper.ConvertHtml2PdfResponseAsync(this, "ReportView/PrintReportByProviders", report, true, "", SelectPdf.PdfPageSize.A4, SelectPdf.PdfPageOrientation.Portrait);
        }

        public ActionResult DiagramCountOfOrders()
        {
            ViewBag.selectListYear = years.ToSelectList("Value", "Text");
            ViewBag.selectListMonth = months.ToSelectList("Value", "Text");

            return View();
        }

        public JsonResult GetDiagram(int? year, int? month)
        {
            DiagramModel[] result = _diagramService.GetItems(year, month, months);
            return Json(result);
        }

        public ActionResult DiagramTotalIncomeByServices()
        {
            ViewBag.selectListYear = years.ToSelectList("Value", "Text");
            ViewBag.selectListMonth = months.ToSelectList("Value", "Text");

            return View();
        }

        [HttpPost]
        public JsonResult GetDiagramIncome(int? year, int? month)
        {
            object[] result = _diagramService.GetIncomeItems(year, month, months);
            return Json(result);
        }

        public async Task GetReportOrderPdf(string orderId)
        {
            var report = new ReportOrderForClientModel
            {
                ServiceList = await _orderService.GetTotalInfoServicesByOrderAsync(orderId),
                PartList = await _orderService.GetAllInfoAttachedPartByIdAsync(orderId),
                PartCustomList = await _orderService.GetUsingCustomSpartMatAsync(orderId),
                Order = await _orderService.GetOrderByIdAsync(orderId)
            };

            //выполнение процедуры
            var result = await _orderService.GetCalculatedTotalMoneyTimeByOrderAsync(orderId);
            //вносим в хранилище c округлением до 2х знаков
            ViewData.Add("totMoneyServ", decimal.Round(result.Item1, 2));
            ViewData.Add("totMoneyServFull", decimal.Round(result.Item2, 2));
            ViewData.Add("totTimeServ", result.Item3);
            ViewData.Add("totMoneyPartsFull", decimal.Round((decimal)result.Item4, 2));
            ViewData.Add("totMoneyFullByOrder", decimal.Round(result.Item4 + result.Item2, 2));

            try
            {
                var executingService = await _orderService.GetExecutingServiceAsync(orderId, "mngclose");

                ViewBag.ManagerNameComplete = executingService?.Employee?.FullName?.ConvertToShortName();
                ViewBag.PostManagerComplete = executingService?.Employee?.Post?.Name;
            }
            catch { }
            await _htmlToPdfHelper.ConvertHtml2PdfResponseAsync(this, "ReportView/OrderForClientReport", report);
        }

        // получить Заказ-заявку
        public async Task GetReportQueryClientPdf(string orderId)
        {
            var report = new ReportOrderForClientCustomQueryModel
            {
                ServiceList = await _orderService.GetTotalInfoServicesByOrderAsync(orderId),
                Order = await _orderService.GetOrderByIdAsync(orderId)
            };

            try
            {
                var executingService = await _orderService.GetExecutingServiceAsync(orderId, "mngclose");

                ViewBag.ManagerNameComplete = executingService?.Employee?.FullName?.ConvertToShortName();
                ViewBag.PostManagerComplete = executingService?.Employee?.Post?.Name;
            }
            catch { }
            await _htmlToPdfHelper.ConvertHtml2PdfResponseAsync(this, "ReportView/OrderForClientCustomQueryReport", report);
        }

        // получить Акт-сдачи приемки автомобиля
        public async Task GetReportAcceptanceCarPdf(string orderId, string clientId)
        {
            var report = new ReportOrderForClientAcceptanceCarViewModel
            {
                CustomParts = (await _orderService.GetReportAcceptedCustomParts(orderId, clientId)).ToList()
            };
            var order = await _orderService.GetOrderByIdAsync(orderId);
            report.Client = await _clientService.GetAsync(clientId);
            report.Car = order.Car;
            report.OrderService = order;
            report.Remarks = order.RemarkToStateCars.ToList();
            report.N = 1;
            try
            {
                var executingService = await _orderService.GetExecutingServiceAsync(orderId, "mngopen");
                report.ManagerName = executingService?.Employee?.FullName;
            }
            catch { }
            await _htmlToPdfHelper.ConvertHtml2PdfResponseAsync(this, "ReportView/OrderForClientAcceptanceCar", report);
        }

        // получить Акт-сдачи приемки запчастей
        public async Task GetReportAcceptanceCustomPartsPdf(int? acceptanceId)
        {
            var report = new AcceptedPartsReportViewModel();
            var doc = await _orderService.GetAcceptDocumentByIdAsync(acceptanceId ?? 0);
            try
            {
                if (doc != null)
                {
                    var client = doc.Client;
                    report.NameClient = client.FullName.ConvertToShortName();
                    report.Phone = client.Phone;
                    report.Adress = client.Address;
                    report.DocumentId = acceptanceId.Value;
                    report.NameEmployee = doc.Employee.FullName.ConvertToShortName();
                    report.Date = doc.AcceptDate.Value;
                    report.SpareParts = doc.AcceptanceCustomSparts.ToList();
                }
            }
            catch { }
            await _htmlToPdfHelper.ConvertHtml2PdfResponseAsync(this, "ReportView/ClientPartsAcceptReport", report);
        }
    }
}