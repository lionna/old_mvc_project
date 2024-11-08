using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;
using CarServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServiceApp.Controllers
{
    public class CarsController(
        ICarService carService,
        IGenericServiceAsync<CarBrand, int> carBrandService,
        IGenericServiceAsync<CarColor, int> carColorService,
        ICommonService commonService) : Controller
    {
        private readonly ICommonService _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
        private readonly IGenericServiceAsync<CarBrand, int> _carBrandService = carBrandService ?? throw new ArgumentNullException(nameof(carBrandService));
        private readonly IGenericServiceAsync<CarColor, int> _carColorService = carColorService ?? throw new ArgumentNullException(nameof(carColorService));
        private readonly ICarService _carService = carService ?? throw new ArgumentNullException(nameof(carService));
        private const int DefaulPageSize = 7;
        private readonly List<string> defaultValue = ["Выбрать..."];

        private readonly List<string> transmissions =
        [
            "Механическая (МКПП)",
            "Автоматическая (АКПП)",
            "Роботизированная (РКПП)",
            "Вариаторная КПП (Вариатор)"
        ];

        public async Task<ActionResult> Search(string vin, string mark, string regNumber, int? page)
        {
            var cars = await _carService.GetAsync(vin, mark, regNumber, page ?? 1, DefaulPageSize);
            if (cars.TotalItems == 0)
            {
                ModelState.AddModelError(string.Empty, "Нет данных по данному запросу....");
                return View(null);
            }

            return View(cars);
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.emptylist = EmptyList();
            var carEdit = new CarEditViewModel
            {
                CarBrands = await CarBrandsAsync(),
                BodyColorList = await CarColorAsync()
            };

            ViewBag.Transmiss = Transmissions();

            return View(carEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CarEditViewModel car)
        {
            ViewBag.emptylist = EmptyList();
            var isCarExist = await _carService.IsVinExistAsync(car.VinNumber);
            if (ModelState.IsValid && isCarExist == false)
            {
                var carModel = MapCarModelForSave(car);

                try
                {
                    await _carService.CreateAsync(carModel);
                    return RedirectToAction("Detail", "Cars", new { vin = car.VinNumber });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Ошибка сохранения данных в базе данных: " + ex.Message);
                }
            }

            ViewBag.Transmiss = Transmissions();
            car.BodyColorList = await CarColorAsync();
            car.CarBrands = await CarBrandsAsync();

            return View(car);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string vin)
        {
            var car = new CarEditViewModel();
            if (await MapCarModelAsync(car, vin) > 0)
            {
                ViewBag.Transmiss = Transmissions();

                return PartialView(car);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CarEditViewModel car)
        {
            if (ModelState.IsValid)
            {
                var carModel = MapCarModelForSave(car);
                try
                {
                    await _carService.UpdateAsync(carModel);
                    return null;
                }
                catch
                {
                    return null;
                }
            }

            ViewBag.Transmiss = Transmissions();
            ViewBag.emptylist = EmptyList();
            car.BodyColorList = await CarColorAsync();
            car.CarBrands = await CarBrandsAsync();

            return PartialView(car);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string vin)
        {
            var carModel = await _carService.GetCarViewModelAsync(vin);
            if (carModel != null)
            {
                return View(carModel);
            }

            return NotFound("В БД нет Автомобиля с VIN номером: " + vin);
        }

        public async Task<ActionResult> Delete(string vin)
        {
            var car = await _carService.GetByVinAsync(vin);

            if (car != null)
            {
                if (car.OrderServices.Count == 0)
                {
                    try
                    {
                        await _carService.DeleteAsync(car);

                        return View("Search");
                    }
                    catch (Exception ex)
                    {
                        // Обработка ошибки сохранения в базе данных
                        ModelState.AddModelError(string.Empty, "Ошибка сохранения данных в базе данных: " + ex.Message);
                    }
                }

                return null;
            }

            return NotFound("В БД нет Автомобиля с VIN номером: " + vin);
        }

        public async Task<ActionResult> AutoCompleteSearch(string term)
        {
            var vinNumbers = await _carService.AutoCompleteSearchAsync(term, DefaulPageSize);

            return Json(vinNumbers);
        }

        [HttpPost]
        public async Task<ActionResult> GetSelectList(int? selectedValue, string nameSelectList)
        {
            if (selectedValue.HasValue && nameSelectList != null)
                switch (nameSelectList)
                {
                    case "BrandId":
                        {
                            var carModelList = await _carService.CarModelByBrandIdAsync(selectedValue);

                            return PartialView("PartialModelView", carModelList);
                        }
                    case "ModelId":
                        {
                            var carModelList = await _carService.CarSeriesByModelIdAsync(selectedValue);

                            return PartialView("PartialModelView", carModelList);
                        }
                    case "SeriesId":
                        {
                            var carModelList = await _carService.CarModelBySeriesIdAsync(selectedValue);

                            return PartialView("PartialModelView", carModelList);
                        }
                    case "ModificationId":
                        {
                            var carModified = await _carService.CarModificationAsync(selectedValue);
                            if (carModified != null)
                            {
                                var carModelList = Years(carModified.StartProductionYear,
                                    carModified.EndProductionYear);

                                return PartialView("PartialModelView", carModelList);
                            }

                            return null;
                        }

                    default: return null;
                }

            return null;
        }

        private SelectList Years(int? modificationYearStart, int? modificationYearEnd)
        {
            var years = _commonService.Years(1920);
            var result = new SelectList(
                years.Where(item =>
                    item.Key >= modificationYearStart &&
                    item.Key <= (modificationYearEnd ?? DateTime.Now.Year)), years);

            return result;
        }

        private async Task<int> MapCarModelAsync(CarEditViewModel carModel, string vin)
        {
            var car = await _carService.GetByVinAsync(vin);
            if (car != null)
            {
                carModel.VinNumber = car.VinNumber;
                carModel.DataSheetCar = car.DataSheetCar;
                carModel.RegistrationNumber = car.RegistrationNumber;
                carModel.OwnerName = car.OwnerName;
                carModel.BrandId = car.CarModification?.Series?.CarModel?.BrandId;
                carModel.CarBrands = await CarBrandsAsync();
                carModel.ModelId = car.CarModification?.Series?.ModelId;
                carModel.ModelCarList = await _carService.CarModelByBrandIdAsync(car.CarModification?.Series?.CarModel?.BrandId);
                carModel.SeriesId = car.CarModification?.SeriesId;
                carModel.SeriesCarList = await _carService.CarSeriesByModelIdAsync(car.CarModification?.Series?.ModelId);
                carModel.ModificationId = car?.ModificationId;
                carModel.CarModifications = await _carService.CarModelBySeriesIdAsync(car.CarModification?.SeriesId);
                carModel.ColorId = car?.ColorId;
                carModel.BodyColorList = await CarColorAsync();
                carModel.YearList = Years(car.CarModification?.StartProductionYear, car.CarModification?.EndProductionYear);

                return 1;
            }

            return -1;
        }

        private static Car MapCarModelForSave(CarEditViewModel car)
        {
            var carModel = new Car
            {
                VinNumber = car.VinNumber,
                ColorId = car.ColorId,
                ModificationId = car.ModificationId,
                OwnerName = car.OwnerName,
                RegistrationNumber = car.RegistrationNumber,
                DataSheetCar = car.DataSheetCar,
                TransmissionType = car.TransmissionType,
                IssueYear = car.IssueYear
            };

            return carModel;
        }

        public SelectList EmptyList()
        {
            return new SelectList(defaultValue, defaultValue);
        }

        private SelectList Transmissions()
        {
            return new SelectList(transmissions, transmissions);
        }

        private async Task<SelectList> CarBrandsAsync()
        {
            return await _carBrandService.GetSelectListAsync();
        }

        private async Task<SelectList> CarColorAsync()
        {
            return await _carColorService.GetSelectListAsync();
        }
    }
}