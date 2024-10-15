using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Service.Abstract;

namespace CarServiceApp.Domain.Service
{
    public class DiagramService(ApplicationDbContext context) : IDiagramService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly int currentYear = DateTime.Now.Year;
        private readonly int currentMonth = DateTime.Now.Month;

        public DiagramModel[] GetItems(int? year, int? month, Dictionary<int, string> months)
        {
            // Переменные года и месяца принимают значение текущего года и текущего месяца, если не указаны иные значения
            year ??= currentYear;
            month ??= currentMonth;

            if (month == 0)
            {
                // Получаем данные для диаграммы за весь год
                var diagramData = _context.OrderService
                    .Where(order => order.DateMakingOrder.Value.Year == year)
                    .GroupBy(order => order.DateMakingOrder.Value.Month)
                    .Select(group => new DiagramModel { y = group.Count(), x = group.FirstOrDefault().DateMakingOrder.Value.Month })
                    .OrderBy(model => model.x)
                    .ToList();

                // Преобразуем данные в формат, подходящий для диаграммы
                var diagramDataFormatted = diagramData.Select(item => new DiagramModel { y = item.y, nameMonth = months[item.x] }).ToArray();

                return diagramDataFormatted; // Возвращаем данные для диаграммы
            }
            else
            {
                // Получаем данные для диаграммы за указанный год и месяц
                var diagramData = _context.OrderService
                    .Where(order => order.DateMakingOrder.Value.Year == year && order.DateMakingOrder.Value.Month == month)
                    .GroupBy(order => order.DateMakingOrder.Value.Day)
                    .Select(group => new DiagramModel { y = group.Count(), x = group.FirstOrDefault().DateMakingOrder.Value.Day })
                    .OrderBy(model => model.x)
                    .ToArray();

                return diagramData; // Возвращаем данные для диаграммы
            }
        }

        public object[] GetIncomeItems(int? year, int? month, Dictionary<int, string> months)
        {
            // Если год или месяц не указаны, принимаем текущий год и текущий месяц
            year ??= DateTime.Now.Year;
            month ??= DateTime.Now.Month;

            if (month == null || month == 0)
            {
                // Получаем данные по доходам за весь год

                // Для услуг
                var servicesForYear = _context.ExecutingService
                    .Where(service => service.OrderService.DateMakingOrder.Value.Year == year && service.ServiceId != "mngopen" && service.ServiceId != "mngclose")
                    .GroupBy(group => group.OrderService.DateMakingOrder.Value.Month)
                    .Select(item => new { y = item.Sum(s => (int)((double)s.Price * (s.TaxAddedValue * 0.01 + 1))), x = item.FirstOrDefault().OrderService.DateMakingOrder.Value.Month })
                    .OrderBy(order => order.x)
                    .ToList();

                // Для запчастей
                var partsForYear = _context.UsingPartMaterial
                    .Where(part => part.ExecutingService.OrderService.DateMakingOrder.Value.Year == year)
                    .GroupBy(group => group.ExecutingService.OrderService.DateMakingOrder.Value.Month)
                    .Select(item => new { y = item.Sum(s => (int)s.CostPart), x = item.FirstOrDefault().ExecutingService.OrderService.DateMakingOrder.Value.Month })
                    .OrderBy(order => order.x)
                    .ToList();

                // Преобразуем данные в формат для передачи

                // Для услуг
                var formattedServicesForYear = servicesForYear.Select(h => new { h.y, nameMonth = months[h.x] }).ToArray();

                // Формируем массив с данными для возврата
                var result = new object[] { formattedServicesForYear, partsForYear };

                return result; // Возвращаем массив с данными
            }
            else
            {
                // Получаем данные по доходам за указанный год и месяц

                // Для услуг
                var servicesForMonth = _context.ExecutingService
                    .Where(service => service.OrderService.DateMakingOrder.Value.Year == year && service.OrderService.DateMakingOrder.Value.Month == month && service.ServiceId != "mngopen" && service.ServiceId != "mngclose")
                    .GroupBy(group => group.OrderService.DateMakingOrder.Value.Day)
                    .Select(item => new { y = item.Sum(s => (int)((double)s.Price * (s.TaxAddedValue * 0.01 + 1))), x = item.FirstOrDefault().OrderService.DateMakingOrder.Value.Day })
                    .OrderBy(order => order.x)
                    .ToList();

                // Для запчастей
                var partsForMonth = _context.UsingPartMaterial
                    .Where(part => part.ExecutingService.OrderService.DateMakingOrder.Value.Year == year && part.ExecutingService.OrderService.DateMakingOrder.Value.Month == month)
                    .GroupBy(group => group.ExecutingService.OrderService.DateMakingOrder.Value.Day)
                    .Select(item => new { y = item.Sum(s => (int)s.CostPart), x = item.FirstOrDefault().ExecutingService.OrderService.DateMakingOrder.Value.Day })
                    .OrderBy(order => order.x)
                    .ToList();

                // Формируем массив с данными для возврата
                var result = new object[] { servicesForMonth, partsForMonth };

                return result; // Возвращаем массив с данными
            }
        }
    }
}