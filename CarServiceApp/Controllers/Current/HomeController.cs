using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}

//using CarServiceApp.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;

//namespace CarServiceApp.Controllers
//{
//    public class HomeController1 : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController1(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }

//        [HttpGet]
//        public JsonResult GetNames()
//        {
//            return new JsonResult(new[] {"Name11","Name22" });
//        }

//        [HttpPost]
//        public JsonResult PostName(string name)
//        {
//            return new JsonResult(Ok());
//        }

//        public IActionResult TestJson()
//        {
//            return View();
//        }

//        public IActionResult DateIndex()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult DateIndex(DateTime date1, DateTime date2)
//        {
//            // Обработка выбранных дат
//            // date1 и date2 содержат выбранные пользователем даты

//            return RedirectToAction("DateIndex"); // Возвращаем пользователя на страницу выбора дат
//        }
//    }
//}