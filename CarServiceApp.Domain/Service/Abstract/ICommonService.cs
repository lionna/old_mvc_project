namespace CarServiceApp.Domain.Service.Abstract
{
    public interface ICommonService
    {
        Dictionary<int, int> Years(int? selectYear = null);

        Dictionary<int, string> Months(bool addDafaultValue = true);
    }
}