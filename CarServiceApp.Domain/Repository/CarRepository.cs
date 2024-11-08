using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Repository.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarServiceApp.Domain.Repository
{
    public class CarRepository(ApplicationDbContext context) : ICarRepository
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        
        public async Task<IEnumerable<string>> AutoCompleteSearchAsync(string term, int take)
        {
            var query = _context.Set<Car>()
                .Where(c => c.VinNumber.Contains(term))
                .Select(c => c.VinNumber)
                .Distinct()
                .Take(take);

            return await query.ToListAsync();
        }

        private static Expression<Func<Car, object>>[] GetCarIncludeInfo()
        {
            return
            [
                x => x.CarColor,
                x => x.CarModification,
                x => x.CarModification.Series,
                x => x.CarModification.Series.CarModel,
                x => x.CarModification.Series.CarModel.CarBrand,
            ];
        }

        public async Task<Car> GetByVinAsync(string vin)
        {
            var query = _context.Set<Car>().AsQueryable();

            if (!string.IsNullOrEmpty(vin))
            {
                query = query.Where(s => s.VinNumber.Contains(vin));
            }

            var includes = GetCarIncludeInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Car car)
        {
            ArgumentNullException.ThrowIfNull(car);

            await _context.Set<Car>().AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Car car)
        {
            ArgumentNullException.ThrowIfNull(car);

            _context.Set<Car>().Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Car car)
        {
            _context.Set<Car>().Remove(car);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsVinExistAsync(string vin)
        {
            Car query = await _context.Set<Car>()
                .FirstOrDefaultAsync(s => s.VinNumber.Contains(vin));

            return query != null;
        }

        public async Task<CarViewModel> GetCarViewModelAsync(string vin)
        {
            var car = await GetByVinAsync(vin);
            if (car != null)
            {                              
                var carModel = new CarViewModel
                {
                    VinNumber = car.VinNumber,
                    DataSheetCar = car.DataSheetCar,
                    IssueYear = car.IssueYear,
                    OwnerName = car.OwnerName,
                    TransmissionType = car.TransmissionType,
                    ColorCode = car.CarColor?.Hex,
                    PaintName = car.CarColor?.Name,
                    ColorName = car.CarColor?.Description,
                    Brand = car.CarModification?.Series?.CarModel?.CarBrand?.Name,
                    Model = car.CarModification?.Series?.CarModel?.Name,
                    Series = car.CarModification?.Series?.Name,
                    Modification = car.CarModification?.Name,
                    CountRepare = car.OrderServices?.Count,
                    ViewCar = car.CarModification?.Series?.CarModel?.CarBrand?.ViewBrand,
                    RegistrationNumber = car.RegistrationNumber
                };

                return carModel;
            }

            return null;
        }

        public async Task<SelectList> CarModelByBrandIdAsync(int? selectedValue)
        {
            var carModels = await _context.Set<CarModel>()
                                          .Where(model => model.BrandId == selectedValue)
                                          .ToListAsync();

            var items = carModels.Select(model => new SelectListItem
            {
                Value = model.Id.ToString(),
                Text = model.Name
            });

            return new SelectList(items, "Value", "Text");
        }

        public async Task<SelectList> CarSeriesByModelIdAsync(int? selectedValue)
        {
            var carSeries = await _context.Set<CarSeries>()
                                          .Where(model => model.ModelId == selectedValue)
                                          .ToListAsync();

            var items = carSeries.Select(model => new SelectListItem
            {
                Value = model.Id.ToString(),
                Text = model.Name
            });

            return new SelectList(items, "Value", "Text");
        }

        public async Task<SelectList> CarModelBySeriesIdAsync(int? selectedValue)
        {
            var carModification = await _context.Set<CarModification>()
                                          .Where(model => model.SeriesId == selectedValue)
                                          .ToListAsync();

            var items = carModification.Select(model => new SelectListItem
            {
                Value = model.Id.ToString(),
                Text = model.Name
            });

            return new SelectList(items, "Value", "Text");
        }

        public async Task<CarModification> CarModificationAsync(int? selectedValue)
        {
            return await _context.Set<CarModification>()
                .FirstOrDefaultAsync(model => model.Id == selectedValue);
        }

        public async Task<GridItem> GetAsync(string vin, string mark, string regNumber, int page, int pageSize)
        {
            var query = _context.Set<Car>().AsQueryable();

            if (!string.IsNullOrEmpty(vin))
            {
                query = query.Where(s => s.VinNumber.Contains(vin));
            }

            if (!string.IsNullOrEmpty(regNumber))
            {
                query = query.Where(s => s.RegistrationNumber.Contains(regNumber));
            }

            if (!string.IsNullOrEmpty(mark))
            {
                query = query.Where(s => s.CarModification.Series.Name.Contains(mark));
            }

            var includes = GetCarIncludeInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var offset = (page - 1) * pageSize;
            var data = await query.Skip(offset).Take(pageSize).ToListAsync();

            return new GridItem
            {
                Data = data,
                TotalItems = query.Count(),
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
        }
    }
}
