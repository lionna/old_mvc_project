using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Repository;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace CarServiceApp.Domain.Service
{
    public class OrderService(
        IGenericRepositoryAsync<ServiceType, int> serviceTypeRepository,
        IGenericRepositoryAsync<TypeOfPayment, int> typeOfPaymentRepository,
        IOrderRepository orderRepository,
        ApplicationDbContext context) : IOrderService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly IGenericRepositoryAsync<ServiceType, int> _serviceTypeRepository = serviceTypeRepository;
        private readonly IGenericRepositoryAsync<TypeOfPayment, int> _typeOfPaymentRepository = typeOfPaymentRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<bool> IsClosedOrderAsync(string id)
        {
            var query = await _context.Set<Entity.Models.OrderService>()
             .FirstOrDefaultAsync(s => s.OrderId == id);

            return query.DateFactCompleting != null;
        }

        private static Expression<Func<Entity.Models.OrderService, object>>[] GetCarIncludeInfo()
        {
            return
            [
                x => x.Car,
                x => x.Car.CarModification,
                x => x.Car.CarModification.Series,
                x => x.Car.CarModification.Series,
                x => x.Car.CarModification.Series.CarModel,
                x => x.Car.CarModification.Series.CarModel.CarBrand,
                x => x.Client,
                x=> x.ExecutingServices,
                x=>x.RemarkToStateCars
            ];
        }

        public async Task<SelectList> GetIncludedCarByClientIdAsync(string id)
        {
            var query = _context.Set<Entity.Models.OrderService>().AsQueryable();

            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(s => s.ClientId.Contains(id));
            }

            var includes = GetCarIncludeInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var result = await query.ToListAsync();

            var items = result.Select(model => new SelectListItem
            {
                Value = model.VinNumber.ToString(),
                Text = $"[{model.Car.CarModification.Series.CarModel.CarBrand.Name}] {model.Car.CarModification.Series.Name} {model.Car.CarModification.Name}"
            });

            return new SelectList(items, "Value", "Text");
        }

        public async Task<SelectList> GetEmployeesAsync(bool usePostFilter = true, int postId = 5, bool userDefaultValue = false)
        {
            const string defaultFullName = "Выбрать...";

            var employees = usePostFilter
               ? await _context.Set<Employee>().Where(f =>
               f.Post.Id == postId &&
               f.ContractToEmployees.FirstOrDefault(d => d.DismissDate == null) != null).ToListAsync()
                    : await _context.Set<Employee>().Where(f =>
                    f.ContractToEmployees.FirstOrDefault(d => d.DismissDate == null) != null).ToListAsync();

            if (userDefaultValue == false)
            {
                var items = employees.Select(model => new SelectListItem
                {
                    Value = model.PersonnelNumber.ToString(),
                    Text = model.FullName
                });

                return new SelectList(items, "Value", "Text");
            }
            else
            {
                employees.Add(new Employee { PersonnelNumber = 0, FullName = defaultFullName });
                var items = employees.Select(model => new SelectListItem
                {
                    Value = model.PersonnelNumber.ToString(),
                    Text = model.FullName
                });

                return new SelectList(items, "Value", "Text", 0);
            }
        }

        public async Task<Entity.Models.OrderService> GetOrderByIdAsync(string id)
        {
            var query = _context.Set<Entity.Models.OrderService>().AsQueryable();

            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(s => s.OrderId.Contains(id));
            }

            var includes = GetCarIncludeInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Load ExecutingServices separately and project them
            var order = await query.FirstOrDefaultAsync();
            if (order != null)
            {
                await _context.Entry(order)
                    .Collection(o => o.ExecutingServices)
                    .Query()
                    .Select(es => new
                    {
                        es.Service,
                        es.OrderService,
                        es.Employee,
                        es.UsingCustomSpartMats,
                        es.UsingPartMaterials,
                    })
                    .LoadAsync();
            }

            return order;
        }

        public async Task<SelectList> GetServiceTypesAsync()
        {
            return await _serviceTypeRepository.GetSelectListAsync();
        }

        public async Task<SelectList> GetTypeOfPaymentsAsync()
        {
            return await _typeOfPaymentRepository.GetSelectListAsync();
        }

        public async Task<TotalInfoModel> GetTotalInfoExecServicesByIdAsync(string orderId)
        {
            var totalInfo = await _context.Set<ExecutingService>()
                .Where(es => es.OrderId == orderId)
                .GroupBy(es => es.OrderId)
                .Select(g => new TotalInfoModel
                {
                    TotalTakeTime = g.Sum(es => es.TakeTime),
                    TotalMoney = g.Sum(es => es.Price * ((es.TaxAddedValue ?? 0.0m * 0.01m + 1))),
                    TotalClearMoney = g.Sum(es => es.Price)
                })
                .FirstOrDefaultAsync();

            return totalInfo;
        }

        public async Task<(decimal, decimal, double, decimal)> GetCalculatedTotalMoneyTimeByOrderAsync(string orderId)
        {
            var query = _context.Set<ExecutingService>().AsQueryable();
            var result = await query.Where(s => s.OrderId == orderId).ToListAsync();
            var query2 = _context.Set<UsingPartMaterial>().AsQueryable();
            var result2 = await query2.Where(s => s.OrderId == orderId).ToListAsync();

            decimal totalMoneyServices = result.Sum(s => s.Price) ?? 0;
            decimal totalMoneyServicesFull = result.Sum(s => (s.Price ?? 0) * ((s.TaxAddedValue ?? 0) * 0.01m + 1));
            double totalTimeServices = result.Sum(s => s.TakeTime) ?? 0;
            decimal totalMoneyPartsFull = result2.Sum(s => (s.CostPart ?? 0) * (decimal)(s.Number ?? 0));

            return (totalMoneyServices, totalMoneyServicesFull, totalTimeServices, totalMoneyPartsFull);
        }

        private static Expression<Func<Entity.Models.Service, object>>[] GetServiceIncludeInfo()
        {
            return
            [
                x => x.ServiceType,
            ];
        }

        public async Task<GridItem> GetServicesAsync(string serviceName, int typeId, int page, int pageSize)
        {
            var query = _context.Set<Entity.Models.Service>().AsQueryable();

            if (!string.IsNullOrEmpty(serviceName))
            {
                query = query.Where(s => s.Name.Contains(serviceName));
            }

            query = query.Where(s => s.TypeId == typeId && s.Available);

            var includes = GetServiceIncludeInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var offset = (page - 1) * pageSize;
            var data = await query.Skip(offset).Take(pageSize).ToListAsync();

            return new GridItem
            {
                Data = data,
                TotalItems = query.Count(),
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
        }

        public async Task<Entity.Models.Service> GetServiceByIdAsync(string serviceId)
        {
            var query = _context.Set<Entity.Models.Service>().AsQueryable();

            return await query.FirstOrDefaultAsync(s => s.Id.Contains(serviceId));
        }

        public async Task<SelectList> ListEmployeeAsync(string serviceId)
        {
            return await _orderRepository.ListEmployeeAsync(serviceId);
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
            return await _orderRepository.AddServiceToOrderAsync(
             orderId,
             serviceId,
             dateComplete,
             takeTime,
             notes,
             personnelNumber,
             taxAddedValue,
             isEdit);
        }

        public async Task<int> DeleteOrderAsync(string orderId)
        {
            try
            {
                var entityToDelete = await _context.Set<OrderService>().FindAsync(orderId)
                     ?? throw new InvalidOperationException($"Entity with id {orderId} not found.");
                _context.Set<OrderService>().Remove(entityToDelete);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> DeleteAttachedServiceAsync(string orderId, string serviceId)
        {
            var usingPartMaterialExists = await _context.UsingPartMaterial.AnyAsync(upm => upm.OrderId == orderId && upm.ServiceId == serviceId);
            var usingCustomSPartMatExists = await _context.UsingCustomSpartMat.AnyAsync(ucspm => ucspm.OrderId == orderId && ucspm.ServiceId == serviceId);
            var executingServiceExists = await _context.ExecutingService.AnyAsync(es => es.OrderId == orderId && es.ServiceId == serviceId);

            if (!usingPartMaterialExists && !usingCustomSPartMatExists && executingServiceExists)
            {
                var executingService = await _context.ExecutingService.FirstOrDefaultAsync(es => es.OrderId == orderId && es.ServiceId == serviceId);
                _context.ExecutingService.Remove(executingService);
                await _context.SaveChangesAsync();

                return 0;
            }
            else
            {
                return -5;
            }

            //int returnCode = 0;
            //using (var connection = new SqlConnection(connectionString))
            //{
            //    await connection.OpenAsync();

            //    using (var command = new SqlCommand("DeleteAttachedService", connection))
            //    {
            //        command.CommandType = CommandType.StoredProcedure;
            //        command.Parameters.AddWithValue("@OrderId", orderId);
            //        command.Parameters.AddWithValue("@ServiceId", serviceId);
            //        command.CommandTimeout = 120;

            //        SqlParameter returnParameter = command.Parameters.Add("@ReturnCode", SqlDbType.Int);
            //        returnParameter.Direction = ParameterDirection.ReturnValue;
            //        await command.ExecuteNonQueryAsync();

            //        returnCode = (int)returnParameter.Value;
            //    }

            //    await connection.CloseAsync();
            //}

            //return returnCode;
        }

        public async Task<int> CloseOrderAsync(
            string orderId, DateTime? dateFactComplete, DateTime? completeDate,
            int? paymentTypeId, int? personnelNumberCloser, bool? isCompleted, string rejectionReason)
        {           
            return await _orderRepository.CloseOrderAsync(orderId, dateFactComplete, completeDate,
            paymentTypeId, personnelNumberCloser, isCompleted, rejectionReason);
        }

        private static Expression<Func<RemarkToStateCar, object>>[] GetRemarkToStateCarInfo()
        {
            return
            [
                x => x.OrderService,
                x => x.OrderService.Car,
                x => x.OrderService.Client,
            ];
        }

        public async Task<List<RemarkToStateCar>> RemarkToStateCarsAsync(string orderId)
        {
            var query = _context.Set<Entity.Models.RemarkToStateCar>().AsQueryable();

            if (!string.IsNullOrEmpty(orderId))
            {
                query = query.Where(s => s.OrderId.Contains(orderId));
            }

            var includes = GetRemarkToStateCarInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<List<UsingCustomSpartMat>> GetUsingCustomSpartMatAsync(string orderId)
        {
            var query = _context.Set<Entity.Models.UsingCustomSpartMat>().AsQueryable();

            if (!string.IsNullOrEmpty(orderId))
            {
                query = query.Where(s => s.OrderId.Contains(orderId));
            }

            return await query.ToListAsync();
        }

        public async Task<List<AllInfoAttachedPart>> GetAllInfoAttachedPartByIdAsync(string orderId)
        {
            return await _orderRepository.GetAllInfoAttachedPartByIdAsync(orderId);
        }

        public async Task<List<TotalInfoServicesByOrder>> GetTotalInfoServicesByOrderAsync(string orderId)
        {
            return await _orderRepository.GetTotalInfoServicesByOrderAsync(orderId);
        }

        private static Expression<Func<Entity.Models.OrderService, object>>[] GetOrderServiceIncludeInfo()
        {
            return
            [
                x => x.Client,
                x => x.TypeOfPayment,
                x => x.Car,
                x => x.Car.CarModification,
                x => x.Car.CarModification.Series,
                x => x.Car.CarModification.Series,
                x => x.Car.CarModification.Series.CarModel,
                x => x.Car.CarModification.Series.CarModel.CarBrand,
            ];
        }

        public async Task<GridItem> SearchOrdersAsync(
            string clientId, string fullName, string orderId,
            DateTime? dateFrom, DateTime? dateTo, bool isClosed, bool isReject,
            int page, int pageSize,
            string fieldNameSort = "FullName",
            bool isAscend = true)
        {
            var queryOrder = _context.Set<Entity.Models.OrderService>().AsQueryable();

            var includes = GetOrderServiceIncludeInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    queryOrder = queryOrder.Include(include);
                }
            }

            var query = from orderService in queryOrder
                        join client in _context.Set<Client>() on orderService.ClientId equals client.ClientId
                        join car in _context.Set<Car>() on orderService.VinNumber equals car.VinNumber
                        where (string.IsNullOrEmpty(clientId) || orderService.ClientId.Contains(clientId))
                           && (string.IsNullOrEmpty(fullName) || client.FullName.Contains(fullName))
                           && (string.IsNullOrEmpty(orderId) || orderService.OrderId.Contains(orderId))
                           && ((dateFrom == null || orderService.DateMakingOrder >= dateFrom) && (dateTo == null || orderService.DateMakingOrder <= dateTo))
                           && ((isClosed && orderService.DateFactCompleting != null) || (!isClosed && orderService.DateFactCompleting == null))
                           && ((isReject && orderService.RejectionReason != null) || (!isReject && orderService.RejectionReason == null))

                        select new OrderListItem
                        {
                            OrderId = orderService.OrderId,
                            DateMakingOrder = orderService.DateMakingOrder,
                            DateFactCompleting = orderService.DateFactCompleting,
                            FullName = client.FullName,
                            Model = orderService.Model,
                            RegistrationNumber = car.RegistrationNumber,
                        };

            if (!string.IsNullOrWhiteSpace(fieldNameSort))
            {
                if (isAscend == true)
                {
                    query = fieldNameSort switch
                    {
                        "FullName" => query.OrderBy(s => s.FullName),
                        "DateMakingOrder" => query.OrderBy(s => s.DateMakingOrder),
                        "Model" => query.OrderBy(s => s.Model),
                        _ => query
                    };
                }
                else
                {
                    query = fieldNameSort switch
                    {
                        "FullName" => query.OrderByDescending(s => s.FullName),
                        "DateMakingOrder" => query.OrderByDescending(s => s.DateMakingOrder),
                        "Model" => query.OrderByDescending(s => s.Model),
                        _ => query
                    };
                }
            }

            var offset = (page - 1) * pageSize;
            var data = await query.Skip(offset).Take(pageSize).ToListAsync();

            return new GridItem
            {
                Data = data,
                TotalItems = await query.CountAsync(),
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
        }

        public async Task<IEnumerable<string>> GetOrderServiceIdsAsync(string term, string clientId, int takeItems)
        {
            var queryOrderIds = _context.Set<Entity.Models.OrderService>()
                .Where(a => a.ClientId.Contains(clientId) && a.OrderId.Contains(term))
                .Select(a => a.OrderId)
                .Distinct()
                .Take(takeItems);

            return await queryOrderIds.ToListAsync();
        }

        public async Task<IEnumerable<Entity.Models.OrderService>> GetOrderServicesByClientIdAsync(string clientId)
        {
            var queryOrder = _context.Set<Entity.Models.OrderService>().AsQueryable();

            var includes = GetOrderServiceIncludeInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    queryOrder = queryOrder.Include(include);
                }
            }

            var query = queryOrder.Where(a => a.ClientId.Contains(clientId));

            return await query.ToListAsync();
        }

        public async Task<(int returnValue, string orderId, string errorMessage)> AddPartOrderAsync(
            DateTime? dateMaking, string clientId, string vinNumber, string description,
                int? currentMill, int employeeId, byte numberWheelCaps, byte numberWipers, byte numberWipersArms,
                bool isAntenna, bool isSpareWheel, bool isCoverDecorEngine, bool isTuner, byte? fluelLevelPercent)
        {
            return await _orderRepository.AddPartOrderAsync(
                dateMaking, clientId, vinNumber, description,
                currentMill, employeeId, numberWheelCaps, numberWipers, numberWipersArms,
                isAntenna, isSpareWheel, isCoverDecorEngine, isTuner, fluelLevelPercent);
        }

        public async Task<(int returnValue, string orderId, string errorMessage)> AddPartialOrderForRecordAsync(DateTime? dateMaking,
            string clientId, string vinNumber, string description, int? currentMill, int employeeId, long? recordId, 
            byte numberWheelCaps, byte numberWipers, byte numberWipersArms, bool isAntenna, bool isSpareWheel, 
            bool isCoverDecorEngine, bool isTuner, byte? fluelLevelPercent)
        {
            return await _orderRepository.AddPartialOrderForRecordAsync(dateMaking, clientId, vinNumber, description,
                currentMill, employeeId, recordId, numberWheelCaps, numberWipers, numberWipersArms,
                isAntenna, isSpareWheel, isCoverDecorEngine, isTuner, fluelLevelPercent);
        }

        public async Task AddRemarkToStateCarAsync(RemarkToStateCar entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _context.Set<RemarkToStateCar>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ExecutingService>> GetExecutingServiceByOrderIdAsync(string orderId)
        {
            var queryOrder = _context.Set<Entity.Models.ExecutingService>().AsQueryable();

            var query = queryOrder.Where(s => s.OrderId.Contains(orderId));

            return await query.ToListAsync();
        }

        private static Expression<Func<Entity.Models.ExecutingService, object>>[] GeExecutingServiceIncludeInfo()
        {
            return
            [
                x => x.Employee,
                x => x.Service,
                x => x.OrderService,
                x => x.Employee.Post,
            ];
        }

        public async Task<ExecutingService> GetExecutingServiceAsync(string orderId, string serviceId)
        {
            var queryOrder = _context.Set<Entity.Models.ExecutingService>().AsQueryable();

            var includes = GeExecutingServiceIncludeInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    queryOrder = queryOrder.Include(include);
                }
            }

            var query = await queryOrder.FirstOrDefaultAsync(s => s.OrderId.Contains(orderId) && s.ServiceId.Contains(serviceId));

            return query;
        }

        private static Expression<Func<Entity.Models.AcceptanceDocument, object>>[] GeAcceptanceDocumentIncludeInfo()
        {
            return
            [
                x => x.Employee,
                x => x.Client,
                x => x.AcceptanceCustomSparts,
                x => x.Employee.Post,
            ];
        }

        public async Task<AcceptanceDocument> GetAcceptDocumentByIdAsync(int acceptanceId)
        {
            var queryOrder = _context.Set<Entity.Models.AcceptanceDocument>().AsQueryable();

            var includes = GeAcceptanceDocumentIncludeInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    queryOrder = queryOrder.Include(include);
                }
            }

            var query = await queryOrder.FirstOrDefaultAsync(s => s.AcceptanceId == acceptanceId);

            return query;
        }

        public async Task<IEnumerable<ReportAcceptedCustomParts>> GetReportAcceptedCustomParts(string orderId, string clientId)
        {
            return await _orderRepository.GetReportAcceptedCustomParts(orderId, clientId);
        }
    }
}