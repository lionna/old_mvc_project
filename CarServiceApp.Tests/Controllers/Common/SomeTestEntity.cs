using CarServiceApp.Domain.Entity.Abstract;

namespace CarServiceApp.Tests.Controllers.Common
{
    public class SomeTestEntity : ICommonEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SomeEntityWithDictionary : ICommonEntityWithDictinary<int>
    {
        public int Id { get; set; }
        public Dictionary<object, string> DropdownList { get; set; }
        public string Name { get; set; }
    }

    public interface ISomeDictionaryService
    {
        Task<Dictionary<object, string>> GetDictionaryAsync();
    }
}
