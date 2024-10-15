using CarServiceApp.Domain.Service.Abstract;

namespace CarServiceApp.Domain.Service
{
    /// <summary>
    /// Класс, реализующий интерфейс ICommonService и предоставляющий общие методы.
    /// </summary>
    public class CommonService : ICommonService
    {
        private readonly int currentYear = DateTime.Now.Year; // Год текущей даты

        /// <summary>
        /// Метод, который возвращает словарь с годами на основе текущего года и предыдущих трех лет.
        /// </summary>
        /// <param name="selectYear">Выбранный год (необязательный параметр)</param>
        /// <returns>Словарь с годами</returns>
        public Dictionary<int, int> Years(int? selectYear = null)
        {
            var years = new Dictionary<int, int>(); // Создаем словарь для хранения годов

            // Проходимся по годам, начиная с выбранного года или текущего года и возвращаем три предыдущих года
            for (var year = currentYear; year >= (selectYear ?? (currentYear - 3)); year--)
            {
                years.Add(year, year); // Добавляем год в словарь
            }

            return years; // Возвращаем словарь с годами
        }

        /// <summary>
        /// Метод, который возвращает словарь с названиями месяцев.
        /// </summary>
        /// <param name="addDafaultValue">Флаг, указывающий, нужно ли добавить дополнительное значение "За весь год" (по умолчанию true)</param>
        /// <returns>Словарь с названиями месяцев</returns>
        public Dictionary<int, string> Months(bool addDafaultValue = true)
        {
            var months = new Dictionary<int, string> // Создаем словарь для хранения названий месяцев
            {
                { 1, "Январь" },
                { 2, "Февраль" },
                { 3, "Март" },
                { 4, "Апрель" },
                { 5, "Май" },
                { 6, "Июнь" },
                { 7, "Июль" },
                { 8, "Август" },
                { 9, "Сентябрь" },
                { 10, "Октябрь" },
                { 11, "Ноябрь" },
                { 12, "Декабрь" }
            };

            if (addDafaultValue) // Если нужно добавить дополнительное значение
            {
                months.Add(0, "За весь год"); // Добавляем значение "За весь год" в словарь
            }

            return months; // Возвращаем словарь с названиями месяцев
        }
    }
}