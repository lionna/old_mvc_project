using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Abstract;
using CarServiceApp.Domain.Extension;
using CarServiceApp.Domain.Repository.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarServiceApp.Domain.Repository
{
    /// <summary>
    /// Репозиторий общего назначения для работы с сущностями в асинхронном режиме.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <typeparam name="TKey">Тип идентификатора сущности.</typeparam>
    /// <remarks>
    /// Создает новый экземпляр репозитория.
    /// </remarks>
    /// <param name="context">Контекст базы данных.</param>
    public class GenericRepositoryAsync<T, TKey>(ApplicationDbContext context)
        : IGenericRepositoryAsync<T, TKey>
        where T : class, ICommonEntity<TKey>
        where TKey : IComparable<TKey>
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        /// <summary>
        /// Получает все сущности асинхронно.
        /// </summary>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Получает словарь всех сущностей асинхронно.
        /// </summary>
        public async Task<Dictionary<object, string>> GetDictionaryAsync()
        {
            var entities = await _context.Set<T>().ToListAsync();
            return entities.ToDictionary(entity => (object)entity.Id, entity => entity.Name);
        }

        /// <summary>
        /// Получает список выбора всех сущностей асинхронно.
        /// </summary>
        public async Task<SelectList> GetSelectListAsync()
        {
            var entities = await GetDictionaryAsync();
            return entities.ToSelectList("Value", "Text");
        }

        /// <summary>
        /// Получает все сущности с учетом параметров пагинации, фильтрации и сортировки асинхронно.
        /// </summary>
        public async Task<GridItem> GetAllAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var offset = (pageNumber - 1) * pageSize;
            var data = await query.Skip(offset).Take(pageSize).ToListAsync();
            var totalItems = await query.CountAsync();

            return new GridItem
            {
                Data = data,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                ItemsPerPage = pageSize
            };
        }

        /// <summary>
        /// Получает сущность по идентификатору асинхронно.
        /// </summary>
        public async Task<T> GetByIdAsync(TKey id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Создает новую сущность асинхронно.
        /// </summary>
        public async Task CreateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновляет сущность асинхронно.
        /// </summary>
        public async Task UpdateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляет сущность по идентификатору асинхронно.
        /// </summary>
        public async Task DeleteAsync(TKey id)
        {
            var entityToDelete = await _context.Set<T>().FindAsync(id)
                ?? throw new InvalidOperationException($"Entity with id {id} not found.");
            _context.Set<T>().Remove(entityToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
