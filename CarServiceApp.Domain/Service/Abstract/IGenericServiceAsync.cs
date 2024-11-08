using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IGenericServiceAsync<T, TKey>
        where T : class, ICommonEntity<TKey>
        where TKey : IComparable<TKey>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<Dictionary<object, string>> GetDictionaryAsync();
        Task<GridItem> GetAllAsync(int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(TKey id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TKey id);
        Task<SelectList> GetSelectListAsync();
    }
}
