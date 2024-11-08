using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Extension;
using CarServiceApp.Domain.Service.Abstract;
using CarServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarServiceDiplom.Controllers
{
    public class PreRecordsController(
        IPreRecordService preRecordService) : Controller
    {
        private readonly IPreRecordService _preRecordService = preRecordService ?? throw new ArgumentNullException(nameof(preRecordService));
        private const int TakeValue6 = 6;

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PreRecordRecivedDataViewModel model)
        {
            var employeeId = "11";
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(employeeId))
            {
                var returnValue = await _preRecordService.CreatePreRecordAsync(
                    model.FullName,
                    model.PhoneNumber,
                    model.MarkModelCar,
                    model.IssueYear,
                    model.RegNumberCar,
                    employeeId,
                    model.ServiceIds);

                return RedirectToAction("Details", new { RecordId = returnValue.Item2 });
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SearchService(string name, string id)
        {
            var services = await _preRecordService.GetServicesByNameAsync(name, id, TakeValue6);
            return PartialView("ServiceSearchPartial", services);
        }

        [HttpPost]
        public async Task<ActionResult> AttachService(string serviceId, long? recordId)
        {
            if (recordId.HasValue && serviceId != null)
            {
                if (await _preRecordService.GetPreRecordServiceAsync(recordId, serviceId) == null)
                {
                    var services = await _preRecordService.AddAsync(recordId, serviceId);

                    return PartialView("ServicesAttachedPartial", services);
                }

                return NotFound();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAttachService(string serviceId, long? recordId)
        {
            if (serviceId != null && recordId.HasValue)
            {
                var preRecordService = await _preRecordService.GetPreRecordServiceAsync(recordId, serviceId);
                if (preRecordService != null && preRecordService.DateReservation == null)
                {
                    await _preRecordService.DeletePreRecordServiceAsync(preRecordService);
                    return null;
                }
                ModelState.AddModelError(string.Empty, "Невозможно удалить услугу, так как она уже  в таблице загрузок мастеров или удалена");
                return View(preRecordService);
            }
            return NotFound();
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(SearchPreRecordsViewModel model, int? page)
        {
            long recordIntervalDown, recordIntervalUp;
            if (model.RecordId.HasValue)/// для получения диапазона номеров записей, при Null в запросе
            {
                recordIntervalDown = model.RecordId.Value;
                recordIntervalUp = model.RecordId.Value;
            }
            else
            {
                recordIntervalDown = 0;
                recordIntervalUp = 10000000;
            }

            var preRecords = await _preRecordService.GetPreRecordsAsync(
                model.NameClient,
                model.RegNumber,
                model.MarkCar,
                recordIntervalDown,
                recordIntervalUp,
                model.DateMakingFrom,
                model.DateMakingBefore,
                model.IsRejection,
                page ?? 1,
                TakeValue6);

            return PartialView("SearchResultPartial", preRecords);
        }

        public async Task<IActionResult> DeleteRecord(long? recordId)
        {
            if (recordId.HasValue)
            {
                var record = await _preRecordService.GetPreRecordByIdAsync(recordId.Value);
                if (record != null)
                {
                    await _preRecordService.DeletePreRecordAsync(record);

                    if (Request.Headers["Referer"].ToString() == "/PreRecords/Details")
                        return Redirect("/PreRecords/List");
                    return Redirect(Request.Headers["Referer"].ToString());
                }
            }
            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        public async Task<ActionResult> Details(long? recordId)
        {
            var preRecord = await _preRecordService.GetPreRecordByIdAsync(recordId ?? 0);
            if (preRecord != null)
            {
                return View(preRecord);
            }

            return NotFound();
        }

        //public async Task<ActionResult> CastToOrder(long? recordId)
        //{
        //    var record = await _preRecordService.GetPreRecordByIdAsync(recordId ?? 0);
        //    if (record != null)
        //    {
        //        ViewBag.Record = recordId;
        //        var namesClient = record.FullName.ConvertToStringNames();// имена раздельно
        //        var clientComparer = new ClientComparer();
        //        var firstName = namesClient[0];
        //        var name = namesClient[1];
        //        var targetClients = await _preRecordService.GetClientAsync(name, firstName, string.Empty); //содержит ли поле NameSerName имя и фамилию
        //        if (targetClients.Count() == 0)// если нет, то проверяем по 3 м буквам фамилии
        //        {
        //            string subStringName;
        //            var sizePart = firstName.Length / 3;

        //            List<Client> unionSetClients;
        //            if (firstName.Length >= 3)//если фамилия не менее 3 символов
        //            {
        //                if (sizePart < 3)
        //                    sizePart = 3;
        //                subStringName = firstName.Substring(0, sizePart);
        //                unionSetClients = await _preRecordService.GetClientAsync(string.Empty, string.Empty, subStringName); ;
        //                for (var n = sizePart; n < firstName.Length - sizePart; n += sizePart) //разделяем фамилию на три части и ищем вхождения по каждой части объединяя наборы данных в один
        //                {
        //                    subStringName = firstName.Substring(n, sizePart);
        //                    unionSetClients = unionSetClients.Union(await _preRecordService.GetClientAsync(string.Empty, string.Empty, subStringName), clientComparer).ToList();
        //                }
        //            }
        //            else
        //            {
        //                subStringName = firstName;
        //                unionSetClients = await _preRecordService.GetClientAsync(string.Empty, string.Empty, subStringName);
        //            }
        //            if (unionSetClients.Count >= 1)// если нашли по части фамилии, то показываем пользователю варианты для выбора
        //                return PartialView("ListClients", unionSetClients.Take(TakeValue6));

        //            return JavaScript("javascript:window.location.href='" + Url.Action("CreateNewClient", "Client", new { recordId }) + "'");// если нет вариантов, то переходим сразу на регистрацию нового клиента
        //        }

        //        return targetClients.Count > 0 ? // если сразу нашли по имени и фамилии, то показываем пользователю кого нашли
        //            PartialView("ListClients", targetClients)
        //            : null;
        //    }

        //    return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        //}

        public async Task<IActionResult> CastToOrder(long? recordId)
        {
            var record = await _preRecordService.GetPreRecordByIdAsync(recordId ?? 0);
            if (record != null)
            {
                ViewBag.RecordId = recordId;
                var namesClient = record.FullName.ConvertToStringNames(); // предположим, что здесь есть конвертация к типу string[]
                var firstName = namesClient[0];
                var name = namesClient[1];
                var targetClients = await _preRecordService.GetClientAsync(name, firstName, string.Empty);

                if (targetClients.Count == 0)
                {
                    string subStringName;
                    var sizePart = firstName.Length / 3;

                    if (firstName.Length >= 3)
                    {
                        if (sizePart < 3)
                            sizePart = 3;

                        subStringName = firstName[..sizePart];
                        var unionSetClients = await _preRecordService.GetClientAsync(string.Empty, string.Empty, subStringName);

                        for (var n = sizePart; n < firstName.Length - sizePart; n += sizePart)
                        {
                            subStringName = firstName.Substring(n, sizePart);
                            unionSetClients = unionSetClients.Union(await _preRecordService.GetClientAsync(string.Empty, string.Empty, subStringName)).ToList();
                        }

                        if (unionSetClients.Count >= 1)
                            return PartialView("ListClients", unionSetClients.Take(TakeValue6));

                        return RedirectToAction("CreateNewClient", "Client", new { recordId });
                    }
                    else
                    {
                        subStringName = firstName;
                        var unionSetClients = await _preRecordService.GetClientAsync(string.Empty, string.Empty, subStringName);

                        if (unionSetClients.Count >= 1)
                            return PartialView("ListClients", unionSetClients.Take(TakeValue6));

                        return RedirectToAction("CreateNewClient", "Client", new { recordId });
                    }
                }

                return targetClients.Count != 0 ? PartialView("ListClients", targetClients) : null;
            }

            return NotFound();
        }

        public async Task<ActionResult> ChangeStatlRecord(long? recordId)
        {
            var record = await _preRecordService.GetPreRecordByIdAsync(recordId ?? 0);
            if (record != null)
            {
                if (record.IsRejection != null && record.IsRejection.Value)
                {
                    record.IsRejection = false;
                    await _preRecordService.UpdatePreRecordync(record);
                }
                else
                {
                    record.IsRejection = true;
                    await _preRecordService.UpdatePreRecordync(record);
                }

                return View("Details", record);
            }

            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }
    }

    internal class ClientComparer : IEqualityComparer<Client>// компаратор для сравнения объектов модели Client
    {
        public bool Equals(Client first, Client second)
        {
            return first?.ClientId == second?.ClientId;
        }

        public int GetHashCode(Client obj)
        {
            return obj.GetHashCode();
        }
    }
}