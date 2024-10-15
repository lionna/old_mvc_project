using CarServiceApp.Domain.Entity.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers.Common
{
    public class CommonEntityController<T, TKey>
        (IGenericServiceAsync<T, TKey> service) : BaseController<T, TKey>(service)
        where T : class, ICommonEntity<TKey>
        where TKey : IComparable<TKey>
    {
        [HttpGet]
        public override async Task<IActionResult> Create()
        {
            return await Task.FromResult(View());
        }

        [HttpPost]
        public override async Task<IActionResult> Create(T entity)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(entity);
                return RedirectToAction("Index");
            }

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

            return View(entity);
        }

        [HttpGet]
        public override async Task<IActionResult> Detail(TKey id)
        {
            var entity = await _service.GetByIdAsync(id);

            return View(entity);
        }

        [HttpDelete]
        public override async Task<IActionResult> Delete(TKey id)
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

//using CarServiceApp.Domain.Entity.Abstract;
//using CarServiceApp.Domain.Repository.Abstract;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq.Expressions;

//namespace CarServiceApp.Controllers.Common
//{
//    public abstract class CommonEntityController<T, TKey>(IGenericRepositoryAsync<T, TKey> repository)
//        : Controller
//        where T : class, ICommonEntity<TKey>
//        where TKey : IComparable<TKey>
//    {
//        private readonly IGenericRepositoryAsync<T, TKey> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

//        protected virtual Expression<Func<T, bool>> GetFilters(string searchString)
//        {
//            return r => string.IsNullOrEmpty(searchString) || r.Name.Contains(searchString);
//        }

//        protected virtual Func<IQueryable<T>, IOrderedQueryable<T>> GetOrders(string sortOrder)
//        {
//            return sortOrder switch
//            {
//                "NameDesc" => q => q.OrderByDescending(r => r.Name),
//                _ => q => q.OrderBy(r => r.Id),
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
//            // Получение данных с использованием общего метода репозитория.
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
//        public virtual IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        public virtual async Task<IActionResult> Create(T entity)
//        {
//            if (ModelState.IsValid)
//            {
//                await _repository.CreateAsync(entity);
//                return RedirectToAction("Index");
//            }

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

//            return View(entity);
//        }

//        [HttpGet]
//        public virtual async Task<IActionResult> Detail(TKey id)
//        {
//            var entity = await _repository.GetByIdAsync(id);

//            return View(entity);
//        }

//        [HttpDelete]
//        public virtual async Task<IActionResult> Delete(TKey id)
//        {
//            try
//            {
//                await _repository.DeleteAsync(id);
//            }
//            catch
//            {
//                ModelState.AddModelError(string.Empty, "Проблема удаления.");
//                return Json(new { isNotSuccess = true, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
//            }

//            return RedirectToAction(nameof(Index));
//        }
//    }
//}