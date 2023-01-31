using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class MaintenanceWorkOrderEmployeeBusiness : SapB1GenericRepository<MaintenanceWorkOrderEmployee>, IMaintenanceWorkOrderEmployeeBusiness
    {
        private readonly IEmployeeBusiness _employeeRepository;
        private const string _tableName = TableNames.MaintenanceWorkOrderEmployee;

        public MaintenanceWorkOrderEmployeeBusiness(SapB1Context context, ISapB1AutoMapper<MaintenanceWorkOrderEmployee> mapper, IEmployeeBusiness employeeRepository) : base(context, mapper)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<ICollection<MaintenanceWorkOrderEmployee>> GetAllAsync(int maintenanceWorkOrderId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_106", new List<dynamic> { maintenanceWorkOrderId }), objectType);
        }

        public async Task<ICollection<MaintenanceWorkOrderEmployee>> GetAllWithIdsAsync(IEnumerable<int> maintenanceWorkOrderIds, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_317", new List<dynamic> { string.Join(",", maintenanceWorkOrderIds) }), objectType);
        }

        public async Task<MaintenanceWorkOrderEmployee> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_105", new List<dynamic> { id }), objectType);
        }

        public Task CreateAsync(MaintenanceWorkOrderEmployee obj)
        {
            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(MaintenanceWorkOrderEmployee obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Set obj
            currentObj.EmployeeId = obj.EmployeeId;
            currentObj.Task = obj.Task;
            currentObj.EstimatedTime = obj.EstimatedTime;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        public async Task DeleteAsync(int id)
        {
            //Get obj
            var obj = await GetAsync(id);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            await DeleteByIdAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task DeleteByMaintenanceWorkOrderIdAsync(int maintenanceWorkOrderId)
        {
            var objs = await GetAllAsync(maintenanceWorkOrderId);
            if (objs != null && objs.Any())
                foreach (var obj in objs)
                    await DeleteAsync(obj.Id);

        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_104", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<MaintenanceWorkOrderEmployee> SetFullProperties(MaintenanceWorkOrderEmployee obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
                obj.Employee = await _employeeRepository.GetAsync(obj.EmployeeId);

            return obj;
        }

        public async Task<ICollection<MaintenanceWorkOrderEmployee>> SetFullProperties(ICollection<MaintenanceWorkOrderEmployee> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            var employeeids = objs.GroupBy(x => x.EmployeeId).Select(g => g.Key);
            var employees = await _employeeRepository.GetAllWithIdsAsync(employeeids);

            foreach (var employee in employees)
                objs.Where(x => x.EmployeeId.Equals(employee.Id)).ToList().ForEach(x => x.Employee = employee);

            return objs;
        }
    }
}
