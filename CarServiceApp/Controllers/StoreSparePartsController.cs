using CarServiceApp.Domain.Service.Abstract;
using CarServiceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    public class StoreSparePartsController(
        IEmployeeService employeeService,
        IAcceptanceInvoiceService acceptanceInvoiceService) : Controller
    {
        private const int PageSize8 = 8;

        private readonly IEmployeeService _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        private readonly IAcceptanceInvoiceService _acceptanceInvoiceService = acceptanceInvoiceService ?? throw new ArgumentNullException(nameof(acceptanceInvoiceService));

        public ActionResult List()
        {
            //"mngparts"
            ViewBag.ListEmployee = _employeeService.Employees(true, 5, true);

            return View();
        }

        public async Task<ActionResult> Search(SearchPartsOfStoreViewModel model)
        {
            //Session["searchParams"] = model;
            //var countOfRows = new ObjectParameter("countOfRows", typeof(int));
            var result = await _acceptanceInvoiceService.FindSparePartsAsync(model.PartId,
                                                                    model.Name,
                                                                    model.Manufacture,
                                                                    model.PriceBefore,
                                                                    model.PriceAfter,
                                                                    model.InvoiceId,
                                                                    model.StockPercent,
                                                                    model.DateIvoiceFrom,
                                                                    model.DateIvoiceBefore,
                                                                    model.PersonnelNumber,
                                                                    "Name", true, 1, 8);

            ViewBag.ListEmployee = _employeeService.Employees(false);

            return PartialView("ResultSearch", result);
        }

        [HttpPost]
        public async Task<ActionResult> SortByParam(SearchPartsOfStoreViewModel model, int? numberColumn, bool? isAccepted, int? pageNumber)
        {
            if (numberColumn.HasValue && isAccepted.HasValue)
            {
                //Session["nmb_clmn"] = nmb_clmn;  // save at session prop
                //Session["is_accend"] = is_accend;
            }
            else
            {
                //nmb_clmn = Session["nmb_clmn"] as int?;  // get from session prop if vars is null
                //is_accend = Session["is_accend"] as bool?;
            }
            //var countOfRows = new ObjectParameter("countOfRows", typeof(int));
            isAccepted ??= true;
            string nameSortColumn;
            pageNumber ??= 1;
            switch (numberColumn)
            {
                case 1: nameSortColumn = "Name"; break;
                case 2: nameSortColumn = "PartId"; break;
                case 3: nameSortColumn = "LotNumber"; break;
                case 4: nameSortColumn = "Manufacture"; break;
                case 5: nameSortColumn = "Unit"; break;
                case 6: nameSortColumn = "Stock"; break;
                case 7: nameSortColumn = "Price"; break;
                case 8: nameSortColumn = "TradeIncrease"; break;
                case 9: nameSortColumn = "Cost"; break;
                case 10: nameSortColumn = "PersonnelNumber"; break;
                case 11: nameSortColumn = "DeliveryDate"; break;
                default: nameSortColumn = "Name"; break;
            }

            //if (Session["searchParams"] is SearchPartsOfStoreViewModel model)
            //{
            var result = await _acceptanceInvoiceService.FindSparePartsAsync(model.PartId,
                                                                    model.Name,
                                                                    model.Manufacture,
                                                                    model.PriceBefore,
                                                                    model.PriceAfter,
                                                                    model.InvoiceId,
                                                                    model.StockPercent,
                                                                    model.DateIvoiceFrom,
                                                                    model.DateIvoiceBefore,
                                                                    model.PersonnelNumber,
                                                                    nameSortColumn, isAccepted, pageNumber, PageSize8
                                                                    );

            ViewBag.ListEmployee = _employeeService.Employees(false);

            return PartialView("ResultSearch", result);
            //}

            //return null;
        }

        [HttpPost]
        public async Task<ActionResult> ChangeTradeIncrease(int[] positionId, byte? inputValue)
        {
            if (positionId.Length != 0 && inputValue.HasValue)
            {
                var isError = false;
                foreach (var item in positionId)
                {
                    var position = await _acceptanceInvoiceService.GetByIdAsync(item);
                    if (position != null)
                    {
                        position.TradeIncrease = inputValue.Value;
                        await _acceptanceInvoiceService.UpdateAsync(position);
                    }
                    else
                        isError = true;
                }
                if (!isError)
                {
                    try
                    {
                        return Json(1);
                    }
                    catch (Exception)
                    {
                        return Json(-1);
                    }
                }

                return Json(-1);
            }

            return Json(-1);
        }
    }
}