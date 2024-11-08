using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;

namespace CarServiceApp.Domain.Service
{
    public class CarService : ICarService
    {
        public readonly ICarRepository _carRepository;
        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
        }

        public async Task<IEnumerable<string>> AutoCompleteSearchAsync(string term, int take)
        {
            return await _carRepository.AutoCompleteSearchAsync(term, take);
        }

        public async Task<SelectList> CarModelByBrandIdAsync(int? selectedValue)
        {
            return await _carRepository.CarModelByBrandIdAsync(selectedValue);
        }

        public async Task<SelectList> CarModelBySeriesIdAsync(int? selectedValue)
        {
            return await _carRepository.CarModelBySeriesIdAsync(selectedValue);
        }

        public async Task<CarModification> CarModificationAsync(int? selectedValue)
        {
            return await _carRepository.CarModificationAsync(selectedValue);
        }

        public async Task<SelectList> CarSeriesByModelIdAsync(int? selectedValue)
        {
            return await _carRepository.CarSeriesByModelIdAsync(selectedValue);
        }

        public async Task CreateAsync(Car car)
        {
            await _carRepository.CreateAsync(car);
        }

        public async Task DeleteAsync(Car car)
        {
            await _carRepository.DeleteAsync(car);
        }

        public async Task<GridItem> GetAsync(string vin, string mark, string regNumber, int page, int pageSize)
        {
            return await _carRepository.GetAsync(vin, mark, regNumber, page, pageSize);
        }

        public async Task<Car> GetByVinAsync(string vin)
        {
            return await _carRepository.GetByVinAsync(vin);
        }

        public async Task<CarViewModel> GetCarViewModelAsync(string vin)
        {
            return await _carRepository.GetCarViewModelAsync(vin);
        }

        public async Task<bool> IsVinExistAsync(string vin)
        {
            return await _carRepository.IsVinExistAsync(vin);
        }

        public async Task UpdateAsync(Car car)
        {
            await _carRepository.UpdateAsync(car);
        }
    }
}
