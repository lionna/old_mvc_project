using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IContractService
    {
        Task<int> CreateNewContractAsync(
            int personnelNumber,
            DateOnly? recruitDate,
            string type,
            int term);

        Task<ContractToEmployee> GetByIdAsync(int personnelNumber);

        Task<ContractToEmployee> GetAsync(int id);

        Task CreateAsync(ContractToEmployee entity);

        Task UpdateAsync(ContractToEmployee entity);

        Task DeleteAsync(int id);

        Task<GridItem> GeAsync(
            int? contractId,
            string name,
            string type,
            int? term,
            bool isOn,
            DateOnly? dateFrom,
            DateOnly? dateTo,
            int page,
            int pageSize);

        Task<int> UnLockContractAsync(int contractId);
    }
}