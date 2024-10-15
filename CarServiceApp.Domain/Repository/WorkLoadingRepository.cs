using CarServiceApp.Domain.Config;
using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Repository.Abstract;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace CarServiceApp.Domain.Repository
{
    public class WorkLoadingRepository(ApplicationDbContext context, IOptions<ApplicationSettings> settings) : IWorkLoadingRepository
    {
        private readonly string connectionString = settings?.Value?.DefaultConnection ?? throw new ArgumentNullException(nameof(settings));
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<List<ExecutingService>> ExecutingServicesAsync(DateTime? date, int? id)
        {
            return await _context.Set<ExecutingService>()
              .Where(s => s.PersonnelNumber == id && s.DateStart.Value == date.Value)
              .Include(e => e.OrderService.Car.CarModification.Series.CarModel.CarBrand)
              .Include(e => e.Service)
              .Include(e => e.Employee)
              .Include(e => e.OrderService.Client)
              .ToListAsync();
        }

        public async Task<List<PreRecordService>> PreRecordServiceAsync(DateTime? date, int? id)
        {
            return await _context.Set<PreRecordService>()
                .Where(s => s.PersonnelNumber == id && s.DateReservation.Value == date.Value)
                .Include(e => e.Employee)
                .Include(e => e.PreRecord)
                .Include(e => e.Service)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExecutingService>> ExecutingServicesAsync()
        {
            return await _context.Set<ExecutingService>().ToListAsync();
        }

        public async Task<IEnumerable<PreRecordService>> PreRecordServiceAsync()
        {
            return await _context.Set<PreRecordService>().ToListAsync();
        }

        public async Task<TableLoadViewModel> TableLoadAsync(DateTime? currentDate)
        {
            currentDate ??= DateTime.Now;
            var list = GetLoadTableModel(currentDate.Value);

            var model = new TableLoadViewModel
            {
                LoadEmployees = list,
                PreRecordServices = (await PreRecordServiceAsync()).ToList()
            };

            return model;
        }

        public async Task<OrderService> GetOrderServiceById(string orderId)
        {
            return await _context.Set<OrderService>()
                .Include(e => e.Car.CarModification.Series.CarModel.CarBrand)
                .Include(e => e.Client)
                .Include(e => e.TypeOfPayment)
                .FirstOrDefaultAsync(s => s.OrderId.Equals(orderId));
        }

        public async Task<int> ExecutionProcedureAsync(string nameProcedure, Dictionary<string, object> inputParam)
        {
            int returnValue = 0;

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = nameProcedure;

                    foreach (var item in inputParam)
                    {
                        var inParam = cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                        inParam.Direction = ParameterDirection.Input;
                    }

                    var returnValueParam = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    returnValueParam.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    returnValue = (int)returnValueParam.Value;
                }
            }

            return returnValue;
        }

        public async Task<int> CancelReservHourForServiceAsync(DateTime? dateTimeReservation, int? personalNumber)
        {
            int returnValue = 0;

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "CancelReservHourForService";
                    command.Parameters.AddWithValue("@dateTimeReserv", dateTimeReservation);
                    command.Parameters.AddWithValue("@persNumber", personalNumber);

                    var returnParameter = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    await command.ExecuteNonQueryAsync();

                    returnValue = (int)returnParameter.Value;
                }
            }

            return returnValue;
        }

        public async Task<TableLoadViewModel> ReservHourForService(DateTime? currentDate)
        {
            DateTime selectedDate = currentDate ?? DateTime.Now;
            List<LoadTableModel> list = [];

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("TableLoadByDay", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@selDate", selectedDate.Date);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new LoadTableModel
                            {
                                CurrentDate = reader["CurrentDate"] as DateTime?,
                                FullName = reader["FullName"] as string,
                                PostName = reader["NamePost"] as string,
                                PersonnelNumber = (int)reader["PersonnelNumber"],
                                h09_00 = reader["h09_00"] as double?,
                                h10_00 = reader["h10_00"] as double?,
                                h11_00 = reader["h11_00"] as double?,
                                h12_00 = reader["h12_00"] as double?,
                                h13_00 = reader["h13_00"] as double?,
                                h14_00 = reader["h14_00"] as double?,
                                h15_00 = reader["h15_00"] as double?,
                                h16_00 = reader["h16_00"] as double?,
                                h17_00 = reader["h17_00"] as double?,
                                h18_00 = reader["h18_00"] as double?,
                            };

                            list.Add(result);
                        }
                    }
                }
            }

            var model = new TableLoadViewModel
            {
                LoadEmployees = list,
                PreRecordServices = (await PreRecordServiceAsync()).ToList()
            };

            return model;
        }

        public async Task<List<Entity.Models.Service>> GetServicesAsync(string partId, string partName, int takeValue)
        {
            return await _context.Set<Entity.Models.Service>()
                .Where(f => f.Name.Contains(partName) && f.Id.Contains(partId) && f.Available)
                .Take(takeValue).ToListAsync();
        }

        private List<LoadTableModel> GetLoadTableModel(DateTime selectedDate)
        {
            List<LoadTableModel> list = [];

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("TableLoadByDay", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@selDate", selectedDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new LoadTableModel
                            {
                                CurrentDate = reader["CurrentDate"] as DateTime?,
                                FullName = reader["FullName"] as string,
                                PostName = reader["NamePost"] as string,
                                PersonnelNumber = (int)reader["PersonnelNumber"],
                                h09_00 = reader["h09_00"] as double?,
                                h10_00 = reader["h10_00"] as double?,
                                h11_00 = reader["h11_00"] as double?,
                                h12_00 = reader["h12_00"] as double?,
                                h13_00 = reader["h13_00"] as double?,
                                h14_00 = reader["h14_00"] as double?,
                                h15_00 = reader["h15_00"] as double?,
                                h16_00 = reader["h16_00"] as double?,
                                h17_00 = reader["h17_00"] as double?,
                                h18_00 = reader["h18_00"] as double?,
                            };

                            list.Add(result);
                        }
                    }
                }
            }

            return list;
        }

        public async Task<TableLoadPreRecordViewModel> TableLoadPreRecordViewModelAsync(DateTime? currentDate, long? preRecordId)
        {
            currentDate ??= DateTime.Now;
            var list = GetLoadTableModel(currentDate.Value);

            var model = new TableLoadPreRecordViewModel
            {
                LoadEmployees = list,
                PreRecordServices = (await PreRecordServiceAsync()).ToList(),
                Record = await PreRecordByIdAsync(preRecordId)
            };

            return model;
        }

        private async Task<PreRecord> PreRecordByIdAsync(long? id)
        {
            return await _context.Set<PreRecord>().FindAsync(id);
        }
    }
}
