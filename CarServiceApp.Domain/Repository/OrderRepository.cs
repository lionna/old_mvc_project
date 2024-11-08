using CarServiceApp.Domain.Config;
using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Extension;
using CarServiceApp.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace CarServiceApp.Domain.Repository
{
    public interface IOrderRepository
    {
        Task<SelectList> ListEmployeeAsync(string serviceId);
        Task<int> AddServiceToOrderAsync(
            string orderId,
            string serviceId,
            DateTime? dateComplete,
            double? takeTime,
            string notes,
            int? personnelNumber,
            int taxAddedValue,
            bool isEdit);
        Task<int> CloseOrderAsync(
           string orderId,
           DateTime? dateFactComplete,
           DateTime? completeDate,
           int? paymentTypeId,
           int? personnelNumberCloser,
           bool? isCompleted,
           string rejectionReason);

        Task<List<AllInfoAttachedPart>> GetAllInfoAttachedPartByIdAsync(string orderId);
        Task<List<TotalInfoServicesByOrder>> GetTotalInfoServicesByOrderAsync(string orderId);
        Task<IEnumerable<ReportAcceptedCustomParts>> GetReportAcceptedCustomParts(string orderId, string clientId);
        Task<(int returnValue, string orderId, string errorMessage)> AddPartialOrderForRecordAsync(
            DateTime? dateMaking,
            string clientId,
            string vinNumber,
            string description,
            int? currentMill,
            int employeeId,
            long? recordId,
            byte numberWheelCaps,
            byte numberWipers,
            byte numberWipersArms,
            bool isAntenna,
            bool isSpareWheel,
            bool isCoverDecorEngine,
            bool isTuner,
            byte? fluelLevelPercent);

        Task<(int returnValue, string orderId, string errorMessage)> AddPartOrderAsync(
            DateTime? dateMaking, string clientId, string vinNumber, string description,
            int? currentMill, int employeeId, byte numberWheelCaps, byte numberWipers, byte numberWipersArms,
            bool isAntenna, bool isSpareWheel, bool isCoverDecorEngine, bool isTuner, byte? fluelLevelPercent);
    }

    public class OrderRepository(
        //ApplicationDbContext context, 
        IOptions<ApplicationSettings> settings) : IOrderRepository
    {
        private readonly string connectionString = settings?.Value?.DefaultConnection ?? throw new ArgumentNullException(nameof(settings));
        //private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<(int returnValue, string orderId, string errorMessage)> AddPartOrderAsync(
    DateTime? dateMaking, string clientId, string vinNumber, string description,
        int? currentMill, int employeeId, byte numberWheelCaps, byte numberWipers, byte numberWipersArms,
        bool isAntenna, bool isSpareWheel, bool isCoverDecorEngine, bool isTuner, byte? fluelLevelPercent)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "AddPartOrder";
                    command.Parameters.AddWithValue("@DateMaking", dateMaking);
                    command.Parameters.AddWithValue("@ClientId", clientId);
                    command.Parameters.AddWithValue("@VINumber", vinNumber);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@CurrentMill", currentMill);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.Parameters.AddWithValue("@NumberWheelCaps", numberWheelCaps);
                    command.Parameters.AddWithValue("@NumberWipers", numberWipers);
                    command.Parameters.AddWithValue("@NumberWipersArms", numberWipersArms);
                    command.Parameters.AddWithValue("@IsAntenna", isAntenna);
                    command.Parameters.AddWithValue("@IsSpareWheel", isSpareWheel);
                    command.Parameters.AddWithValue("@IsCoverDecorEngine", isCoverDecorEngine);
                    command.Parameters.AddWithValue("@IsTuner", isTuner);
                    command.Parameters.AddWithValue("@FluelLevelPercent", fluelLevelPercent);

                    var orderIdParam = command.Parameters.Add("@OrderId", SqlDbType.NVarChar, 20);
                    orderIdParam.Direction = ParameterDirection.Output;

                    var returnValueParam = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    returnValueParam.Direction = ParameterDirection.ReturnValue;

                    var errorMessageParam = command.Parameters.Add("@ErrorMessage", SqlDbType.NVarChar, 200);
                    errorMessageParam.Direction = ParameterDirection.Output;

                    try
                    {
                        await connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();

                        int returnValue = (int)returnValueParam.Value;
                        string orderId = (string)orderIdParam.Value;
                        string errorMessage = (string)errorMessageParam.Value;

                        return (returnValue, orderId, errorMessage);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public async Task<(int returnValue, string orderId, string errorMessage)> AddPartialOrderForRecordAsync(
            DateTime? dateMaking,
            string clientId,
            string vinNumber,
            string description,
            int? currentMill,
            int employeeId,
            long? recordId,
            byte numberWheelCaps,
            byte numberWipers,
            byte numberWipersArms,
            bool isAntenna,
            bool isSpareWheel,
            bool isCoverDecorEngine,
            bool isTuner,
            byte? fluelLevelPercent)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "AddPartialOrderForRecord";
                    command.Parameters.AddWithValue("@DateMaking", dateMaking);
                    command.Parameters.AddWithValue("@ClientId", clientId);
                    command.Parameters.AddWithValue("@VINnumber", vinNumber);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@CurrentMill", currentMill);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.Parameters.AddWithValue("@RecordId", recordId);
                    command.Parameters.AddWithValue("@NumberWheelCaps", numberWheelCaps);
                    command.Parameters.AddWithValue("@NumberWipers", numberWipers);
                    command.Parameters.AddWithValue("@NumberWipersArms", numberWipersArms);
                    command.Parameters.AddWithValue("@IsAntenna", isAntenna);
                    command.Parameters.AddWithValue("@IsSpareWheel", isSpareWheel);
                    command.Parameters.AddWithValue("@IsCoverDecorEngine", isCoverDecorEngine);
                    command.Parameters.AddWithValue("@IsTuner", isTuner);
                    command.Parameters.AddWithValue("@FluelLevelPercent", fluelLevelPercent);

                    var orderIdParam = command.Parameters.Add("@OrderId", SqlDbType.NVarChar, 20);
                    orderIdParam.Direction = ParameterDirection.Output;

                    var returnValueParam = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    returnValueParam.Direction = ParameterDirection.ReturnValue;

                    var errorMessageParam = command.Parameters.Add("@ErrorMessage", SqlDbType.NVarChar, 200);
                    errorMessageParam.Direction = ParameterDirection.Output;

                    try
                    {
                        await connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();

                        int returnValue = (int)returnValueParam.Value;
                        string orderId = (string)orderIdParam.Value;
                        string errorMessage = (string)errorMessageParam.Value;

                        return (returnValue, orderId, errorMessage);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public async Task<IEnumerable<ReportAcceptedCustomParts>> GetReportAcceptedCustomParts(string orderId, string clientId)
        {
            List<ReportAcceptedCustomParts> list = [];

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetReportAcceptedCustomParts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.Parameters.AddWithValue("@ClientId", clientId);
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var result = new ReportAcceptedCustomParts
                            {
                                PartId = reader["PartId"] as string,
                                Name = reader["Name"] as string,
                                Unit = reader["Unit"] as string,
                                Manufacture = reader["Manufacture"] as string,
                                FullName = reader["FullName"] as string,
                                StateSPart = reader["StateSPart"] as string,
                                ClientId = reader["ClientId"] as string,
                                AcceptDate = reader["AcceptDate"] as DateTime?,
                                Number = reader["Number"] as double?,
                                StockCustom = reader["StockCustom"] as double?,
                                AcceptanceId = (int)reader["AcceptanceId"]
                            };

                            list.Add(result);
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return list;
        }

        public async Task<List<TotalInfoServicesByOrder>> GetTotalInfoServicesByOrderAsync(string orderId)
        {
            var list = new List<TotalInfoServicesByOrder>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetTotalInfoServicesByOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var result = new TotalInfoServicesByOrder
                            {
                                ServiceName = reader["ServiceName"] as string,
                                ServiceId = reader["ServiceId"] as string,
                                PriceHour = reader["PriceHour"] as decimal?,
                                RateTime = reader["RateTime"] as double?,
                                TakeTime = reader["TakeTime"] as double?,
                                TaxAddedValue = reader["TaxAddedValue"] as int?,
                                SummMoney = reader["SummMoney"] as double?,
                                DateCompleting = reader["DateCompleting"] as DateTime?,
                                FullName = reader["FullName"] as string,
                            };

                            list.Add(result);
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return list;
        }

        public async Task<List<AllInfoAttachedPart>> GetAllInfoAttachedPartByIdAsync(string orderId)
        {
            var list = new List<AllInfoAttachedPart>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetAllInfoAttachedPartById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var result = new AllInfoAttachedPart
                            {
                                Name = reader["Name"] as string,
                                PartId = reader["PartId"] as string,
                                Manufacture = reader["Manufacture"] as string,
                                LotNumber = reader["LotNumber"] as int?,
                                Price = reader["Price"] as decimal?,
                                CostPart = reader["CostPart"] as decimal?,
                                Unit = reader["Unit"] as string,
                                Number = reader["Number"] as double?,
                                Summ = reader["Summ"] as double?,
                                FullName = reader["FullName"] as string,
                            };

                            list.Add(result);
                        }
                    }
                }

                await connection.CloseAsync();
            }
            return list;
        }

        public async Task<int> CloseOrderAsync(
           string orderId, DateTime? dateFactComplete, DateTime? completeDate,
           int? paymentTypeId, int? personnelNumberCloser, bool? isCompleted, string rejectionReason)
        {
            int returnCode = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("CloseOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.Parameters.AddWithValue("@DateFactComplete", dateFactComplete);
                    command.Parameters.AddWithValue("@DateComplete", completeDate);
                    command.Parameters.AddWithValue("@TypePaymentId", paymentTypeId);
                    command.Parameters.AddWithValue("@PersonnelNumberCloser", personnelNumberCloser);
                    command.Parameters.AddWithValue("@IsCompleted", isCompleted);
                    command.Parameters.AddWithValue("@RejectionReason", rejectionReason);
                    command.CommandTimeout = 120;

                    SqlParameter returnParameter = command.Parameters.Add("@ReturnCode", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    await command.ExecuteNonQueryAsync();

                    returnCode = (int)returnParameter.Value;
                }
                await connection.CloseAsync();
            }

            return returnCode;
        }

        public async Task<int> AddServiceToOrderAsync(
            string orderId,
            string serviceId,
            DateTime? dateComplete,
            double? takeTime,
            string notes,
            int? personnelNumber,
            int taxAddedValue,
            bool isEdit)
        {
            int returnCode = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("AddServiceToOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.Parameters.AddWithValue("@ServiceId", serviceId);
                    command.Parameters.AddWithValue("@DateComplete", dateComplete);
                    command.Parameters.AddWithValue("@TakeTime", takeTime);
                    command.Parameters.AddWithValue("@Notes", notes);
                    command.Parameters.AddWithValue("@PersonalId", personnelNumber);
                    command.Parameters.AddWithValue("@TaxAdded", taxAddedValue);
                    command.Parameters.AddWithValue("@IsEdit", isEdit);
                    command.CommandTimeout = 120;

                    SqlParameter returnParameter = command.Parameters.Add("@ReturnCode", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    await command.ExecuteNonQueryAsync();

                    returnCode = (int)returnParameter.Value;
                }

                await connection.CloseAsync();
            }

            return returnCode;
        }

        public async Task<SelectList> ListEmployeeAsync(string serviceId)
        {
            var list = new Dictionary<object, string>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetListAppropriateEmployees", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServiceId", serviceId);
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var id = (int)reader[0];
                            if (!list.ContainsKey(id))
                                list.Add(id, reader[1] as string);
                        }
                    }
                }
            }

            return list.ToSelectList("Value", "Text");
        }
    }
}
