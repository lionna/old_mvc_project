using CarServiceApp.Domain.Entity.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace CarServiceApp.Controllers.Common
{

    /// <summary>
    /// Базовый контроллер для управления сущностями.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <typeparam name="TKey">Тип идентификатора сущности.</typeparam>
    /// <remarks>
    /// Конструктор базового контроллера.
    /// </remarks>
    /// <param name="service">Репозиторий для работы с сущностями.</param>
    public abstract class BaseController<T, TKey> // Типы T и TKey определяют тип сущности и тип ее идентификатора соответственно.
        (IGenericServiceAsync<T, TKey> service) : Controller// repository: Репозиторий для работы с сущностями
        where T : class, ICommonEntity<TKey>// T должен быть классом, реализующим интерфейс ICommonEntity<TKey>
        where TKey : IComparable<TKey>// TKey должен быть классом, реализующим интерфейс IComparable<TKey>
    {
        /// <summary>
        /// Репозиторий для работы с сущностями.
        /// </summary>
        protected readonly IGenericServiceAsync<T, TKey> _service = service ?? throw new ArgumentNullException(nameof(service));

        /// <summary>
        /// Возвращает выражение для фильтрации сущностей по поисковому запросу.
        /// </summary>
        protected virtual Expression<Func<T, bool>> GetFilters(string searchString)
        {
            return r => string.IsNullOrEmpty(searchString) || r.Name.Contains(searchString);
        }

        /// <summary>
        /// Возвращает функцию для упорядочивания списка сущностей.
        /// </summary>
        protected virtual Func<IQueryable<T>, IOrderedQueryable<T>> GetOrders(string sortOrder)
        {
            return sortOrder switch
            {
                "NameDesc" => q => q.OrderByDescending(r => r.Name),
                _ => q => q.OrderBy(r => r.Id),
            };
        }

        /// <summary>
        /// Применяет фильтры к запросу сущностей.
        /// </summary>
        protected virtual IQueryable<T> ApplyFilters(IQueryable<T> query, string searchString)
        {
            var filters = GetFilters(searchString);
            if (filters != null)
                query = query.Where(filters);
            return query;
        }

        /// <summary>
        /// Применяет порядок сортировки к запросу сущностей.
        /// </summary>
        protected virtual IQueryable<T> ApplyOrders(IQueryable<T> query, string sortOrder)
        {
            var orders = GetOrders(sortOrder);
            if (orders != null)
                query = orders(query);
            return query;
        }

        /// <summary>
        /// Возвращает массив выражений для подключения связанных данных.
        /// </summary>
        protected virtual Expression<Func<T, object>>[] GetIncludeInfo()
        {
            return null;
        }

        /// <summary>
        /// Отображает список сущностей.
        /// </summary>
        public virtual async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 5,
            string sortOrder = "",
            string searchString = "")
        {
            // Получение данных с использованием общего метода репозитория.
            var model = await _service.GetAllAsync(
                pageNumber: page,
                pageSize: pageSize,
                filter: GetFilters(searchString),
                orderBy: GetOrders(sortOrder),
                includes: GetIncludeInfo()
            );

            // Передача данных в представление.
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
            ViewData["CurrentFilter"] = searchString;

            return View(model);
        }

        /// <summary>
        /// Отображает список сущностей.
        /// </summary>
        public virtual async Task<IActionResult> Search(
            int page = 1,
            int pageSize = 5,
            string sortOrder = "",
            string searchString = "")
        {
            // Получение данных с использованием общего метода репозитория.
            var model = await _service.GetAllAsync(
                pageNumber: page,
                pageSize: pageSize,
                filter: GetFilters(searchString),
                orderBy: GetOrders(sortOrder),
                includes: GetIncludeInfo()
            );

            // Передача данных в представление.
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
            ViewData["CurrentFilter"] = searchString;

            return View(model);
        }

        /// <summary>
        /// Отображает форму создания новой сущности.
        /// </summary>
        [HttpGet]
        public virtual async Task<IActionResult> Create()
        {
            return await Task.FromResult(View());
        }

        /// <summary>
        /// Обрабатывает данные формы создания новой сущности.
        /// </summary>
        [HttpPost]
        public virtual async Task<IActionResult> Create(T entity)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(entity);
                return RedirectToAction("Index");
            }

            return View(entity);
        }

        /// <summary>
        /// Отображает форму редактирования сущности.
        /// </summary>
        [HttpGet]
        public virtual async Task<IActionResult> Edit(TKey id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Обрабатывает данные формы редактирования сущности.
        /// </summary>
        [HttpPost]
        public virtual async Task<IActionResult> Edit(TKey id, T entity)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(entity);
                return RedirectToAction("Index");
            }

            return View(entity);
        }

        /// <summary>
        /// Отображает детали сущности.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        [HttpGet]
        public virtual async Task<IActionResult> Detail(TKey id)
        {
            var entity = await _service.GetByIdAsync(id);

            return View(entity);
        }

        /// <summary>
        /// Удаляет сущность.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой сущности.</param>
        /// <returns>Результат действия.</returns>
        [HttpDelete]
        public virtual async Task<IActionResult> Delete(TKey id)
        {
            try
            {
                await _service.DeleteAsync(id);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Проблема удаления.");
                return Json(new { isNotSuccess = true, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            return RedirectToAction(nameof(Index));
        }


    }
}