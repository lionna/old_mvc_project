using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface ICarService
    {
        Task<IEnumerable<string>> AutoCompleteSearchAsync(string term, int take);
        Task<Car> GetByVinAsync(string vin);
        Task<bool> IsVinExistAsync(string vin);
        Task CreateAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(Car car);
        Task<CarViewModel> GetCarViewModelAsync(string vin);
        Task<SelectList> CarModelByBrandIdAsync(int? selectedValue);
        Task<SelectList> CarSeriesByModelIdAsync(int? selectedValue);
        Task<SelectList> CarModelBySeriesIdAsync(int? selectedValue);
        Task<CarModification> CarModificationAsync(int? selectedValue);
        Task<GridItem> GetAsync(string vin, string mark, string regNumber, int page, int pageSize);
    }
}
