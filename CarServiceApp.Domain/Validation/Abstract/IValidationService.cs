using System.Text.RegularExpressions;

namespace CarServiceApp.Domain.Validation.Abstract
{
    public interface IValidationService
    {
        bool PasswordIsValid(string password, int min = 5, int max = 30);
        bool PhoneIsValid(string phoneNumber, int? id = null, int min = 5, int max = 20);
        bool LoginIsValid(string login, int? id = null, int min = 5, int max = 100);
        bool EmailIsValid(string email, int? id = null, int min = 5, int max = 100);
        bool FullNameIsValid(string name, string textName, int min = 2, int max = 50);
        bool NameValidation(Regex regex, string name, string textName, int min = 2, int max = 50);
        bool NameIsValid(
            string name,
            string textName,
            int min = 2,
            int max = 50,
            int? id = null,
            SqlExpressionName sqlExpressionName = SqlExpressionName.None);
        bool NameLatinOnlyIsValid(
           string name,
           string textName,
           int min = 2,
           int max = 50,
           int? id = null,
           SqlExpressionName sqlExpressionName = SqlExpressionName.None);
        bool NameCyrillicOnlyIsValid(
           string name,
           string textName,
           int min = 2,
           int max = 50,
           int? id = null,
           SqlExpressionName sqlExpressionName = SqlExpressionName.None);
        int DatabaseValidation(string value, int? id, SqlExpressionName sqlExpression);
    }

}
