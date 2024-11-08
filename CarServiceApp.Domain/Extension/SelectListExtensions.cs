using Microsoft.AspNetCore.Mvc.Rendering; // Импорт пространства имен для классов SelectList и SelectListItem

namespace CarServiceApp.Domain.Extension
{
    /// <summary>
    /// Статический класс, предоставляющий методы расширения для класса SelectList.
    /// </summary>
    public static class SelectListExtensions
    {
        /// <summary>
        /// Метод расширения, который преобразует словарь целочисленных значений в SelectList.
        /// </summary>
        /// <param name="dictionary">Словарь, содержащий пары ключ-значение</param>
        /// <param name="dataValueField">Имя поля, содержащего значение данных</param>
        /// <param name="dataTextField">Имя поля, содержащего текстовое представление данных</param>
        /// <returns>Объект SelectList, созданный из словаря</returns>
        public static SelectList ToSelectList(this Dictionary<int, int> dictionary, string dataValueField, string dataTextField)
        {
            // Преобразуем каждую пару ключ-значение словаря в объект SelectListItem
            var items = dictionary.Select(kv => new SelectListItem
            {
                Value = kv.Key.ToString(), // Преобразуем ключ в строку и присваиваем значение поля Value
                Text = kv.Value.ToString() // Преобразуем значение в строку и присваиваем значение поля Text
            });

            // Создаем новый объект SelectList, используя список элементов SelectListItem и указанные поля данных
            return new SelectList(items, dataValueField, dataTextField);
        }

        /// <summary>
        /// Метод расширения, который преобразует словарь строковых значений в SelectList.
        /// </summary>
        /// <param name="dictionary">Словарь, содержащий пары ключ-значение</param>
        /// <param name="dataValueField">Имя поля, содержащего значение данных</param>
        /// <param name="dataTextField">Имя поля, содержащего текстовое представление данных</param>
        /// <returns>Объект SelectList, созданный из словаря</returns>
        public static SelectList ToSelectList(this Dictionary<int, string> dictionary, string dataValueField, string dataTextField)
        {
            // Преобразуем каждую пару ключ-значение словаря в объект SelectListItem
            var items = dictionary.Select(kv => new SelectListItem
            {
                Value = kv.Key.ToString(), // Преобразуем ключ в строку и присваиваем значение поля Value
                Text = kv.Value // Присваиваем значение поля Text
            });

            // Создаем новый объект SelectList, используя список элементов SelectListItem и указанные поля данных
            return new SelectList(items, dataValueField, dataTextField);
        }


        /// <summary>
        /// Метод расширения, который преобразует словарь строковых значений в SelectList.
        /// </summary>
        /// <param name="dictionary">Словарь, содержащий пары ключ-значение</param>
        /// <param name="dataValueField">Имя поля, содержащего значение данных</param>
        /// <param name="dataTextField">Имя поля, содержащего текстовое представление данных</param>
        /// <returns>Объект SelectList, созданный из словаря</returns>
        public static SelectList ToSelectList(this Dictionary<object, string> dictionary, string dataValueField, string dataTextField)
        {
            // Преобразуем каждую пару ключ-значение словаря в объект SelectListItem
            var items = dictionary.Select(kv => new SelectListItem
            {
                Value = kv.Key.ToString(), // Преобразуем ключ в строку и присваиваем значение поля Value
                Text = kv.Value // Присваиваем значение поля Text
            });

            // Создаем новый объект SelectList, используя список элементов SelectListItem и указанные поля данных
            return new SelectList(items, dataValueField, dataTextField);
        }
    }
}
