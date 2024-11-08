using CarServiceApp.Domain.Service.Abstract;
using CarServiceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    public class PartsController : Controller
    {
        //private readonly IdentityContext identityContext = new IdentityContext();
        //private readonly DbConnectionLayer dbConnection = new DbConnectionLayer("autoserviceDB");
        private const int PageSize = 12;

        private const int TakeValue6 = 6;

        //private readonly autoserviceDEntities autoServiceDb;
        private const string ConstNotFoundMessage = "Нет данных по запросу...";

        private readonly IAcceptanceInvoiceService _acceptanceInvoiceService;
        private readonly IOrderService _orderService;

        public PartsController(IAcceptanceInvoiceService acceptanceInvoiceService, IOrderService orderService)
        {
            _acceptanceInvoiceService = acceptanceInvoiceService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult> AttachSpareParts(string clientId, string serviceId, string orderId)
        {
            if (orderId != null && serviceId != null && orderId != null)
            {
                var attachedParts = await _acceptanceInvoiceService.GetAttachedPartsAsync(orderId, serviceId);
                var attachedCustomParts = await _acceptanceInvoiceService.GetAttachedCustomPartsAsync(orderId, serviceId);
                var attachedSparePart = new AttachedSparePartViewModel
                {
                    ServiceId = serviceId,
                    ClientId = clientId,
                    OrderId = orderId,
                    ServiceName = await _acceptanceInvoiceService.GetServiceName(serviceId),
                    AttachedPartsByService = attachedParts,
                    AttachedCustomPartsByService = attachedCustomParts
                };
                ViewBag.IsClosedOrder = await _orderService.IsClosedOrderAsync(orderId);
                //Session.Add("ViewModelAttachedParts", attachedSparePart);

                return PartialView(attachedSparePart);
            }

            return NotFound();
        }

        //[HttpGet]
        //public ActionResult PartsSearch(string name, string manufacture, float? price_from, float? price_to)
        //{
        //    var stockOfSpareParts = autoServiceDb.StockOfSpareParts.Where(p => p.Stock > 0 && p.Name.Contains(name) && p.Manufacture.Contains(manufacture) && (float)p.Price.Value >= (price_from ?? 0) && (float)p.Price.Value <= (price_to ?? 1000000)).Take(TakeValue6);
        //    if (stockOfSpareParts.ToArray().Length != 0)
        //    {
        //        var model = Session["ViewModelAttachedParts"] as AttachedSparePartViewModel;
        //        model.StockOfParts = stockOfSpareParts;

        //        return PartialView(model);
        //    }

        //    Response.Write(ConstNotFoundMessage);

        //    return null;
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AttachCurrentSparePart(UsingPartMaterial attachingSparePart)
        //{
        //    var user = identityContext.Users.Where(n => n.UserName == User.Identity.Name);
        //    int? personalNumber = identityContext.Users.FirstOrDefault(n => n.UserName == User.Identity.Name)?.PersonnelNumber; // получаем реальный таб номер сотрудника
        //    var result = autoServiceDb.AttachSPartToService(attachingSparePart.OrderId, attachingSparePart.ID_service, attachingSparePart.ID_position, personalNumber, attachingSparePart.Number);
        //    if (result > 0)
        //    {
        //        if (Session["ViewModelAttachedParts"] is AttachedSparePartViewModel model)
        //        {
        //            model.AttachedPartsByService = autoServiceDb.GetAttachedParts(model.OrderId, model.ID_service);

        //            return PartialView("UpdatedTableSpareParts", model);
        //        }
        //    }

        //    return JavaScript("$('#popErorr2').html('Невозможно прикрепить материал!').slideDown(300).delay(2000).slideUp(300); ");
        //}

        //[HttpPost]
        //public ActionResult DeleteAttachedPart(UsingPartMaterial attachingSparePart)
        //{
        //    if (attachingSparePart != null)
        //    {
        //        autoServiceDb.Entry(attachingSparePart).State = EntityState.Deleted;
        //        autoServiceDb.SaveChanges();
        //        if (Session["ViewModelAttachedParts"] is AttachedSparePartViewModel model)
        //        {
        //            model.AttachedPartsByService = autoServiceDb.GetAttachedParts(model.OrderId, model.ID_service);

        //            return PartialView("UpdatedTableSpareParts", model);
        //        }
        //    }

        //    return HttpNotFound();
        //}

        //[HttpPost]
        //public ActionResult DeleteAttachedCustomPart(UsingCustomSPartMat attachingCustomPart)
        //{
        //    if (attachingCustomPart != null)
        //    {
        //        autoServiceDb.Entry(attachingCustomPart).State = EntityState.Deleted;
        //        autoServiceDb.SaveChanges();
        //        if (Session["ViewModelAttachedParts"] is AttachedSparePartViewModel model)
        //        {
        //            model.AttachedCustomPartsByService =
        //                autoServiceDb.GetAttachedCustomParts(model.OrderId, model.ID_service);

        //            return PartialView("UpdatedTableCustomParts", model);
        //        }
        //    }
        //    return HttpNotFound();
        //}

        //[HttpGet]
        //public ActionResult CustomPartsSearch(string name, string manufacture, int? acceptanceID)
        //{
        //    try
        //    {
        //        var model = Session["ViewModelAttachedParts"] as AttachedSparePartViewModel;
        //        var stockCustomParts = autoServiceDb.GetStockCustomPartsById(model.ClientId).ToList().Where(m =>
        //            m.Name.Contains(name) || m.Manufacture.Contains(manufacture) ||
        //            m.ID_acceptance == (acceptanceID ?? 0));
        //        if (stockCustomParts.ToArray().Length != 0)
        //        {
        //            model.StockCustomOfParts = stockCustomParts;

        //            return PartialView(model);
        //        }

        //        Response.Write(ConstNotFoundMessage);

        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //[HttpPost]
        //public ActionResult AttachCurrentCustomPart(UsingCustomSPartMat model)
        //{
        //    var inputParams = new Dictionary<string, object> {
        //                                                        { "id_part", model.ID_part },
        //                                                        { "id_acceptance", model.ID_acceptance },
        //                                                        { "id_service", model.ID_service },
        //                                                        {"id_order", model.OrderId},
        //                                                        {"number", model.Number}
        //                                                    };

        //    var statusValue = dbConnection.ExecutionProcedure("AttachCustomPart", inputParams);
        //    if (statusValue > 0)
        //    {
        //        var attachedCustomPart = Session["ViewModelAttachedParts"] as AttachedSparePartViewModel;
        //        attachedCustomPart.AttachedCustomPartsByService = autoServiceDb.GetAttachedCustomParts(model.OrderId, model.ID_service);

        //        return PartialView("UpdatedTableCustomParts", attachedCustomPart);
        //    }

        //    return JavaScript("$('#popErorr2').html('" + dbConnection.InfoMessage + "').slideDown(300).delay(2000).slideUp(300); ");
        //}

        //public ActionResult List()
        //{
        //    return View();
        //}

        //public ActionResult Search(string namePart, string id, int? page)
        //{
        //    var parts = autoServiceDb.SparePartMaterials.Where(s => s.Name.Contains(namePart) || s.ID_part.Contains(id)).ToList();
        //    if (parts.Count == 0)
        //    {
        //        return HttpNotFound();
        //    }

        //    var pageNumber = page ?? 1;

        //    return PartialView("SearchResultPartial", parts.ToPagedList(pageNumber, PageSize));
        //}

        //public ActionResult Details(string ID_part)
        //{
        //    if (ID_part == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var parts = autoServiceDb.SparePartMaterials.Find(ID_part);
        //    if (parts == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(parts);
        //}

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(SparePartMaterial part)
        //{
        //    if (ModelState.IsValid && autoServiceDb.SparePartMaterials.Find(part.ID_part) == null)
        //    {
        //        autoServiceDb.SparePartMaterials.Add(part);
        //        autoServiceDb.SaveChanges();

        //        return RedirectToAction("Details", new { part.ID_part });
        //    }

        //    return View(part);
        //}

        //public ActionResult CheckID(string ID_part)
        //{
        //    return autoServiceDb.SparePartMaterials.Find(ID_part) != null ? Json(-1, JsonRequestBehavior.AllowGet) : null;
        //}

        //public ActionResult Edit(string ID_part)
        //{
        //    if (ID_part == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var part = autoServiceDb.SparePartMaterials.Find(ID_part);
        //    if (part == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(part);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(SparePartMaterial part)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        autoServiceDb.Entry(part).State = EntityState.Modified;
        //        autoServiceDb.SaveChanges();
        //        return RedirectToAction("Details", new { part.ID_part });
        //    }

        //    return View(part);
        //}

        //[HttpGet, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(string ID_part)
        //{
        //    var part = autoServiceDb?.SparePartMaterials?.Find(ID_part);
        //    if (part != null && part.Acceptance_Invoice?.Count == 0)
        //    {
        //        autoServiceDb.SparePartMaterials.Remove(part);
        //        autoServiceDb.SaveChanges();

        //        return Json(1, JsonRequestBehavior.AllowGet);
        //    }

        //    return Json(0, JsonRequestBehavior.AllowGet);
        //}
    }
}