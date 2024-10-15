using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;
using CarServiceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    public class AcceptanceCustomsController(
        IClientService clientService,
        IInvoiceService invoiceService,
        IEmployeeService employeeService,
        IProviderService providerService,
        ISparePartMaterialService sparePartMaterialService,
        IUsingPartMaterialService usingPartMaterialService,
        IAcceptanceDocumentService acceptanceDocumentService
            ) : Controller
    {
        private readonly DateTime currentDateTime = DateTime.Now;
        private const int TakeValue10 = 10;
        private const int TakeValue5 = 5;
        private const string ConstNotFoundMessage = "Нет данных по запросу...";

        private readonly IClientService _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        private readonly IInvoiceService _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        private readonly IEmployeeService _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        private readonly IProviderService _providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));
        private readonly ISparePartMaterialService _sparePartMaterialService = sparePartMaterialService ?? throw new ArgumentNullException(nameof(sparePartMaterialService));
        private readonly IUsingPartMaterialService _usingPartMaterialService = usingPartMaterialService ?? throw new ArgumentNullException(nameof(usingPartMaterialService));
        private readonly IAcceptanceDocumentService _acceptanceDocumentService = acceptanceDocumentService ?? throw new ArgumentNullException(nameof(acceptanceDocumentService));

        public async Task<ActionResult> Manage()
        {
            var paramAccept = new AcceptanceCustomParamsViewModel
            {
                ListEmployee = _employeeService.Employees(),
                ListClients = await _clientService.ClientsAsync(TakeValue5)
            };

            return View(paramAccept);
        }

        [HttpPost]
        public async Task<ActionResult> SearchParts(string id, string name)
        {
            var spareParts = await _sparePartMaterialService
                .GetAsync(id, name, TakeValue10);

            if (spareParts.Count != 0)
                return PartialView("SearchPartResults", spareParts);

            // Response.Write(ConstNotFoundMessage);
            ModelState.AddModelError(string.Empty, $"Значение не найдено");

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> AddPart(string number, string state, int? documentId, string partId)
        {
            if (!documentId.HasValue)
                return null;

            if (double.TryParse(number, out var numberValue))
            {
                var status = await _acceptanceDocumentService.AddCustomPartInvoiceAsync(numberValue, state, partId, documentId ?? 0);

                if (status == -1)
                    return Json(partId);

                var viewModel = GetAcceptedParts(documentId.Value);

                return PartialView("UpdateTable", viewModel);
            }

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> AddInvoice(AcceptanceDocument acceptanceDocument)
        {
            if (ModelState.IsValid)
            {
                var foundDocument = await _acceptanceDocumentService
                .AcceptanceDocumentAsync(acceptanceDocument.AcceptanceId);

                if (foundDocument == null)
                {
                    if (acceptanceDocument.AcceptDate == null)
                    {
                        acceptanceDocument.AcceptDate = currentDateTime;
                    }
                    // var acceptId = new ObjectParameter("acceptanceId", typeof(int));

                    var id = await _acceptanceDocumentService.AddCustomInvoiceAsync(acceptanceDocument.ClientId, acceptanceDocument.PersonnelNumber, acceptanceDocument.AcceptDate);
                    acceptanceDocument.AcceptanceId = id;

                    if (id == -1)
                        return Json("Неверно введены параметры");

                    var infoInvoice = await _acceptanceDocumentService.GetInfoCustomInvoiceByIdAsync(acceptanceDocument.AcceptanceId);

                    return PartialView("EmptyTableInvoice", infoInvoice);
                }
                else
                {
                    if (foundDocument.AcceptanceCustomSparts?.Count > 0)
                    {
                        var viewModel = GetAcceptedParts(acceptanceDocument.AcceptanceId);
                        return PartialView("UpdateTable", viewModel);
                    }

                    var infoInvoice = _acceptanceDocumentService.GetInfoCustomInvoiceByIdAsync(acceptanceDocument.AcceptanceId);

                    return PartialView("EmptyTableInvoice", infoInvoice);
                }
            }

            return Json("Неверно введены параметры");
        }

        [HttpPost]
        public async Task<ActionResult> SearchInvoice(int? AcceptanceId)
        {
            if (AcceptanceId.HasValue)
            {
                var viewModel = await GetAcceptedParts(AcceptanceId.Value);

                if (viewModel.CustomInvoiceInfo == null)
                {
                    ModelState.AddModelError(string.Empty, $"Акт сдачи-приемки под номером{AcceptanceId} отсутсвует в БД");
                    return View();
                    // return JavaScript("$('#popErorr').html('Акт сдачи-приемки под номером № " + AcceptanceId + " отсутсвует в БД').slideDown(1000).delay(3000).slideUp(1000); ");
                }
                if (viewModel.AcceptencedParts.Count > 0)
                    return PartialView("UpdateTable", viewModel);

                return PartialView("EmptyTableInvoice", viewModel.CustomInvoiceInfo);
            }

            return null;
        }

        public async Task<ActionResult> Delete(string partId, int? acceptanceId)
        {
            if (!string.IsNullOrWhiteSpace(partId) && acceptanceId.HasValue)
            {
                var deletedPart = await _acceptanceDocumentService.DeletePartAsync(partId, acceptanceId);

                if (deletedPart == -1)
                {
                    ModelState.AddModelError(string.Empty, $"Данный материал (запчасть) не может быть исключена из документа, так как уже задействована в услугах (работах)");
                    return View();
                    // return JavaScript("$('#popErorr').html('Данный материал (запчасть) не может быть исключена из документа, так как уже задействована в услугах (работах)').slideDown(1000).delay(3000).slideUp(1000); ");
                }

                var viewModel = await GetAcceptedParts(acceptanceId.Value);

                return viewModel.AcceptencedParts.Count > 0
                    ? PartialView("UpdateTable", viewModel)
                    : PartialView("EmptyTableInvoice", viewModel.CustomInvoiceInfo);
            }

            return null;
        }

        public async Task<ActionResult> DeleteInvoice(int? acceptanceId)
        {
            if (acceptanceId.HasValue)
            {
                // var inputParams = new Dictionary<string, object> { { "acceptanceId", acceptanceId } };
                var statusValue = await _invoiceService.DeleteCustomInvoiceAsync(acceptanceId.Value);

                switch (statusValue)
                {
                    case -1: return null;
                    case -2:
                        {
                            ModelState.AddModelError(string.Empty, $"Ошибка");
                            return View();
                            // return JavaScript("$('#popErorr').html('" + dbConnection.InfoMessage + "').slideDown(1000).delay(3000).slideUp(1000); ");
                        }
                    default:
                        // return JavaScript("$('#updTable').html(''); $('#popSuccess').html('Акт сдачи-приемки №" + acceptanceId + " удален из БД!').slideDown(1000).delay(3000).slideUp(1000); ");
                        {
                            ModelState.AddModelError(string.Empty, $"Акт сдачи-приемки {acceptanceId} удален из БД!");
                            return View();
                            // return JavaScript("$('#popErorr').html('" + dbConnection.InfoMessage + "').slideDown(1000).delay(3000).slideUp(1000); ");
                        }
                }
            }

            return null;
        }

        public async Task<ActionResult> EditInvoice(int? acceptanceId)
        {
            if (acceptanceId.HasValue)
            {
                var infoInvoice = await _acceptanceDocumentService.GetInfoCustomInvoiceByIdAsync(acceptanceId);
                if (infoInvoice != null)
                {
                    //  Session["InfoEditInvoice"] = infoInvoice;
                    ViewBag.ListEmployee = _employeeService.Employees();
                    ViewBag.ListClients = await _clientService.ClientsAsync(TakeValue5);

                    return PartialView("EditInvoice", infoInvoice);
                }
            }

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> EditSaveInvoice(AcceptanceDocument acceptanceDocument)
        {
            if (ModelState.IsValid)
            {
                await _acceptanceDocumentService.CreateAsync(acceptanceDocument);
                var viewModel = GetAcceptedParts(acceptanceDocument.AcceptanceId);

                return PartialView("UpdateTable", viewModel);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> CancelEditInvoice(int? acceptanceId)
        {
            if (acceptanceId.HasValue)
            {
                var infoInvoice = await _acceptanceDocumentService.GetInfoCustomInvoiceByIdAsync(acceptanceId);
                if (infoInvoice != null)
                {
                    //infoInvoice = Session["InfoEditInvoice"] as GetInfoCustomInvoiceByIdDoc_Result;
                    return PartialView("RefreshInfoInvoice", infoInvoice);
                }
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> ClearInvoice(int? acceptanceId)
        {
            if (acceptanceId.HasValue)
            {
                {
                    //var inputParams = new Dictionary<string, object> { { "acceptanceId", acceptanceId } };
                    var status = await _acceptanceDocumentService.ClearCustomInvoiceAsync(acceptanceId.Value);
                    if (status == 0)
                    {
                        var viewModel = await GetAcceptedParts(acceptanceId.Value);

                        return PartialView("EmptyTableInvoice", viewModel.CustomInvoiceInfo);
                    }

                    ModelState.AddModelError(string.Empty, $"Ошибка очистки");
                    return View();
                    //return JavaScript("$('#popErorr').html('" + dbConnection.InfoMessage + "').slideDown(1000).delay(3000).slideUp(1000); ");
                }
            }

            return null;
        }

        public async Task<ActionResult> SearchClients(string id, string name)
        {
            IEnumerable<Client> clients = await _clientService.SearchClientsAsync(id, name, TakeValue5);

            return PartialView("ResultSearchClient", clients.ToList());
        }

        // [ChildActionOnly]
        public async Task<AcceptedCustomPartsViewModel> GetAcceptedParts(int docId)
        {
            var acceptedParts = await _acceptanceDocumentService.GetAcceptedCustomPartsAsync(docId);
            var infoInvoice = await _acceptanceDocumentService.GetInfoCustomInvoiceByIdAsync(docId);
            var viewModel = new AcceptedCustomPartsViewModel
            {
                AcceptencedParts = acceptedParts,
                CustomInvoiceInfo = infoInvoice
            };

            return viewModel;
        }
    }
}