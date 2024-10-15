using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    public class WorkLoadingController(IWorkLoadingService workLoadingService) : Controller
    {
        private const int TakeValue8 = 8;
        private readonly IWorkLoadingService _workLoadingService = workLoadingService ?? throw new ArgumentNullException(nameof(workLoadingService));
        private readonly DateTime DafaultCurrentDate = DateTime.Now;

        public async Task<ActionResult> Index(DateTime? currentDate)
        {
            currentDate ??= DafaultCurrentDate;
            TableLoadViewModel result = await _workLoadingService.TableLoadAsync(currentDate);
            result.CurrentDate = currentDate;
            return View(result);
        }

        public async Task<ActionResult> TableLoad(string orderId, DateTime? currentDate)
        {
            if (orderId == null)
                return NotFound();

            currentDate ??= DafaultCurrentDate;

            var tableLoad = await _workLoadingService.TableLoadAsync(currentDate);

            var model = new TableLoadViewModel();
            var order = await _workLoadingService.GetOrderServiceById(orderId);
            if (order != null)
            {
                model.Order = order;
                model.LoadEmployees = tableLoad.LoadEmployees;
                model.PreRecordServices = tableLoad.PreRecordServices;
                try
                {
                    model.NameCar = "[" + order?.Car?.CarModification?.Series?.CarModel?.CarBrand?.Name + "] " + order?.Car?.CarModification?.Series?.Name;
                }
                catch { }

                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> ReservationTime(
            string[] serviceId, string orderId, DateTime? currentDate, int? personnelNumber, int? hourSelect)
        {
            if (serviceId != null && orderId != null && currentDate.HasValue && personnelNumber.HasValue && hourSelect.HasValue)
            {
                var status = await _workLoadingService.ReservHourForServiceAsync(
                    serviceId, orderId, currentDate, personnelNumber, hourSelect);
                if (status != 1)
                {
                    ModelState.AddModelError("", "Произошла ошибка");
                    return View();
                    //return JavaScript("$('#popErorr').html('" + "Произошла ошибка" + "').slideDown(400).delay(1500).slideUp(400); ");
                }
                //return JavaScript("window.location.reload();");
                return RedirectToAction("TableLoat");
            }
            //return JavaScript("$('#popErorr').html('Ошибка запроса...').slideDown(400).delay(1500).slideUp(400); ");
            ModelState.AddModelError("", "Ошибка запроса");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteReservationTime(DateTime? currentDate, int? personnelNumber, int? vb)
        {
            if (currentDate.HasValue && personnelNumber.HasValue && vb.HasValue)
            {
                DateTime? date = new DateTime(
                    currentDate.Value.Year,
                    currentDate.Value.Month,
                    currentDate.Value.Day,
                    vb.Value, 0, 0);

                var statusValue = await _workLoadingService.CancelReservHourForServiceAsync(date, personnelNumber);

                if (statusValue < 0)
                {
                    ModelState.AddModelError("", "Произошла ошибка");
                    return View();
                }
                return RedirectToAction("TableLoat");
                //return JavaScript(statusValue < 0 ? "$('#popErorr').html('Удалить данный промежуток времени не удалось...').slideDown(400).delay(1500).slideUp(400); " : "window.location.reload();");
            }

            ModelState.AddModelError("", "Ошибка запроса");
            return View();
            //return JavaScript("$('#popErorr').html('Ошибка запроса...').slideDown(400).delay(1500).slideUp(400); ");
        }

        [HttpPost]
        public async Task<ActionResult> GetDetailsForTime(DateTime? currentDate, int? personnelNumber, int? vb)
        {
            DateTime? date;
            if (currentDate.HasValue && personnelNumber.HasValue && vb.HasValue)
            {
                date = new DateTime(currentDate.Value.Year, currentDate.Value.Month, currentDate.Value.Day, vb.Value, 0, 0);

                var execServList = await _workLoadingService.ExecutingServicesAsync(date, personnelNumber);
                var preServList = await _workLoadingService.PreRecordServiceAsync(date, personnelNumber);
                ViewBag.SelectedDate = new DateTime(currentDate.Value.Year, currentDate.Value.Month, currentDate.Value.Day).ToString("yyyy-MM-dd");
                //для основных
                if (execServList.Count > 0 && preServList.Count == 0)
                {
                    try
                    {
                        var order = execServList.FirstOrDefault()?.OrderService;
                        ViewBag.NameCar = "[" + order?.Car?.CarModification?.Series?.CarModel?.CarBrand?.Name + "] " + order?.Car?.CarModification?.Series?.Name;

                        return PartialView("InfoTimePartial", execServList);
                    }
                    catch { ViewBag.NameCar = "Error"; }
                }
                else
                {   //для предзаказа
                    if (execServList.Count == 0 && preServList.Count > 0)
                    {
                        return PartialView("InfoPreTimePartial", preServList);
                    }
                }

                ModelState.AddModelError("", "В данном заказе еще нет услуг");
                return View();

                //return JavaScript("$('#popErorr').html('В данном заказе еще нет услуг').slideDown(400).delay(1500).slideUp(400); ");
            }

            ModelState.AddModelError("", "Ошибка запроса...");
            return View();
            //return JavaScript("$('#popErorr').html('Ошибка запроса...').slideDown(400).delay(1500).slideUp(400); ");
        }

        // страница нагрузок  мастеров для предварительной записи
        public async Task<ActionResult> TableLoadPreReservation(long? preRecordId, DateTime? currentDate)
        {
            if (preRecordId == null)
                return NotFound();

            currentDate ??= DafaultCurrentDate;

            var model = await _workLoadingService.TableLoadPreRecordViewModelAsync(currentDate, preRecordId);

            return View(model);
        }

        //для предварительных заказов
        [HttpPost]
        public async Task<ActionResult> ReservationPreTime(string[] serviceId, string recordId, DateTime? currentDate, int? personnelNumber, int? hourSelect)
        {
            if (serviceId != null && recordId != null && currentDate.HasValue && personnelNumber.HasValue && hourSelect.HasValue)
            {
                var statusValue = await _workLoadingService.PreReservHourForServiceAsync(
                    serviceId, recordId, currentDate, personnelNumber, hourSelect);
                //return statusValue != 1
                //    ? JavaScript("$('#popErorr').html('" + "Произошла ошибка" + "').slideDown(400).delay(1500).slideUp(400); ")
                //    : JavaScript("window.location.reload();");

                if (statusValue != 1)
                {
                    ModelState.AddModelError("", "Произошла ошибка");
                    return View();
                    //return JavaScript("$('#popErorr').html('" + "Произошла ошибка" + "').slideDown(400).delay(1500).slideUp(400); ");
                }

                return RedirectToAction("TableLoadPreReservation");
            }
            ModelState.AddModelError("", "Произошла ошибка");
            return View();
            //return JavaScript("$('#popErorr').html('Ошибка запроса...').slideDown(400).delay(1500).slideUp(400); ");
        }

        [HttpPost]
        public async Task<ActionResult> SearchParts(string partId, string partName)
        {
            var services = await _workLoadingService.GetServicesAsync(partId, partName, TakeValue8);

            return PartialView("ServiceSearchPartial", services);
        }
    }
}