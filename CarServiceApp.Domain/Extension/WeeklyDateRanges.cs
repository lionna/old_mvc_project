namespace CarServiceApp.Domain.Extension
{
    /// <summary>
    /// Статический класс, предоставляющий методы для работы с диапазонами дат по неделям.
    /// </summary>
    public static class WeeklyDateRanges
    {
        /// <summary>
        /// Метод, который возвращает первый день недели для указанной даты.
        /// </summary>
        /// <param name="dat">Дата</param>
        /// <returns>Первый день недели</returns>
        public static DateTime GetFirstDayOfWeek(this DateTime dat)
        {
            // Определяем номер дня недели (1 - понедельник, ..., 7 - воскресенье)
            int day = (int)dat.DayOfWeek == 0 ? 7 : ((int)dat.DayOfWeek);
            // Возвращаем дату, соответствующую первому дню недели (понедельнику)
            return dat.Date.AddDays(-day + 1);
        }

        /// <summary>
        /// Метод, который возвращает последний день недели для указанной даты.
        /// </summary>
        /// <param name="dat">Дата</param>
        /// <returns>Последний день недели</returns>
        public static DateTime GetLastDayOfWeek(this DateTime dat)
        {
            // Определяем количество дней, которые нужно прибавить к указанной дате, чтобы получить последний день недели (воскресенье)
            int day = (int)dat.DayOfWeek == 0 ? 0 : 7 - (int)dat.DayOfWeek;
            // Получаем последний день недели и устанавливаем время на конец дня
            var date = dat.Date.AddDays(day);
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }
    }
}
