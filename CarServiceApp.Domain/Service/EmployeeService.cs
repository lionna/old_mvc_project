using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Extension;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarServiceApp.Domain.Service
{
    public class EmployeeService(ApplicationDbContext context) : IEmployeeService
    {
        private const string DefaultFullName = "Выбрать...";

        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task DeleteAsync(Entity.Models.Employee entity)
        {
            _context.Set<Entity.Models.Employee>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        private static Expression<Func<Entity.Models.Employee, object>>[] GetInfo()
        {
            return
            [
                x => x.Department,
                x => x.Post,
                x => x.ContractToEmployees,
            ];
        }

        public async Task<GridItem> GetAsync(
            string id,
            string name,
            int page,
            int pageSize)
        {
            var query = _context.Set<Entity.Models.Employee>().AsQueryable();

            query = !string.IsNullOrWhiteSpace(name)
                 ? query.Where(s => s.FullName.Contains(name))
                 : query.Where(s => s.PersonnelNumber == int.Parse(id));

            var includes = GetInfo();

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

        public async Task<List<string>> GetAsync(string id)
        {
            return (await _context.Set<Entity.Models.Employee>().Where(s => s.PersonnelNumber == int.Parse(id)).ToListAsync())
                .Select(a => a.PersonnelNumber.ToString()).Distinct().ToList();
        }

        public async Task<List<string>> GetByNameAsync(string name)
        {
            return (await _context.Set<Entity.Models.Employee>().Where(s => s.FullName.Contains(name)).ToListAsync())
                .Select(a => a.FullName).Distinct().ToList();
        }

        public async Task<Dictionary<object, string>> GetDictionaryAsync()
        {
            var entities = await _context.Set<Employee>().ToListAsync();
            return entities.ToDictionary(entity => (object)entity.PersonnelNumber, entity => entity.FullName);
        }

        public async Task<SelectList> GetSelectListAsync()
        {
            var entities = await GetDictionaryAsync();
            return entities.ToSelectList("Value", "Text");
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var query = _context.Set<Entity.Models.Employee>().AsQueryable();

            query = query.Where(s => s.PersonnelNumber == id);

            var includes = GetInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Employee entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _context.Set<Employee>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Set<Employee>().Update(entity);
            await _context.SaveChangesAsync();
        }

        //mngparts
        public SelectList Employees(bool usePostFilter = true, int postId = 5, bool userDefaultValue = false)
        {
            if (userDefaultValue == false)
                return new SelectList(
                    Employees(usePostFilter, postId),
                    "PersonnelNumber",
                    "FullName");

            var employeeList = Employees(usePostFilter, postId);
            employeeList.Add(new Employee { PersonnelNumber = 0, FullName = DefaultFullName });
            return new SelectList(employeeList, "PersonnelNumber", "FullName", 0);
        }

        private List<Employee> Employees(bool usePostFilter = true, int postId = 4)// "mngparts"
        {
            var query = _context.Set<Entity.Models.Employee>().AsQueryable();
            return usePostFilter
                ? query.Where(f =>
                f.PostId == postId &&
                f.ContractToEmployees.FirstOrDefault(d => d.DismissDate == null) != null).ToList()
                    : query.Where(f =>
                    f.ContractToEmployees.FirstOrDefault(d => d.DismissDate == null) != null).ToList();
        }
    }
}