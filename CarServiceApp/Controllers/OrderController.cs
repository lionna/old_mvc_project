using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Extension;
using CarServiceApp.Domain.Service.Abstract;
using CarServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace CarServiceApp.Controllers
{
    public class OrderController(
        IHttpContextAccessor httpContextAccessor,
        IOrderService orderService,
        IClientService clientService) : Controller
    {
        private const int PageSize5 = 5;
        private const int PageSize8 = 8;
        private const int TakeItems = 20;

        private readonly IOrderService _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        private readonly IClientService _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        [HttpGet]
        public async Task<ActionResult> AddOrder(string id, long? recordId)
        {
            if (id != null)
            {
                var client = await _clientService.GetAsync(id);
                var order = new OrderViewModel
                {
                    ClientId = id ?? throw new ArgumentNullException(nameof(id)),
                    RecordId = recordId ?? throw new ArgumentNullException(nameof(recordId)),
                    FullName = client?.FullName,
                    ListEmplyees = await _orderService.GetEmployeesAsync(true, 4)
                };

                ViewBag.CarVINs = await _orderService.GetIncludedCarByClientIdAsync(id);
                ViewBag.ShortName = client?.FullName?.ConvertToShortName();

                return View(order);
            }

            return RedirectToAction("Search");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrder(OrderViewModel order)
        {
            string orderId;

            if (ModelState.IsValid)
            {
                if (order.RecordId == null)
                {
                    var value = await _orderService.AddPartOrderAsync(
                        order.DateMakingOrder, order.ClientId, order.VinNumber, order.Descriptions,
                        order.CurrentMileageCar, order.PersonnelNumber,
                        (byte)(order.NumberWheelCaps != null ? order.NumberWheelCaps.Length : 0),
                        (byte)(order.NumberWipers != null ? order.NumberWipers.Length : 0),
                        (byte)(order.NumberWipersArms?.Length ?? 0), order.IsAntenna, order.IsSpareWheel,
                        order.IsCoverDecorEngine, order.IsTuner, order.FluelLevelPercent);

                    if (!string.IsNullOrWhiteSpace(value.errorMessage))
                    {
                        ModelState.AddModelError(string.Empty, value.errorMessage);
                    }

                    orderId = value.orderId;
                }
                else
                {
                    var value = await _orderService
                        .AddPartialOrderForRecordAsync(order.DateMakingOrder, order.ClientId,
                        order.VinNumber, order.Descriptions, order.CurrentMileageCar,
                        order.PersonnelNumber, order.RecordId,
                        (byte)(order.NumberWheelCaps != null ? order.NumberWheelCaps.Length : 0),
                        (byte)(order.NumberWipers != null ? order.NumberWipers.Length : 0),
                        (byte)(order.NumberWipersArms?.Length ?? 0), order.IsAntenna, order.IsSpareWheel,
                        order.IsCoverDecorEngine, order.IsTuner, order.FluelLevelPercent);

                    if (!string.IsNullOrWhiteSpace(value.errorMessage))
                    {
                        ModelState.AddModelError(string.Empty, value.errorMessage);
                    }
                    orderId = value.orderId;
                }

                //if (Session["car_marks"] is List<CarMarkViewModel> carMarks)
                //{
                //    foreach (var item in carMarks)
                //    {
                //        var remarkCar = new RemarkToStateCar { XAxisPos = item.X_Axis.Value, YAxisPos = item.Y_Axis.Value, OrderId = orderId.Value as string, NumberType = item.TypeMark, RemarkText = item.addonInfo };
                //        await _orderService.AddRemarkToStateCarAsync(remarkCar);
                //    }
                //}

                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext.Session.TryGetValue("car_marks", out byte[] data))
                {
                    var carMarks = System.Text.Json.JsonSerializer.Deserialize<List<CarMarkViewModel>>(data);
                    if (carMarks != null)
                    {
                        foreach (var item in carMarks)
                        {
                            var remarkCar = new RemarkToStateCar { XAxisPos = item.X_Axis.Value, YAxisPos = item.Y_Axis.Value, OrderId = orderId as string, NumberType = item.TypeMark, RemarkText = item.addonInfo };
                            await _orderService.AddRemarkToStateCarAsync(remarkCar);
                        }
                    }
                }

                return RedirectToAction("CaryOnOrder", new { OrderId = orderId });
            }

            return NotFound("Либо модель не валидна, либо нет заказа!");
        }

        [HttpGet]
        public async Task<ActionResult> CaryOnOrder(string orderId)
        {
            if (orderId != null)
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);

                ViewBag.ModelName = "[" + order.Car.CarModification.Series.CarModel.CarBrand.Name + "] " + order.Car.CarModification.Series.Name;

                var listEmployees = await _orderService.GetEmployeesAsync(true, 4);
                ViewBag.ShortName = order.Client.FullName.ConvertToShortName();
                ViewBag.ListTypeServ = await _orderService.GetServiceTypesAsync();
                ViewBag.ListOfPayment = await _orderService.GetTypeOfPaymentsAsync();
                ViewBag.ListEmployee = listEmployees ?? throw new ArgumentNullException(nameof(listEmployees));
                ViewBag.TotalInfoForTable = await _orderService.GetTotalInfoExecServicesByIdAsync(orderId);

                //выполнение процедуры
                var result = await _orderService.GetCalculatedTotalMoneyTimeByOrderAsync(orderId);
                //вносим в репозиторий c округлением до 2х знаков
                ViewData.Add("totMoneyServ", decimal.Round(result.Item1, 2));
                ViewData.Add("totMoneyServFull", decimal.Round(result.Item2, 2));
                ViewData.Add("totTimeServ", result.Item3);
                ViewData.Add("totMoneyPartsFull", decimal.Round((decimal)result.Item4, 2));
                ViewData.Add("totMoneyFullByOrder", decimal.Round(result.Item4 + result.Item2, 2));

                return order.DateFactCompleting == null ? View(order) : View("CaryOnOrderClosed", order);
            }

            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public async Task<ActionResult> CaryOnOrderClosed(string orderId)
        {
            if (orderId != null)
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);

                ViewBag.ModelName = "[" + order.Car.CarModification.Series.CarModel.CarBrand.Name + "] " + order.Car.CarModification.Series.Name;

                var listEmployees = await _orderService.GetEmployeesAsync(true, 4);
                ViewBag.ShortName = order.Client.FullName.ConvertToShortName();
                ViewBag.ListTypeServ = await _orderService.GetServiceTypesAsync();
                ViewBag.ListOfPayment = await _orderService.GetTypeOfPaymentsAsync();
                ViewBag.ListEmployee = listEmployees ?? throw new ArgumentNullException(nameof(listEmployees));
                ViewBag.TotalInfoForTable = await _orderService.GetTotalInfoExecServicesByIdAsync(orderId);

                //выполнение процедуры
                var result = await _orderService.GetCalculatedTotalMoneyTimeByOrderAsync(orderId);
                //вносим в репозиторий c округлением до 2х знаков
                ViewData.Add("totMoneyServ", decimal.Round(result.Item1, 2));
                ViewData.Add("totMoneyServFull", decimal.Round(result.Item2, 2));
                ViewData.Add("totTimeServ", result.Item3);
                ViewData.Add("totMoneyPartsFull", decimal.Round((decimal)result.Item4, 2));
                ViewData.Add("totMoneyFullByOrder", decimal.Round(result.Item4 + result.Item2, 2));

                return order.DateFactCompleting == null ? View(order) : View("CaryOnOrderClosed", order);
            }

            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<ActionResult> ServiceSearch(string serviceName, int typeId, int? page)
        {
            page ??= 1;

            var services = await _orderService.GetServicesAsync(serviceName, typeId, page.Value, PageSize5);

            if (services.TotalItems == 0)
            {
                return NotFound();
            }

            return PartialView(services);
        }

        [HttpGet]
        public async Task<ActionResult> AddService(string serviceId, string orderId)
        {
            var service = await _orderService.GetServiceByIdAsync(serviceId);
            var listEmployees = await _orderService.ListEmployeeAsync(serviceId);
            var execServiceViewModel = new ServiceViewModel
            {
                Service = service ?? throw new ArgumentNullException(nameof(service)),
                ListEmployees = listEmployees ?? throw new ArgumentNullException(nameof(listEmployees)),
                OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId)),
                TaxAddedValue = 20
            };

            return PartialView(execServiceViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddService(ServiceViewModel serviceModel)
        {
            var result = 0;

            if (ModelState.IsValid)
            {
                result = await _orderService.AddServiceToOrderAsync(serviceModel.OrderId,
                    serviceModel.ServiceId,
                    serviceModel.DateCompleting,
                    serviceModel.TakeTime,
                    serviceModel.Notes,
                    serviceModel.PersonnelNumber,
                    serviceModel.TaxAddedValue,
                    false);
            }

            ViewBag.IsSuccess = result > 0 ? true.ToString() : false.ToString();
            ViewBag.IsAdding = true;

            var execServices = await _orderService.GetExecutingServiceByOrderIdAsync(serviceModel.OrderId);
            ViewBag.TotalInfoForTable = await _orderService.GetTotalInfoExecServicesByIdAsync(serviceModel.OrderId);

            return PartialView("AddServiceToTable", execServices);
        }

        [HttpGet]
        public async Task<ActionResult> EditService(string serviceId, string orderId)
        {
            var service = await _orderService.GetServiceByIdAsync(serviceId);
            var listEmployees = await _orderService.ListEmployeeAsync(serviceId);
            var attachedService = await _orderService.GetExecutingServiceAsync(orderId, serviceId);
            var execServViewModel = new ServiceViewModel
            {
                Service = service ?? throw new ArgumentNullException(nameof(service)),
                ListEmployees = listEmployees ?? throw new ArgumentNullException(nameof(listEmployees)),
                OrderId = orderId ?? throw new ArgumentNullException(nameof(orderId)),
                PersonnelNumber = attachedService?.PersonnelNumber,
                TakeTime = attachedService?.TakeTime,
                DateCompleting = attachedService?.DateCompleting,
                Notes = attachedService?.Notes,
                TaxAddedValue = attachedService?.TaxAddedValue ?? 0,
                IsEnableChange = attachedService?.DateStart == null,
                ServiceId = attachedService?.ServiceId,
            };

            return PartialView(execServViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> EditService(ServiceViewModel serviceModel)
        {
            var result = 0;
            if (ModelState.IsValid)
            {
                result = await _orderService.AddServiceToOrderAsync(serviceModel.OrderId,
                    serviceModel.ServiceId,
                    serviceModel.DateCompleting,
                    serviceModel.TakeTime,
                    serviceModel.Notes,
                    serviceModel.PersonnelNumber,
                    serviceModel.TaxAddedValue,
                    true);
            }
            ViewBag.IsSuccess = result > 0 ? true.ToString() : false.ToString();
            ViewBag.IsAdding = false;
            var execServices = await _orderService.GetExecutingServiceByOrderIdAsync(serviceModel.OrderId);
            ViewBag.TotalInfoForTable = await _orderService.GetTotalInfoExecServicesByIdAsync(serviceModel.OrderId);

            return PartialView("AddServiceToTable", execServices);
        }

        [HttpGet]
        public async Task<ActionResult> DisplayOrdersByClient(string clientId)
        {
            var orders = await _orderService.GetOrderServicesByClientIdAsync(clientId);
            var client = orders.FirstOrDefault().Client;
            if (client != null)
                ViewBag.ShortName = client.FullName.ConvertToShortName();

            ViewBag.CountOfOrders = orders.Count();
            var searchModel = new SearchOrdersViewModel
            {
                ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId))
            };

            return View(searchModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            if (!string.IsNullOrWhiteSpace(orderId))
            {
                var result = await _orderService.DeleteOrderAsync(orderId);
                if (result > 0)
                {
                    ModelState.AddModelError(string.Empty, "Заказ удален!");
                    return View();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Невозможно удалить заказ! В нем уже задействованы услуги.");
                    return View();
                }
            }
            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAttachedService(string orderId, string serviceId)
        {
            if (orderId != null && serviceId != null)
            {
                var result = await _orderService.DeleteAttachedServiceAsync(serviceId, orderId);
                if (result > 0)
                {
                    ModelState.AddModelError(string.Empty, "Заказ удален!");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Невозможно удалить заказ! В нем уже задействованы услуги.");
                }

                return View();
            }

            return RedirectToAction(nameof(CaryOnOrder), "Order", new { orderId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Close(Domain.Entity.Models.OrderService order, int PersonnelNumber)
        {
            bool isClosed;
            string orderId = order.OrderId;

            if (order.RejectionReason == null)
            {
                isClosed = await _orderService.CloseOrderAsync(
                    orderId,
                    order.DateFactCompleting,
                    order.DateCompleting,
                    order.PaymentId,
                    PersonnelNumber,
                    true,
                    null) > 0;
            }
            else
            {
                isClosed = await _orderService.CloseOrderAsync(orderId, order.DateFactCompleting, order.DateCompleting, null, PersonnelNumber, false, order.RejectionReason) > 0;
            }

            if (isClosed)
            {
                ModelState.AddModelError(string.Empty, $"Заказ [{orderId}] успешно закрыт!");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Невозможно закрыть заказ [{orderId}]!");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> AutocompleteSearch(string term, string clientId)
        {
            clientId ??= string.Empty;
            term ??= string.Empty;
            var orderId = await _orderService.GetOrderServiceIdsAsync(term, clientId, TakeItems);

            return Json(orderId);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(SearchOrdersViewModel searchOrder, int? page)
        {
            string fieldSort = searchOrder.NumbSortRow switch
            {
                1 => "NameSerName",
                3 => "DateMakingOrder",
                2 => "Model",
                _ => "DateMakingOrder",
            };
            var searchResult = await _orderService.SearchOrdersAsync(searchOrder.ClientId,
                                                 searchOrder.FullName,
                                                 searchOrder.OrderId,
                                                 searchOrder.DateMakingFrom,
                                                 searchOrder.DateMakingBefore,
                                                 searchOrder.IsClosed,
                                                 searchOrder.IsRejection,
                                                 page ?? 1,
                                                 PageSize8,
                                                 fieldSort,
                                                 searchOrder.IsAsc);

            return PartialView("SearchResults", searchResult);
        }

        public async Task<ActionResult> TotalInfoServiceByOrder(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var execServices = await _orderService.GetTotalInfoServicesByOrderAsync(id);

                return execServices != null
                    ? PartialView("TableTotalInfoService", execServices.ToList())
                    : null;
            }

            return null;
        }

        public async Task<ActionResult> TotalInfoAttachedPartByOrder(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var attachedParts = await _orderService.GetAllInfoAttachedPartByIdAsync(id);

                return attachedParts != null
                    ? PartialView("TableTotalAttachedParts", attachedParts.ToList())
                    : null;
            }

            return null;
        }

        public async Task<ActionResult> TotalInfoAttachedCustomPartByOrder(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var attachedParts = await _orderService.GetUsingCustomSpartMatAsync(id);

                return PartialView("TableTotalAttachedCustomParts", attachedParts.ToList());
            }

            return null;
        }

        //[HttpPost]
        //public JsonResult AddRemarkItem(CarMarkViewModel carMark)
        //{
        //    if (carMark.tempID == 1 && !carMark.IsEdit) //добаляем новый список элементов если это первый элемент
        //    {
        //        var carMarks = new List<CarMarkViewModel> { carMark };
        //        Session["car_marks"] = carMarks;
        //    }
        //    else
        //    {
        //        if (Session["car_marks"] != null)// иначе проверяем есть ли сессия и добавляем в уже существующий список
        //        {
        //            var carMarks = Session["car_marks"] as List<CarMarkViewModel>;
        //            if (carMark.IsEdit)
        //            {
        //                var remark = carMarks?.Find(f => f.tempID == carMark.tempID.Value);
        //                if (remark != null)
        //                {
        //                    remark.TypeMark = carMark.TypeMark;
        //                    remark.addonInfo = carMark.addonInfo;
        //                }
        //            }
        //            else
        //                carMarks.Add(carMark);
        //        }
        //        else return Json(new { stat = 0 });
        //    }
        //    return Json(new { stat = 1, carMark.IsEdit });
        //}

        [HttpPost]
        public IActionResult AddRemarkItem(CarMarkViewModel carMark)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (carMark.tempID == 1 && !carMark.IsEdit)
            {
                var carMarks = new List<CarMarkViewModel> { carMark };
                var serializedCarMarks = JsonConvert.SerializeObject(carMarks);
                httpContext.Session.SetString("car_marks", serializedCarMarks);
            }
            else
            {
                var serializedCarMarks = httpContext.Session.GetString("car_marks");
                if (serializedCarMarks != null)
                {
                    var carMarks = JsonConvert.DeserializeObject<List<CarMarkViewModel>>(serializedCarMarks);

                    if (carMark.IsEdit)
                    {
                        var remark = carMarks?.Find(f => f.tempID == carMark.tempID.Value);
                        if (remark != null)
                        {
                            remark.TypeMark = carMark.TypeMark;
                            remark.addonInfo = carMark.addonInfo;
                        }
                    }
                    else
                    {
                        carMarks.Add(carMark);
                    }

                    serializedCarMarks = JsonConvert.SerializeObject(carMarks);
                    httpContext.Session.SetString("car_marks", serializedCarMarks);
                }
                else
                {
                    return Json(new { stat = 0 });
                }
            }

            return Json(new { stat = 1, carMark.IsEdit });
        }

        //[HttpPost]
        //public JsonResult GetInfoRemarkItem(byte? tempId)
        //{
        //    if (Session["car_marks"] != null && tempId.HasValue)
        //    {
        //        var carMarks = Session["car_marks"] as List<CarMarkViewModel>;
        //        var carRemark = carMarks.Find(f => f.tempID == tempId.Value);

        //        return Json(carRemark);
        //    }

        //    return null;
        //}
        [HttpPost]
        public IActionResult GetInfoRemarkItem(byte? tempId)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext.Session.TryGetValue("car_marks", out byte[] data) && tempId.HasValue)
            {
                var serializedCarMarks = System.Text.Encoding.UTF8.GetString(data);
                var carMarks = JsonConvert.DeserializeObject<List<CarMarkViewModel>>(serializedCarMarks);
                var carRemark = carMarks.Find(f => f.tempID == tempId.Value);

                return Json(carRemark);
            }

            return Json(null);
        }

        //[HttpPost]
        //public JsonResult DeleteInfoRemarkItem(byte? tempId)
        //{
        //    if (Session["car_marks"] != null && tempId.HasValue)
        //    {
        //        var carMarks = Session["car_marks"] as List<CarMarkViewModel>;
        //        var carRemark = carMarks?.Find(f => f.tempID == tempId.Value);
        //        var statusValue = carMarks?.Remove(carRemark);
        //        return Json(new { stat = statusValue != null && (bool)statusValue });
        //    }

        //    return Json(new { stat = false });
        //}

        [HttpPost]
        public IActionResult DeleteInfoRemarkItem(byte? tempId)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext.Session.TryGetValue("car_marks", out byte[] data) && tempId.HasValue)
            {
                var serializedCarMarks = System.Text.Encoding.UTF8.GetString(data);
                var carMarks = JsonConvert.DeserializeObject<List<CarMarkViewModel>>(serializedCarMarks);
                var carRemark = carMarks?.Find(f => f.tempID == tempId.Value);
                var statusValue = carMarks?.Remove(carRemark);

                if (statusValue != null)
                {
                    var updatedSerializedCarMarks = JsonConvert.SerializeObject(carMarks);
                    httpContext.Session.SetString("car_marks", updatedSerializedCarMarks);
                    return Json(new { stat = true });
                }
            }

            return Json(new { stat = false });
        }

        [HttpPost]
        public async Task<JsonResult> GetInfoRemarkItemFromDB(string orderId)
        {
            var remarkList = await _orderService.RemarkToStateCarsAsync(orderId);
            var remarkListJson = new List<RemarkToStateCarViewModel>();

            foreach (var item in remarkList)
            {
                remarkListJson.Add(new RemarkToStateCarViewModel
                {
                    X_Axis_pos = item.XAxisPos,
                    Y_Axis_pos = item.YAxisPos,
                    OrderId = item.OrderId,
                    RemarkId = item.RemarkId,
                    NumberType = item.NumberType,
                    RemarkText = item.RemarkText
                });
            }

            return Json(remarkListJson);
        }
    }
}