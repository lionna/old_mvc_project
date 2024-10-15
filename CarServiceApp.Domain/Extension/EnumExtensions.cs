namespace CarServiceApp.Domain.Extension
{
    /// <summary>
    /// Статический класс, предоставляющий методы расширения для работы с перечислениями (enum).
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Метод, который возвращает имена значений перечисления (enum).
        /// </summary>
        /// <typeparam name="TEnum">Тип перечисления (enum)</typeparam>
        /// <returns>Массив строк, содержащий имена значений перечисления</returns>
        public static string[] GetEnumNamesAndValues<TEnum>() where TEnum : Enum
        {
            // Возвращаем массив строк, содержащий имена значений перечисления
            return Enum.GetNames(typeof(TEnum));
        }

        /// <summary>
        /// Метод, который преобразует перечисление (enum) в словарь, где ключ - это числовое значение перечисления, а значение - его строковое представление.
        /// </summary>
        /// <typeparam name="TEnum">Тип перечисления (enum)</typeparam>
        /// <returns>Словарь, содержащий числовые значения и строковые представления значений перечисления</returns>
        public static Dictionary<int, string> ToDictionary<TEnum>() where TEnum : Enum
        {
            // Преобразуем значения перечисления в список и преобразуем список в словарь
            return Enum.GetValues(typeof(TEnum))
                .Cast<int>() // Приводим каждый элемент к типу int
                .ToDictionary(value => value, value => Enum.GetName(typeof(TEnum), value)); // Создаем словарь, где ключ - это числовое значение перечисления, а значение - его строковое представление
        }
    }
}
