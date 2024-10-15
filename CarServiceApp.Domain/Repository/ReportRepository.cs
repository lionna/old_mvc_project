using CarServiceApp.Domain.Config;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Repository.Abstract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace CarServiceApp.Domain.Repository
{
    public class ReportRepository(IOptions<ApplicationSettings> settings) : IReportRepository
    {
        private readonly string connectionString = settings?.Value?.DefaultConnection ?? throw new ArgumentNullException(nameof(settings));

        public async Task<List<AmountMoneyByProvider>> AmountMoneyByProvidersAsync()
        {
            List<AmountMoneyByProvider> list = [];

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM AmountMoneyByProviders", connection))
                {
                    command.CommandTimeout = 120;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var result = new AmountMoneyByProvider
                            {
                                Id = (long)reader["Id"],
                                Name = reader["Name"] as string,
                                Year = reader["Year"] as int?,
                                Month = reader["Month"] as int?,
                                DeliveryDate = reader["DeliveryDate"] as DateTime?,
                                LotNumber = reader["LotNumber"] as string,
                                Amount = reader["Amount"] as double?
                            };

                            list.Add(result);
                        }
                    }
                }
            }

            return list;
        }

        public async Task<IEnumerable<ReportByOrderModel>> ReportOrdersAsync()
        {
            List<ReportByOrderModel> list = [];

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM ReportByExecOrders", connection))
                {
                    command.CommandTimeout = 120;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var result = new ReportByOrderModel
                            {
                                OrderId = reader["OrderId"] as string,
                                DateMaking = reader["DateMaking"] as DateTime?,
                                FullName = reader["FullName"] as string,
                                Model = reader["Model"] as string,
                                RegisterNumber = reader["RegistrationNumber"] as string,
                                IssueYear = reader["IssueYear"] as int?,
                                CostService = reader["CostServ"] as decimal?,
                                TaxAddedValue = reader["TaxAddedValue"] as decimal?,
                                PriceService = reader["PriceServ"] as decimal?,
                                TotallPriceParts = reader["TotallPriceParts"] as decimal?,
                                TotallPriceOrder = reader["TotallPriceOrder"] as decimal?
                            };

                            list.Add(result);
                        }
                    }
                }
            }

            return list;
        }

        public async Task<IEnumerable<ReportByExecServiceByMonthForEmployee>> ReportServiceByMonthForEmployee(int? year, int? month)
        {
            var list = new List<ReportByExecServiceByMonthForEmployee>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM ReportByExecServByMonthForEmployee WHERE Year = @Year AND Month = @Month";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Year", year ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Month", month ?? (object)DBNull.Value);
                    command.CommandTimeout = 120;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var result = new ReportByExecServiceByMonthForEmployee
                            {
                                FullName = reader["FullName"] as string,
                                PersonnelNumber = (int)reader["PersonnelNumber"],
                                NamePost = reader["NamePost"] as string,
                                CountOfOrders = reader["CountOfOrders"] as int?,
                                CountOfServices = reader["CountOfServices"] as int?,
                                TakeTime = reader["TakeTime"] as double?,
                                AveragePriceServ = reader["AveragePriceServ"] as decimal?,
                                AveragePriceOrder = reader["AveragePriceOrder"] as decimal?,
                                TotalMoney = reader["TotalMoney"] as decimal?,
                                Year = reader["Year"] as int?,
                                Month = reader["Month"] as int?
                            };

                            list.Add(result);
                        }
                    }
                }
            }

            return list;
        }

        public async Task<IEnumerable<ReportResultModel>> AnalyseProcedureAsync(
            DateTime? highBoundary,
            DateTime? lowBoundary,
            string procedureName,
            string id,
            string name)
        {
            List<ReportResultModel> list = [];
            highBoundary ??= DateTime.Now;
            lowBoundary ??= DateTime.Now;

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@highBoundary", highBoundary);
                    command.Parameters.AddWithValue("@lowBoundary", lowBoundary);
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var result = new ReportResultModel
                            {
                                Id = reader[id] as string,
                                Name = reader[name] as string,
                                SaleAmount = reader["SaleAmount"] as decimal?,
                                ABC = reader["ABC"] as string,
                                XYZ = reader["XYZ"] as string,
                                ABC_XYZ = reader["ABC_XYZ"] as string,
                            };

                            list.Add(result);
                        }
                    }
                }
            }

            return list;
        }
    }
}
