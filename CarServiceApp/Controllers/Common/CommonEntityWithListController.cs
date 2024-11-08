using CarServiceApp.Domain.Entity.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers.Common
{
    public abstract class CommonEntityWithListController<T, TKey>(IGenericServiceAsync<T, TKey> service) : BaseController<T, TKey>(service)
        where T : class, ICommonEntityWithDictinary<TKey>
        where TKey : IComparable<TKey>
    {
        protected abstract Task<Dictionary<object, string>> GetDictionaryAsync();

        [HttpGet]
        public override async Task<IActionResult> Create()
        {
            T entity = Activator.CreateInstance<T>();
            entity.DropdownList = await GetDictionaryAsync();

            return View(entity);
        }

        [HttpGet]
        public override async Task<IActionResult> Detail(TKey id)
        {
            var entity = await _service.GetByIdAsync(id);

            return View(entity);
        }

        [HttpPost]
        public override async Task<IActionResult> Create(T entity)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(entity);
                return RedirectToAction("Index");
            }

            entity.DropdownList = await GetDictionaryAsync();
            return View(entity);
        }

        [HttpGet]
        public override async Task<IActionResult> Edit(TKey id)
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

            entity.DropdownList = await GetDictionaryAsync();

            return View(entity);
        }

        [HttpPost]
        public override async Task<IActionResult> Edit(TKey id, T entity)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(entity);
                return RedirectToAction("Index");
            }

            entity.DropdownList = await GetDictionaryAsync();
            return View(entity);
        }

        [HttpDelete]
        public override async Task<IActionResult> Delete(TKey id)
        {
            try
            {
                await _service.DeleteAsync(id);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Проблема удаления.");
                return Json(new { isNotSuccess = true, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

//using CarServiceApp.Domain.Entity.Abstract;
//using CarServiceApp.Domain.Repository.Abstract;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq.Expressions;

//namespace CarServiceApp.Controllers.Common
//{
//    public abstract class CommonEntityWithListController<T, TKey>(IGenericRepositoryAsync<T, TKey> repository)
//        : Controller
//        where T : class, ICommonEntityWithDictinary<TKey>
//        where TKey : IComparable<TKey>
//    {
//        private readonly IGenericRepositoryAsync<T, TKey> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

//        protected abstract Task<Dictionary<object, string>> GetDictionaryAsync();

//        protected virtual Expression<Func<T, bool>> GetFilters(string searchString)
//        {
//            return x => string.IsNullOrEmpty(searchString) || x.Name.Contains(searchString);
//        }

//        protected virtual Func<IQueryable<T>, IOrderedQueryable<T>> GetOrders(string sortOrder)
//        {
//            return sortOrder switch
//            {
//                "NameDesc" => x => x.OrderByDescending(s => s.Name),
//                _ => x => x.OrderBy(x => x.Id),
//            };
//        }

//        protected virtual Expression<Func<T, object>>[] GetIncludeInfo()
//        {
//            return null;
//        }

//        public virtual async Task<IActionResult> Index(
//            int page = 1,
//            int pageSize = 5,
//            string sortOrder = "",
//            string searchString = "")
//        {
//            var model = await _repository.GetAllAsync(
//                pageNumber: page,
//                pageSize: pageSize,
//                filter: GetFilters(searchString),
//                orderBy: GetOrders(sortOrder),
//                includes: GetIncludeInfo()
//            );

//            // Передача данных в представление.
//            ViewData["CurrentSort"] = sortOrder;
//            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
//            ViewData["CurrentFilter"] = searchString;

//            return View(model);
//        }

//        [HttpGet]
//        public virtual async Task<IActionResult> Create()
//        {
//            T entity = Activator.CreateInstance<T>();
//            entity.DropdownList = await GetDictionaryAsync();

//            return View(entity);
//        }

//        [HttpGet]
//        public virtual async Task<IActionResult> Detail(TKey id)
//        {
//            var entity = await _repository.GetByIdAsync(id);

//            return View(entity);
//        }

//        [HttpPost]
//        public virtual async Task<IActionResult> Create(T entity)
//        {
//            if (ModelState.IsValid)
//            {
//                await _repository.CreateAsync(entity);
//                return RedirectToAction("Index");
//            }

//            entity.DropdownList = await GetDictionaryAsync();
//            return View(entity);
//        }

//        [HttpGet]
//        public virtual async Task<IActionResult> Edit(TKey id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var entity = await _repository.GetByIdAsync(id);
//            if (entity == null)
//            {
//                return NotFound();
//            }

//            entity.DropdownList = await GetDictionaryAsync();

//            return View(entity);
//        }

//        [HttpPost]
//        public virtual async Task<IActionResult> Edit(TKey id, T entity)
//        {
//            //if (id != entity.Id)
//            //{
//            //    return NotFound();
//            //}

//            if (ModelState.IsValid)
//            {
//                await _repository.UpdateAsync(entity);
//                return RedirectToAction("Index");
//            }

//            entity.DropdownList = await GetDictionaryAsync();
//            return View(entity);
//        }

//        [HttpDelete]
//        public virtual async Task<IActionResult> Delete(TKey id)
//        {
//            try
//            {
//                await _repository.DeleteAsync(id);
//            }
//            catch (Exception ex)
//            {
//                ModelState.AddModelError(string.Empty, "Проблема удаления.");
//                return Json(new { isNotSuccess = true, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
//            }

//            return RedirectToAction(nameof(Index));
//        }
//    }
//}