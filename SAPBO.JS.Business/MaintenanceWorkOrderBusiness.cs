using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class MaintenanceWorkOrderBusiness : SapB1GenericRepository<MaintenanceWorkOrder>, IMaintenanceWorkOrderBusiness
    {
        private readonly IMaintenancePriorityBusiness _maintenancePriorityRepository;
        private readonly IMaintenanceTypeBusiness _maintenanceTypeRepository;
        private readonly IProductionMachineBusiness _productionMachineRepository;
        private readonly IProductionMachineZoneBusiness _productionMachineZoneRepository;
        private readonly ICostCenterBusiness _costCenterRepository;
        private readonly IEmployeeBusiness _employeeRepository;
        private readonly IMaintenanceWorkOrderToolBusiness _maintenanceWorkOrderToolRepository;
        private readonly IMaintenanceWorkOrderReplacementBusiness _maintenanceWorkOrderReplacementRepository;
        private readonly IMaintenanceWorkOrderEmployeeBusiness _maintenanceWorkOrderEmployeeRepository;
        private readonly IMachineFailureBusiness _machineFailureRepository;

        private const string _tableName = TableNames.MaintenanceWorkOrder;

        public MaintenanceWorkOrderBusiness(SapB1Context context, ISapB1AutoMapper<MaintenanceWorkOrder> mapper, IMaintenancePriorityBusiness maintenancePriorityRepository, IMaintenanceTypeBusiness maintenanceTypeRepository, IProductionMachineBusiness productionMachineRepository, IProductionMachineZoneBusiness productionMachineZoneRepository, IEmployeeBusiness employeeRepository, IMaintenanceWorkOrderToolBusiness maintenanceWorkOrderToolRepository, IMaintenanceWorkOrderReplacementBusiness maintenanceWorkOrderReplacementRepository, IMaintenanceWorkOrderEmployeeBusiness maintenanceWorkOrderEmployeeRepository, IMachineFailureBusiness machineFailureRepository, ICostCenterBusiness costCenterRepository) : base(context, mapper, true)
        {
            _maintenancePriorityRepository = maintenancePriorityRepository;
            _maintenanceTypeRepository = maintenanceTypeRepository;
            _productionMachineRepository = productionMachineRepository;
            _productionMachineZoneRepository = productionMachineZoneRepository;
            _costCenterRepository = costCenterRepository;
            _employeeRepository = employeeRepository;
            _maintenanceWorkOrderToolRepository = maintenanceWorkOrderToolRepository;
            _maintenanceWorkOrderReplacementRepository = maintenanceWorkOrderReplacementRepository;
            _maintenanceWorkOrderEmployeeRepository = maintenanceWorkOrderEmployeeRepository;
            _machineFailureRepository = machineFailureRepository;
        }

        public async Task<ICollection<MaintenanceWorkOrder>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_231", new List<dynamic> { year, month }), objectType);
        }

        public async Task<MaintenanceWorkOrder> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_102", new List<dynamic> { id }), objectType);
        }

        public async Task CreateAsync(MaintenanceWorkOrder obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Planificado;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            await CreateAsync(_tableName, obj, obj.Id.ToString());

            //Tools
            if (obj.Tools != null && obj.Tools.Any())
                foreach (var tool in obj.Tools)
                {
                    tool.MaintenanceWorkOrderId = obj.Id;
                    await _maintenanceWorkOrderToolRepository.CreateAsync(tool);
                }
            //Replacements
            if (obj.Replacements != null && obj.Replacements.Any())
                foreach (var replacement in obj.Replacements)
                {
                    replacement.MaintenanceWorkOrderId = obj.Id;
                    await _maintenanceWorkOrderReplacementRepository.CreateAsync(replacement);
                }
            //Employees
            if (obj.Employees != null && obj.Employees.Any())
                foreach (var job in obj.Employees)
                {
                    job.MaintenanceWorkOrderId = obj.Id;
                    await _maintenanceWorkOrderEmployeeRepository.CreateAsync(job);
                }
            //Failures
            if (obj.Failures != null && obj.Failures.Any())
                await _machineFailureRepository.UpdateByMaintenanceWorkOrderIdAsync(obj.Failures.Select(x => x.Id).ToList(), obj.CreatedBy, obj.Id);
        }

        public async Task UpdateAsync(MaintenanceWorkOrder obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(obj, Enums.ObjectAction.Update, currentObj);

            //Set obj
            currentObj.UpdatedBy = obj.UpdatedBy;

            currentObj.Description = obj.Description;
            currentObj.Remark = obj.Remark;

            currentObj.MaintenancePriorityId = obj.MaintenancePriorityId;
            currentObj.MaintenanceTypeId = obj.MaintenanceTypeId;
            currentObj.ProductionMachineId = obj.ProductionMachineId;
            currentObj.ProductionMachineZoneId = obj.ProductionMachineZoneId;
            currentObj.CostCenterId = obj.CostCenterId;
            currentObj.EffectiveHours = obj.EffectiveHours;
            currentObj.OtmTypeId = obj.OtmTypeId;
            currentObj.EmployeeId = obj.EmployeeId;

            currentObj.StartDate = obj.StartDate;
            currentObj.FinalDate = obj.FinalDate;

            currentObj.StopPlant = obj.StopPlant;
            currentObj.StopMachine = obj.StopMachine;

            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());

            await _maintenanceWorkOrderToolRepository.DeleteByMaintenanceWorkOrderIdAsync(obj.Id);
            await _maintenanceWorkOrderReplacementRepository.DeleteByMaintenanceWorkOrderIdAsync(obj.Id);
            await _maintenanceWorkOrderEmployeeRepository.DeleteByMaintenanceWorkOrderIdAsync(obj.Id);
            await _machineFailureRepository.DeleteByMaintenanceWorkOrderIdAsync(obj.Id, obj.UpdatedBy);

            //Tools
            if (obj.Tools != null && obj.Tools.Any())
                foreach (var tool in obj.Tools)
                {
                    tool.MaintenanceWorkOrderId = obj.Id;
                    await _maintenanceWorkOrderToolRepository.CreateAsync(tool);
                }
            //Replacements
            if (obj.Replacements != null && obj.Replacements.Any())
                foreach (var replacement in obj.Replacements)
                {
                    replacement.MaintenanceWorkOrderId = obj.Id;
                    await _maintenanceWorkOrderReplacementRepository.CreateAsync(replacement);
                }
            //Tools
            if (obj.Employees != null && obj.Employees.Any())
                foreach (var employee in obj.Employees)
                {
                    employee.MaintenanceWorkOrderId = obj.Id;
                    await _maintenanceWorkOrderEmployeeRepository.CreateAsync(employee);
                }
            //Failures
            if (obj.Failures != null && obj.Failures.Any())
                await _machineFailureRepository.UpdateByMaintenanceWorkOrderIdAsync(obj.Failures.Select(x => x.Id).ToList(), obj.UpdatedBy, obj.Id);
        }

        public async Task DeleteAsync(int id, string deleteBy)
        {
            var obj = await GetAsync(id);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            obj.DeletedBy = deleteBy;

            CheckRules(obj, Enums.ObjectAction.Delete);

            obj.StatusId = (int)Enums.StatusType.Anulado;
            obj.DeletedAt = DateTime.Now;

            await SoftDeleteByIdAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task InitAsync(int id, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);
            currentObj.UpdatedBy = updatedBy;

            CheckRules(currentObj, Enums.ObjectAction.Init);

            //Set obj
            currentObj.StatusId = (int)Enums.StatusType.Iniciado;
            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        public async Task EndAsync(int id, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);
            currentObj.UpdatedBy = updatedBy;

            CheckRules(currentObj, Enums.ObjectAction.End);

            //Set obj
            currentObj.StatusId = (int)Enums.StatusType.Terminado;
            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        private async void CheckRules(MaintenanceWorkOrder obj, Enums.ObjectAction objectAction, MaintenanceWorkOrder currentObj = null)
        {
            switch (objectAction)
            {
                case Enums.ObjectAction.Insert:
                    //Check User - Create
                    if (string.IsNullOrEmpty(obj.CreatedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                case Enums.ObjectAction.Update:
                    //Check Status
                    if (currentObj?.StatusType != Enums.StatusType.Planificado &&
                        currentObj?.StatusType != Enums.StatusType.Iniciado)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Update
                    if (string.IsNullOrEmpty(obj.UpdatedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                case Enums.ObjectAction.Delete:
                    //Check Status
                    if (obj.StatusType != Enums.StatusType.Planificado)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Delete
                    if (string.IsNullOrEmpty(obj.DeletedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                case Enums.ObjectAction.Init:
                    //Check Status
                    if (currentObj?.StatusType != Enums.StatusType.Planificado)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Update
                    if (string.IsNullOrEmpty(obj.UpdatedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                case Enums.ObjectAction.End:
                    //Check Status
                    if (currentObj?.StatusType != Enums.StatusType.Iniciado)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Update
                    if (string.IsNullOrEmpty(obj.UpdatedBy))
                        throw new Exception(AppMessages.UserError);

                    //Check FinalDate
                    if (!obj.FinalDate.HasValue)
                        throw new Exception(AppMessages.MaintenanceWorkOrder_FinalDate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectAction), objectAction, null);
            }

            if (objectAction == Enums.ObjectAction.Insert || objectAction == Enums.ObjectAction.Update)
            {
                //Check FinalDate
                if (obj.StatusType == Enums.StatusType.Terminado && !obj.FinalDate.HasValue)
                    throw new Exception(AppMessages.MaintenanceWorkOrder_FinalDate);

                //Check FinalDate < StartDate
                if (obj.FinalDate.HasValue)
                    if (obj.FinalDate.Value < obj.StartDate)
                        throw new Exception(AppMessages.MaintenanceWorkOrder_FinalDate_Lessthan);

                //Check Employees
                if (obj.Employees == null || obj.Employees.Count.Equals(0))
                    throw new Exception(AppMessages.MaintenanceWorkOrder_Employees);

                //Check Failures
                var maintenanceType = await _maintenanceTypeRepository.GetAsync(obj.MaintenanceTypeId);
                if (maintenanceType.RequireFailure && (obj.Failures == null || !obj.Failures.Any()))
                    throw new Exception(AppMessages.MaintenanceWorkOrder_MaintenanceType);
            }
        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_101", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<MaintenanceWorkOrder> SetFullProperties(MaintenanceWorkOrder obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.MaintenancePriority = await _maintenancePriorityRepository.GetAsync(obj.MaintenancePriorityId);
                obj.MaintenanceType = await _maintenanceTypeRepository.GetAsync(obj.MaintenanceTypeId);
                obj.Employee = await _employeeRepository.GetAsync(obj.EmployeeId, objectType);

                if (obj.ProductionMachineId.HasValue)
                    obj.ProductionMachine = await _productionMachineRepository.GetAsync(obj.ProductionMachineId.Value);

                if (obj.ProductionMachineZoneId.HasValue)
                    obj.ProductionMachineZone = await _productionMachineZoneRepository.GetAsync(obj.ProductionMachineZoneId.Value);

                if (!string.IsNullOrEmpty(obj.CostCenterId))
                    obj.CostCenter = await _costCenterRepository.GetAsync(obj.CostCenterId);

                if (objectType == Enums.ObjectType.Full)
                {
                    obj.Tools = await _maintenanceWorkOrderToolRepository.GetAllAsync(obj.Id, objectType);
                    obj.Replacements = await _maintenanceWorkOrderReplacementRepository.GetAllAsync(obj.Id, objectType);
                    obj.Employees = await _maintenanceWorkOrderEmployeeRepository.GetAllAsync(obj.Id, objectType);
                    obj.Failures = await _machineFailureRepository.GetAllByMaintenanceWorkOrderIdAsync(obj.Id, objectType);
                }
            }

            return obj;
        }

        public async Task<ICollection<MaintenanceWorkOrder>> SetFullProperties(ICollection<MaintenanceWorkOrder> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                //MaintenancePriority
                var maintenancePriorityIds = objs.GroupBy(x => x.MaintenancePriorityId).Select(g => g.Key);
                var maintenancePriorities = await _maintenancePriorityRepository.GetAllWithIdsAsync(maintenancePriorityIds);

                foreach (var maintenancePriority in maintenancePriorities)
                    objs.Where(x => x.MaintenancePriorityId.Equals(maintenancePriority.Id)).ToList().ForEach(x => x.MaintenancePriority = maintenancePriority);

                //MaintenanceType
                var maintenanceTypeIds = objs.GroupBy(x => x.MaintenanceTypeId).Select(g => g.Key);
                var maintenanceTypes = await _maintenanceTypeRepository.GetAllWithIdsAsync(maintenanceTypeIds);

                foreach (var maintenanceType in maintenanceTypes)
                    objs.Where(x => x.MaintenanceTypeId.Equals(maintenanceType.Id)).ToList().ForEach(x => x.MaintenanceType = maintenanceType);

                //ProductionMachine
                var productionMachineIds = objs.Where(y => y.ProductionMachineId.HasValue).GroupBy(x => x.ProductionMachineId.Value).Select(g => g.Key);
                var productionMachines = await _productionMachineRepository.GetAllWithIdsAsync(productionMachineIds);

                foreach (var productionMachine in productionMachines)
                    objs.Where(x => x.ProductionMachineId.Equals(productionMachine.Id)).ToList().ForEach(x => x.ProductionMachine = productionMachine);

                //ProductionMachineZone
                var productionMachineZoneIds = objs.Where(y => y.ProductionMachineZoneId.HasValue).GroupBy(x => x.ProductionMachineZoneId.Value).Select(g => g.Key);
                var productionMachineZones = await _productionMachineZoneRepository.GetAllWithIdsAsync(productionMachineZoneIds);

                foreach (var productionMachineZone in productionMachineZones)
                    objs.Where(x => x.ProductionMachineZoneId.Equals(productionMachineZone.Id)).ToList().ForEach(x => x.ProductionMachineZone = productionMachineZone);

                //CostCenter
                var costCenterIds = objs.Where(y => !string.IsNullOrEmpty(y.CostCenterId)).GroupBy(x => x.CostCenterId).Select(g => g.Key);
                var costCenters = await _costCenterRepository.GetAllWithIdsAsync(costCenterIds);

                foreach (var costCenter in costCenters)
                    objs.Where(x => x.CostCenterId.Equals(costCenter.Id)).ToList().ForEach(x => x.CostCenter = costCenter);

                //SuperEmployee
                var employeeIds = objs.GroupBy(x => x.EmployeeId).Select(g => g.Key);
                var superEmployees = await _employeeRepository.GetAllWithIdsAsync(employeeIds, objectType);

                foreach (var employee in superEmployees)
                    objs.Where(x => x.EmployeeId.Equals(employee.Id)).ToList().ForEach(x => x.Employee = employee);

                if (objectType == Enums.ObjectType.Full)
                {
                    //Details
                    var maintenanceWorkOrderIds = objs.Select(x => x.Id);

                    var tools = await _maintenanceWorkOrderToolRepository.GetAllWithIdsAsync(maintenanceWorkOrderIds, objectType);
                    var replacements = await _maintenanceWorkOrderReplacementRepository.GetAllWithIdsAsync(maintenanceWorkOrderIds, objectType);
                    var employees = await _maintenanceWorkOrderEmployeeRepository.GetAllWithIdsAsync(maintenanceWorkOrderIds, objectType);
                    var failures = await _machineFailureRepository.GetAllWithIdsAsync(maintenanceWorkOrderIds, objectType);

                    foreach (var maintenanceWorkOrder in objs)
                    {
                        maintenanceWorkOrder.Tools = tools.Where(x => x.MaintenanceWorkOrderId.Equals(maintenanceWorkOrder.Id)).ToList();
                        maintenanceWorkOrder.Replacements = replacements.Where(x => x.MaintenanceWorkOrderId.Equals(maintenanceWorkOrder.Id)).ToList();
                        maintenanceWorkOrder.Employees = employees.Where(x => x.MaintenanceWorkOrderId.Equals(maintenanceWorkOrder.Id)).ToList();
                        maintenanceWorkOrder.Failures = failures.Where(x => x.MaintenanceWorkOrderId.Equals(maintenanceWorkOrder.Id)).ToList();
                    }
                }
            }

            return objs;
        }
    }
}
