using CarServiceApp.Domain.Model;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IReportService
    {
        Task<IEnumerable<ReportResultModel>> AnalysePartsTrade(int typePeriodId, DateTime? highBoundary, DateTime? lowBoundary);

        Task<IEnumerable<ReportResultModel>> AnalyseServicesTrade(int typePeriodId, DateTime? highBoundary, DateTime? lowBoundary);

        Task<List<ReportByOrderModel>> ReportByExecOrdersAsync(int? year, int? month, int? numbSortRow, bool? isAsc);

        Task<IEnumerable<AmountMoneyByProvider>> AmountMoneyByProvidersAsync();

        Task<IEnumerable<ReportByExecServiceByMonthForEmployee>> ReportByExecServiceByMonthForEmployeesAsync(
           int? year,
           int? month);
    }
}