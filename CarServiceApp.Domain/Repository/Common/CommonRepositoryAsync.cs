using System.Linq.Expressions;
using CarServiceApp.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApp.Domain.Repository;

public abstract class CommonRepositoryAsync
{
    public static async Task<PagingOutput<T>> GetAllWithPageAsync<T>(
        IQueryable<T> query,
        int pageNumber,
        int pageSize,
        params Expression<Func<T, object>>[] includes) where T : class
    {
        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        var offset = (pageNumber - 1) * pageSize;
        var data = await query.Skip(offset).Take(pageSize).ToListAsync();
        var totalItems = await query.CountAsync();

        return new PagingOutput<T>
        {
            Data = data,
            TotalItems = totalItems,
            CurrentPage = pageNumber,
            ItemsPerPage = pageSize
        };
    }

    public static async Task<GridItem> GetAllWithPagesAsync<T>(
        IQueryable<T> query,
        int pageNumber,
        int pageSize,
        params Expression<Func<T, object>>[] includes) where T : class
    {
        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
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

    public static GridItem GetAllWithPages<T>(
    IEnumerable<T> query,
    int pageNumber,
    int pageSize,
    params Expression<Func<T, object>>[] includes) where T : class
    {
        // Преобразуем IEnumerable<T> в IQueryable<T>, чтобы использовать методы Include
        var queryable = query.AsQueryable();

        // Применяем включения, если они были указаны
        if (includes != null)
        {
            foreach (var include in includes)
            {
                queryable = queryable.Include(include);
            }
        }

        // Вычисляем смещение
        var offset = (pageNumber - 1) * pageSize;

        // Применяем смещение и взятие указанного количества записей
        var data = queryable.Skip(offset).Take(pageSize).ToList();

        // Получаем общее количество элементов
        var totalItems = queryable.Count();

        // Возвращаем результат
        return new GridItem
        {
            Data = data,
            TotalItems = totalItems,
            CurrentPage = pageNumber,
            ItemsPerPage = pageSize
        };
    }
}