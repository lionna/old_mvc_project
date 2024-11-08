using CarServiceApp.Domain.Model;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IDiagramService
    {
        DiagramModel[] GetItems(int? year, int? month, Dictionary<int, string> months);

        object[] GetIncomeItems(int? targetYear, int? targetMonth, Dictionary<int, string> monthNames);
    }
}