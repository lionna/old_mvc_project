using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using System.Data;

namespace CarServiceApp.Domain.Service
{
    public class ReportService(IReportRepository reportRepository) : IReportService
    {
        private readonly IReportRepository _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));

        public async Task<IEnumerable<AmountMoneyByProvider>> AmountMoneyByProvidersAsync()
        {
            return await _reportRepository.AmountMoneyByProvidersAsync();
        }

        public async Task<IEnumerable<ReportByExecServiceByMonthForEmployee>> ReportByExecServiceByMonthForEmployeesAsync(
           int? year,
           int? month)
        {
            return (await _reportRepository.ReportServiceByMonthForEmployee(year, month))
                .OrderByDescending(m => m.TotalMoney);
        }

        public async Task<List<ReportByOrderModel>> ReportByExecOrdersAsync(
            int? year,
            int? month,
            int? numbSortRow = 3,
            bool? isAsc = true)
        {
            List<ReportByOrderModel> report;
            var model = await _reportRepository.ReportOrdersAsync();
            switch (numbSortRow.Value)
            {
                case 1:
                    report = isAsc != null && isAsc.Value
                        ? model.Where(d => d.DateMaking.Value.Year == year && d.DateMaking.Value.Month == month)
                        .OrderBy(o => o.FullName).ToList()
                        : model.Where(d => d.DateMaking.Value.Year == year && d.DateMaking.Value.Month == month)
                        .OrderByDescending(o => o.FullName).ToList();
                    break;

                case 2:
                    report = isAsc != null && isAsc.Value
                        ? model.Where(d => d.DateMaking.Value.Year == year && d.DateMaking.Value.Month == month)
                        .OrderBy(o => o.TotallPriceOrder).ToList()
                        : model.Where(d => d.DateMaking.Value.Year == year && d.DateMaking.Value.Month == month)
                        .OrderByDescending(o => o.TotallPriceOrder).ToList();
                    break;

                case 3:
                    report = isAsc != null && isAsc.Value
                        ? model.Where(d => d.DateMaking.Value.Year == year && d.DateMaking.Value.Month == month)
                        .OrderBy(o => o.DateMaking).ToList()
                        : model.Where(d => d.DateMaking.Value.Year == year && d.DateMaking.Value.Month == month)
                        .OrderByDescending(o => o.DateMaking).ToList();
                    break;

                default:
                    report = model
                        .Where(d => d.DateMaking.Value.Year == year && d.DateMaking.Value.Month == month)
                        .OrderBy(o => o.DateMaking).ToList();
                    break;
            }

            return report;
        }

        public async Task<IEnumerable<ReportResultModel>> AnalyseServicesTrade(
            int typePeriodId,
            DateTime? highBoundary,
            DateTime? lowBoundary)
        {
            string procedureName;

            switch (typePeriodId)
            {
                case 2:
                    procedureName = "ABC_XYZ_services_by_quater";
                    break;

                case 3:
                    procedureName = "ABC_XYZ_services_by_day";
                    break;

                default:
                    procedureName = "ABC_XYZ_services_by_month";
                    break;
            }

            return await _reportRepository.AnalyseProcedureAsync(highBoundary, lowBoundary, procedureName, "ServiceId", "ServiceName");
        }

        public async Task<IEnumerable<ReportResultModel>> AnalysePartsTrade(
            int typePeriodId,
            DateTime? highBoundary,
            DateTime? lowBoundary)
        {
            string procedureName;

            switch (typePeriodId)
            {
                case 2:
                    procedureName = "ABC_XYZ_parts_by_quarter";
                    break;

                case 3:
                    procedureName = "ABC_XYZ_parts_by_day";
                    break;

                default:
                    procedureName = "ABC_XYZ_parts_by_month";
                    break;
            }

            return await _reportRepository.AnalyseProcedureAsync(highBoundary, lowBoundary, procedureName, "PartId", "Name");
        }
    }
}