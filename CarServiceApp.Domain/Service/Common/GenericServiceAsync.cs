using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity.Abstract;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace CarServiceApp.Domain.Service.Common
{
    public class GenericServiceAsync<T, TKey>(IGenericRepositoryAsync<T, TKey> repository) : IGenericServiceAsync<T, TKey>
        where T : class, ICommonEntity<TKey>
        where TKey : IComparable<TKey>
    {
        private readonly IGenericRepositoryAsync<T, TKey> _repository = repository;

        public async Task CreateAsync(T entity)
        {
            await _repository.CreateAsync(entity);
        }

        public async Task DeleteAsync(TKey id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<GridItem> GetAllAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize, filter, orderBy, includes);
        }

        public async Task<T> GetByIdAsync(TKey id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Dictionary<object, string>> GetDictionaryAsync()
        {
            return await _repository.GetDictionaryAsync();
        }

        public async Task<SelectList> GetSelectListAsync()
        {
            return await _repository.GetSelectListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
        }
    }
}
