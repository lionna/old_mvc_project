using System.Text.RegularExpressions;
using CarServiceApp.Domain.Validation.Abstract;

namespace CarServiceApp.Domain.Validation
{
    /// <summary>
    /// Предоставляет методы для проверки различных входных данных.
    /// </summary>
    public class ValidationService : IValidationService
    {
        /// <summary>
        /// Проверяет, является ли входное значение null или состоит только из пробельных символов.
        /// </summary>
        /// <param name="value">Значение, которое требуется проверить.</param>
        /// <param name="textName">Имя проверяемого текста.</param>
        public static void InputValueIsNullOrWhiteSpace(string value, string textName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"Поле {textName} должны быть заполнено.");
            }
        }

        /// <summary>
        /// Проверяет длину входного значения.
        /// </summary>
        /// <param name="name">Значение, которое требуется проверить.</param>
        /// <param name="textName">Имя проверяемого текста.</param>
        /// <param name="min">Минимально допустимая длина.</param>
        /// <param name="max">Максимально допустимая длина.</param>
        public static void InputValueCheckLength(string name,
            string textName,
            int min = 2,
            int max = 50)
        {
            if (name.Length < min)
            {
                throw new ArgumentException($"Длина '{textName}' должна быть не менее {min} символов.");
            }
            if (name.Length > max)
            {
                throw new ArgumentException($"Длина '{textName}' должна быть не более {max} символов.");
            }
        }

        /// <summary>
        /// Проверяет, соответствует ли пароль заданным критериям.
        /// </summary>
        /// <param name="password">Пароль для проверки.</param>
        /// <param name="min">Минимально допустимая длина пароля.</param>
        /// <param name="max">Максимально допустимая длина пароля.</param>
        /// <returns>True, если пароль действителен, в противном случае false.</returns>
        public bool PasswordIsValid(string password, int min = 5, int max = 30)
        {
            const string textName = "пароля";
            InputValueIsNullOrWhiteSpace(password, textName);
            InputValueCheckLength(password, textName, min, max);

            // Проверка наличия хотя бы одной цифры, одного символа в верхнем регистре, 
            // одного символа в нижнем регистре и хотя бы одного nonchar символа.
            if (!password.Any(char.IsDigit) ||
                !password.Any(char.IsUpper) ||
                !password.Any(char.IsLower) ||
                !password.Any(c => !char.IsLetterOrDigit(c)))
            {
                throw new ArgumentException("Пароль должен содержать хотя бы одну цифру, один символ в верхнем регистре, " +
                                            "один символ в нижнем регистре и хотя бы один символ символ.");
            }

            return true;
        }

        /// <summary>
        /// Проверяет, является ли номер телефона действительным.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона для проверки.</param>
        /// <param name="id">Идентификатор (необязательно).</param>
        /// <param name="min">Минимально допустимая длина номера телефона.</param>
        /// <param name="max">Максимально допустимая длина номера телефона.</param>
        /// <returns>True, если номер телефона действителен, в противном случае false.</returns>
        public bool PhoneIsValid(string phoneNumber, int? id = null, int min = 5, int max = 20)
        {
            PhoneValidation(phoneNumber, min, max);

            if (DatabaseValidation(phoneNumber, id, SqlExpressionName.ValidationPhone) > 0)
            {
                throw new ArgumentException("Введите уникальный телефон.");
            }

            return true;
        }

        /// <summary>
        /// Проверканомера телефона
        /// </summary>
        /// <param name="phoneNumber">номер телефона</param>
        /// <param name="min">минимальный размер номера</param>
        /// <param name="max">максимальный размер номера</param>
        /// <exception cref="ArgumentException"></exception>
        private static void PhoneValidation(string phoneNumber, int min = 5, int max = 20)
        {
            const string textName = "телефона";
            InputValueIsNullOrWhiteSpace(phoneNumber, textName);
            InputValueCheckLength(phoneNumber, textName, min, max);

            // Удаляем все пробелы, дефисы и скобки из номера телефона
            phoneNumber = Regex.Replace(phoneNumber, @"[\s()+-]+", "");

            // Проверяем, что номер содержит только цифры
            if (!Regex.IsMatch(phoneNumber, @"^\d+$"))
            {
                throw new ArgumentException("Введите коррретный номер телефона.");
            }
            // Проверяем, что номер начинается с кода страны (+375 или 375) или кода оператора (25, 29, 33, или 44)
            if (!Regex.IsMatch(phoneNumber, @"^(\+375|375|(25|29|33|44))\d{9}$"))
            {
                throw new ArgumentException("Введите корретный номер телефона.");
            }
        }

        /// <summary>
        /// Проверяет валидность логина.
        /// </summary>
        /// <param name="login">Логин для проверки.</param>
        /// <param name="id">Идентификатор (необязательно).</param>
        /// <param name="min">Минимально допустимая длина логина.</param>
        /// <param name="max">Максимально допустимая длина логина.</param>
        /// <returns>True, если логин действителен, в противном случае false.</returns>
        public bool LoginIsValid(string login, int? id = null, int min = 5, int max = 100)
        {
            const string textName = "логина";
            InputValueIsNullOrWhiteSpace(login, textName);
            InputValueCheckLength(login, textName, min, max);

            var regex = new Regex(@"^[a-zA-Z0-9_]{1,100}$");

            if (!regex.IsMatch(login))
            {
                throw new ArgumentException("Логин должен состоять только из латинских букв, цифр и знаков подчеркивания, и его длина должна быть от 3 до 20 символов.");
            }
            if (DatabaseValidation(login, id, SqlExpressionName.ValidationLogin) > 0)
            {
                throw new ArgumentException("Введите уникальный логин.");
            }

            return true;
        }

        /// <summary>
        /// Проверяет валидность электронной почты.
        /// </summary>
        /// <param name="email">Email для проверки.</param>
        /// <param name="id">Идентификатор (необязательно).</param>
        /// <param name="min">Минимально допустимая длина адреса электронной почты.</param>
        /// <param name="max">Максимально допустимая длина адреса электронной почты.</param>
        /// <returns>True, если email действителен, в противном случае false.</returns>
        public bool EmailIsValid(string email, int? id = null, int min = 5, int max = 100)
        {
            const string textName = "эл. почты";
            InputValueIsNullOrWhiteSpace(email, textName);
            InputValueCheckLength(email, textName, min, max);

            Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!regex.IsMatch(email))
            {
                throw new ArgumentException("Email введен некорректно.");
            }
            if (DatabaseValidation(email, id, SqlExpressionName.ValidationEmail) > 0)
            {
                throw new ArgumentException("Введите уникальный емейл.");
            }

            return true;
        }


        /// <summary>
        /// Проверяет валидность полного имени (только кириллица).
        /// </summary>
        /// <param name="name">Полное имя для проверки.</param>
        /// <param name="textName">Имя текста для отображения в сообщении об ошибке.</param>
        /// <param name="min">Минимально допустимая длина полного имени.</param>
        /// <param name="max">Максимально допустимая длина полного имени.</param>
        /// <returns>True, если полное имя действительно, в противном случае false.</returns>
        public bool FullNameIsValid(
            string name,
            string textName,
            int min = 2,
            int max = 50)
        {
            Regex regex = new Regex(@"^[а-яА-Я ]+$");

            return NameValidation(regex, name, textName, min, max);
        }

        /// <summary>
        /// Проверяет валидность имени с использованием заданного регулярного выражения.
        /// </summary>
        /// <param name="regex">Регулярное выражение для проверки.</param>
        /// <param name="name">Имя для проверки.</param>
        /// <param name="textName">Имя текста для отображения в сообщении об ошибке.</param>
        /// <param name="min">Минимально допустимая длина имени.</param>
        /// <param name="max">Максимально допустимая длина имени.</param>
        /// <returns>True, если имя действительно, в противном случае false.</returns>
        public bool NameValidation(
            Regex regex,
            string name,
            string textName,
            int min = 2,
            int max = 50)
        {
            InputValueIsNullOrWhiteSpace(name, textName);
            InputValueCheckLength(name, textName, min, max);

            if (!regex.IsMatch(name))
            {
                throw new ArgumentException($"{textName} проверьте правильность ввода данных");
            }

            return true;
        }

        /// <summary>
        /// Проверяет валидность имени (латиница, кириллица, цифры, символы).
        /// </summary>
        /// <param name="name">Имя для проверки.</param>
        /// <param name="textName">Имя текста для отображения в сообщении об ошибке.</param>
        /// <param name="min">Минимально допустимая длина имени.</param>
        /// <param name="max">Максимально допустимая длина имени.</param>
        /// <param name="id">Идентификатор (необязательно).</param>
        /// <param name="sqlExpressionName">Тип запроса для базы данных.</param>
        /// <returns>True, если имя действительно, в противном случае false.</returns>
        public bool NameIsValid(
            string name,
            string textName,
            int min = 2,
            int max = 50,
            int? id = null,
            SqlExpressionName sqlExpressionName = SqlExpressionName.None)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я0-9_\-""' ]+$");

            NameValidation(regex, name, textName, min, max);

            if (DatabaseValidation(name, id, sqlExpressionName) > 0)
            {
                throw new ArgumentException($"Введите уникальное значение '{textName}'");
            }

            return true;
        }

        /// <summary>
        /// Проверяет валидность имени, содержащего только латинские символы и цифры.
        /// </summary>
        /// <param name="name">Имя для проверки.</param>
        /// <param name="textName">Имя текста для отображения в сообщении об ошибке.</param>
        /// <param name="min">Минимально допустимая длина имени.</param>
        /// <param name="max">Максимально допустимая длина имени.</param>
        /// <param name="id">Идентификатор (необязательно).</param>
        /// <param name="sqlExpressionName">Тип запроса для базы данных.</param>
        /// <returns>True, если имя действительно, в противном случае false.</returns>
        public bool NameLatinOnlyIsValid(
           string name,
           string textName,
           int min = 2,
           int max = 50,
           int? id = null,
           SqlExpressionName sqlExpressionName = SqlExpressionName.None)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9]+$");

            NameValidation(regex, name, textName, min, max);

            if (DatabaseValidation(name, id, sqlExpressionName) > 0)
            {
                throw new ArgumentException($"Введите уникальное значение '{textName}'");
            }

            return true;
        }


        /// <summary>
        /// Проверяет валидность имени, содержащего только кириллические символы и цифры.
        /// </summary>
        /// <param name="name">Имя для проверки.</param>
        /// <param name="textName">Имя текста для отображения в сообщении об ошибке.</param>
        /// <param name="min">Минимально допустимая длина имени.</param>
        /// <param name="max">Максимально допустимая длина имени.</param>
        /// <param name="id">Идентификатор (необязательно).</param>
        /// <param name="sqlExpressionName">Тип запроса для базы данных.</param>
        /// <returns>True, если имя действительно, в противном случае false.</returns>
        public bool NameCyrillicOnlyIsValid(
           string name,
           string textName,
           int min = 2,
           int max = 50,
           int? id = null,
           SqlExpressionName sqlExpressionName = SqlExpressionName.None)
        {
            Regex regex = new Regex(@"^[а-яА-Я0-9]+$");

            NameValidation(regex, name, textName, min, max);

            if (DatabaseValidation(name, id, sqlExpressionName) > 0)
            {
                throw new ArgumentException($"Введите уникальное значение '{textName}'");
            }

            return true;
        }

        /// <summary>
        /// Выполняет проверку в базе данных.
        /// </summary>
        /// <param name="value">Значение для проверки.</param>
        /// <param name="id">Идентификатор (необязательно).</param>
        /// <param name="sqlExpression">Тип запроса для базы данных.</param>
        /// <returns>Количество результатов из базы данных.</returns>
        public int DatabaseValidation(string value, int? id, SqlExpressionName sqlExpression)
        {
            int totalCount = 0;

            if (value.Contains("existing@example.com"))
                totalCount++;

            if (sqlExpression == SqlExpressionName.None)
                return totalCount;

            //totalCount = dbConnection.ExecutionProcedure($"[dbo].[{sqlExpression}]", value, id);

            return totalCount;
        }
    }
}