using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CarServiceApp.Domain.Service
{
    public class ContractService(ApplicationDbContext context) : IContractService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<GridItem> GeAsync(
            int? contractId,
            string name,
            string type,
            int? term,
            bool isOn,
            DateOnly? dateFrom,
            DateOnly? dateTo,
            int page,
            int pageSize)
        {
            var contracts = await _context.Employee
         .Join(
             _context.ContractToEmployee
                 .Where(c =>
                     (contractId == null || c.ContractId == contractId) &&
                     _context.Employee.Where(e => name == null || e.FullName.Contains(name)).Select(e => e.PersonnelNumber).Contains(c.PersonnelNumber) &&
                     (type == null || c.Type.Contains(type)) &&
                     //((isOn && c.DismissDate == null) || (!isOn && c.DismissDate != null)) &&
                     (dateFrom == null || c.RecruitDate >= dateFrom) &&
                     (dateTo == null || c.RecruitDate <= dateTo) &&
                     (term == null || c.Term == term)
                 )
                 .Select(c => new { c.ContractId, c.RecruitDate, c.DismissDate, c.Type, c.Term, c.PersonnelNumber }),
             e => e.PersonnelNumber,
             c => c.PersonnelNumber,
             (e, c) => new { 
                 c.ContractId, c.RecruitDate, c.DismissDate, c.Type, 
                 e.FullName, c.Term, c.PersonnelNumber 
             }
         )
         .Select(c => new ContractModel
         {
             ContractId = c.ContractId,
             RecruitDate = c.RecruitDate,
             DismissDate = c.DismissDate,
             Type = c.Type,
             FullName = c.FullName,
             Term = c.Term,
             PersonnelNumber = c.PersonnelNumber
         })
         .ToListAsync();


            var offset = (page - 1) * pageSize;
            var data = contracts.Skip(offset).Take(pageSize);

            return new GridItem
            {
                Data = data,
                TotalItems = contracts.Count(),
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
        }

        public async Task<int> UnLockContractAsync(int contractId)
        {
            var contract = await _context.ContractToEmployee
                .Where(c => c.ContractId == contractId && c.DismissDate == null)
                .FirstOrDefaultAsync();

            if (contract != null)
            {
                contract.DismissDate = DateOnly.FromDateTime(DateTime.UtcNow);

                await _context.SaveChangesAsync();

                var personnelNumber = contract.PersonnelNumber;

                var departmentsToUpdate = await _context.Department
                    .Where(d => d.ChiefPersonalNumber == personnelNumber)
                    .ToListAsync();

                foreach (var department in departmentsToUpdate)
                {
                    department.ChiefPersonalNumber = null;
                }

                await _context.SaveChangesAsync();

                return 1;
            }

            return 0;
        }

        public async Task DeleteAsync(int id)
        {
            var entityToDelete = await _context.Set<ContractToEmployee>().FindAsync(id)
               ?? throw new InvalidOperationException($"Entity with id {id} not found.");
            _context.Set<ContractToEmployee>().Remove(entityToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(ContractToEmployee entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _context.Set<ContractToEmployee>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ContractToEmployee entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Set<ContractToEmployee>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ContractToEmployee> GetByIdAsync(int personnelNumber)
        {
            return await _context.Set<ContractToEmployee>()
                 .FirstOrDefaultAsync(e => e.PersonnelNumber == personnelNumber && e.DismissDate == null);
        }

        private static Expression<Func<ContractToEmployee, object>>[] GetInfo()
        {
            return
            [
                x => x.Employee,
            ];
        }

        public async Task<ContractToEmployee> GetAsync(int id)
        {
            //return await _context.Set<ContractToEmployee>()
            //     .FirstOrDefaultAsync(e => e.ContractId == id);

            var query = _context.Set<ContractToEmployee>().AsQueryable();

            var includes = GetInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(e => e.ContractId == id);
        }

        public async Task<int> CreateNewContractAsync(int personnelNumber, DateOnly? recruitDate, string type, int term)
        {
            var employeeExists = await _context.Employee.AnyAsync(e => e.PersonnelNumber == personnelNumber);
            var activeContractExists = await _context.ContractToEmployee.AnyAsync(c => c.PersonnelNumber == personnelNumber && c.DismissDate == null);

            if (employeeExists && !activeContractExists && term >= 1 && term <= 5)
            {
                recruitDate ??= DateOnly.FromDateTime(DateTime.UtcNow);

                if (recruitDate > DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    recruitDate = DateOnly.FromDateTime(DateTime.UtcNow);
                }

                var newContract = new ContractToEmployee
                {
                    PersonnelNumber = personnelNumber,
                    Type = type,
                    Term = term,
                    RecruitDate = recruitDate.Value
                };

                _context.ContractToEmployee.Add(newContract);
                await _context.SaveChangesAsync();

                return 1; // Успешное создание контракта
            }
            else
            {
                return 0; // Ошибка при создании контракта
            }
        }
    }
}