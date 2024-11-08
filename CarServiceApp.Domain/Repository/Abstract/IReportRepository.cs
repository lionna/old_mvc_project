using CarServiceApp.Domain.Model;

namespace CarServiceApp.Domain.Repository.Abstract
{
    public interface IReportRepository
    {
        Task<IEnumerable<ReportResultModel>> AnalyseProcedureAsync(
            DateTime? highBoundary,
            DateTime? lowBoundary,
            string procedureName,
            string id,
            string name);

        Task<List<AmountMoneyByProvider>> AmountMoneyByProvidersAsync();

        Task<IEnumerable<ReportByExecServiceByMonthForEmployee>> ReportServiceByMonthForEmployee(
           int? year,
          int? month);

        Task<IEnumerable<ReportByOrderModel>> ReportOrdersAsync();
    }
}
