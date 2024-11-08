using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;
using CarServiceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    public class AcceptanceController(
        IInvoiceService invoiceService,
        IEmployeeService employeeService,
        IProviderService providerService,
        ISparePartMaterialService sparePartMaterialService,
        IUsingPartMaterialService usingPartMaterialService
            ) : Controller
    {
        private readonly DateTime currentDateTime = DateTime.Now;
        private const int TakeValue10 = 10;
        private const int TakeValue5 = 5;
        private const string ConstNotFoundMessage = "Нет данных по запросу...";

        private readonly IInvoiceService _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        private readonly IEmployeeService _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        private readonly IProviderService _providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));
        private readonly ISparePartMaterialService _sparePartMaterialService = sparePartMaterialService ?? throw new ArgumentNullException(nameof(sparePartMaterialService));
        private readonly IUsingPartMaterialService _usingPartMaterialService = usingPartMaterialService ?? throw new ArgumentNullException(nameof(usingPartMaterialService));

        public async Task<ActionResult> Manage(int? lotNumber)
        {
            var parameterAcceptance = new AcceptanceParamsViewModel
            {
                ListEmployee = _employeeService.Employees(),
                ListProvider = await _providerService.ProvidersAsync(),
                ListProvidersSearch = await _providerService.ProvidersSearchAsync()
            };

            ViewBag.LotNumberCurrent = lotNumber;

            return View(parameterAcceptance);
        }

        [HttpPost]
        public async Task<ActionResult> SearchParts(string partId, string name)
        {
            var spareParts = await _sparePartMaterialService.GetAsync(partId, name, TakeValue10);

            if (spareParts.Count != 0)
                return PartialView("SearchPartResults", spareParts);

            // Response.Write(ConstNotFoundMessage);

            ModelState.AddModelError(string.Empty, $"Значение не найдено");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddPart(string number, string price, int? lotNumber, string partId)
        {
            if (!lotNumber.HasValue)
                return null;

            if (double.TryParse(number, out var numberValue) && decimal.TryParse(price, out var priceValue))
            {
                var status = await _invoiceService.AddSparePartInvoiceAsync(numberValue, priceValue, partId, lotNumber);
                if (status == -1)
                    return Json(partId);

                var viewModel = GetAcceptedParts(lotNumber.Value);

                return PartialView("UpdateTable", viewModel);
            }

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> AddInvoice(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                var foundInvoices = await _invoiceService.InvoiceAsync(invoice.LotNumber);
                if (foundInvoices == null && invoice.ProviderId != "0")
                {
                    await _invoiceService.AddInvoiceAsync(
                        invoice.ProviderId, invoice.PersonnelNumber, invoice.DeliveryDate, invoice.LotNumber);
                    var infoInvoice = await _invoiceService.GetInfoInvoiceByLotNumberAsync(invoice.LotNumber);

                    return PartialView("EmptyTableInvoice", infoInvoice);
                }
                else
                {
                    if (foundInvoices?.AcceptanceInvoices?.Count > 0)
                    {
                        var viewModel = GetAcceptedParts(invoice.LotNumber);
                        return PartialView("UpdateTable", viewModel);
                    }

                    var infoInvoice = await _invoiceService.GetInfoInvoiceByLotNumberAsync(invoice.LotNumber);
                    return PartialView("EmptyTableInvoice", infoInvoice);
                }
            }

            return Json("Неверно введены параметры");
        }

        [HttpPost]
        public async Task<ActionResult> SearchInvoice(int? lotNumber, DateOnly? dateLotStart, DateOnly? dateLotEnd, string providerId)
        {
            dateLotStart ??= DateOnly.MinValue;
            dateLotEnd ??= DateOnly.FromDateTime(DateTime.UtcNow);

            var invoices = await _invoiceService.GeInvoiceAsync(lotNumber, dateLotStart, dateLotEnd, providerId);
            if (invoices.Count > 0)
            {
                if (invoices.Count == 1)
                {
                    var viewModel = await GetAcceptedParts(invoices.FirstOrDefault()?.LotNumber ?? 0);
                    return
                        viewModel.AcceptencedParts.Count > 0
                        ? PartialView("UpdateTable", viewModel)
                        : PartialView("EmptyTableInvoice", viewModel.InvoiceInfo);
                }

                return PartialView("ListInvoices", invoices.OrderBy(o => o.DeliveryDate).ToList());
            }

            ModelState.AddModelError(string.Empty, $"Акт сдачи-приемки под номером {lotNumber} отсутсвует в БД");

            return View();//JavaScript("$('#popErorr').html('Акт сдачи-приемки под номером № " + lotNumber + " отсутсвует в БД').slideDown(500).delay(3000).slideUp(500); ");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? postId, int? lotNumber)
        {
            if (postId.HasValue && lotNumber.HasValue)
            {
                if ((await _usingPartMaterialService.UsingPartMaterialsAsync(postId)).Count == 0)// если позиции запчаcтей еще не были задействаованы
                {
                    await _invoiceService.DeleteAcceptanceInvoiceAsync(postId);
                    var viewModel = await GetAcceptedParts(lotNumber.Value);

                    return viewModel.AcceptencedParts.Count > 0
                        ? PartialView("UpdateTable", viewModel)
                        : PartialView("EmptyTableInvoice", viewModel.InvoiceInfo);
                }

                ModelState.AddModelError(string.Empty, $"Данный материал (запчасть) не может быть исключена из документа, так как уже задействована в услугах (работах)");
                return View();//JavaScript("$('#popErorr').html('Данный материал (запчасть) не может быть исключена из документа, так как уже задействована в услугах (работах)').slideDown(1000).delay(3000).slideUp(1000); ");
            }

            return null;
        }

        public async Task<ActionResult> DeleteInvoice(int? lotNumber)
        {
            if (lotNumber.HasValue)
            {
                var statusValue = await _invoiceService.DeleteInvoiceAsync(lotNumber.Value); ;
                switch (statusValue)
                {
                    case -1:
                        {
                            ModelState.AddModelError(string.Empty, $"Ошибка удаления lotNumber={lotNumber}");
                            return View();
                        }
                    case -2:
                        {
                            ModelState.AddModelError(string.Empty, $"Материалы данного акта сдачи-приемки уже задействованы. Невозможно удалить документ! lotNumber={lotNumber}");
                            return View();
                        }//JavaScript("$('#popErorr').html('Ошибка').slideDown(1000).delay(3000).slideUp(1000); ");
                    default:
                        {
                            ModelState.AddModelError(string.Empty, $"Акт сдачи-приемки{lotNumber} удален из БД!");
                            return View();
                        }//JavaScript("$('#updTable').html(''); $('#popSuccess').html('Акт сдачи-приемки №" + lotNumber + " удален из БД!').slideDown(1000).delay(3000).slideUp(1000); ");
                }
            }

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> EditInvoice(int? lotNumber)
        {
            if (lotNumber.HasValue)
            {
                var infoInvoice = await _invoiceService.GetInfoInvoiceByLotNumberAsync((int)lotNumber);
                if (infoInvoice != null)
                {
                    // Session["InfoEditInvoice"] = infoInvoice;
                    ViewBag.ListEmployee = _employeeService.Employees();
                    ViewBag.ListProvider = await _providerService.ProvidersSearchAsync(TakeValue5, false);

                    return PartialView("EditInvoice", infoInvoice);
                }
            }

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> EditSaveInvoice(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                await _invoiceService.DeleteInvoiceAsync(invoice);

                var viewModel = GetAcceptedParts(invoice.LotNumber);

                return PartialView("UpdateTable", viewModel);
            }
            return null;
        }

        [HttpGet]
        public async Task<ActionResult> CancelEditInvoice(int? lotNumber)
        {
            if (lotNumber.HasValue)
            {
                var infoInvoice = await _invoiceService.GetInfoInvoiceByLotNumberAsync(lotNumber ?? 0);
                if (infoInvoice != null)
                {
                    //  infoInvoice = Session["InfoEditInvoice"] as GetInfoInvoiceByLotNumber_Result;

                    return PartialView("RefreshInfoInvoice", infoInvoice);
                }
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> ClearInvoice(int? lotNumber)
        {
            if (lotNumber.HasValue)
            {
                var status = await _invoiceService.ClearInvoiceAsync(lotNumber.Value);
                if (status == 0)
                {
                    var viewModel = await GetAcceptedParts(lotNumber.Value);
                    return PartialView("EmptyTableInvoice", viewModel.InvoiceInfo);
                }
                ModelState.AddModelError(string.Empty, $"Ошибка!");
                return View();//JavaScript("$('#popErorr').html('Ошибка').slideDown(1000).delay(3000).slideUp(1000); ");
            }

            return null;
        }

        public async Task<ActionResult> SearchProvider(string id, string name)
        {
            IEnumerable<Provider> providers = await _providerService.ProvidersAsync(id, name, TakeValue5);

            return PartialView("ResultSearchProvider", providers);
        }

        //[ChildActionOnly]
        public async Task<AcceptancedPartsViewModel> GetAcceptedParts(int lotNumber)
        {
            var acceptedParts = await _sparePartMaterialService.GetAcceptancedSparePartsAsync(lotNumber);
            var infoInvoice = await _invoiceService.GetInfoInvoiceByLotNumberAsync(lotNumber);
            var viewModel = new AcceptancedPartsViewModel
            {
                AcceptencedParts = acceptedParts,
                TotallSummByParts = (decimal?)acceptedParts.FirstOrDefault()?.Total,
                InvoiceInfo = infoInvoice.FirstOrDefault()
            };

            return viewModel;
        }
    }
}