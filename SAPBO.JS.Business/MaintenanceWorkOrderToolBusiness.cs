using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class MaintenanceWorkOrderToolBusiness : SapB1GenericRepository<MaintenanceWorkOrderTool>, IMaintenanceWorkOrderToolBusiness
    {
        private readonly IMaintenanceToolBusiness _maintenanceToolRepository;
        private const string _tableName = TableNames.MaintenanceWorkOrderTool;

        public MaintenanceWorkOrderToolBusiness(SapB1Context context, ISapB1AutoMapper<MaintenanceWorkOrderTool> mapper, IMaintenanceToolBusiness maintenanceToolRepository) : base(context, mapper)
        {
            _maintenanceToolRepository = maintenanceToolRepository;
        }

        public async Task<ICollection<MaintenanceWorkOrderTool>> GetAllAsync(int maintenanceWorkOrderId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_112", new List<dynamic> { maintenanceWorkOrderId }), objectType);
        }

        public async Task<ICollection<MaintenanceWorkOrderTool>> GetAllWithIdsAsync(IEnumerable<int> maintenanceWorkOrderIds, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_315", new List<dynamic> { string.Join(",", maintenanceWorkOrderIds) }), objectType);
        }

        public async Task<MaintenanceWorkOrderTool> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_111", new List<dynamic> { id }), objectType);
        }

        public Task CreateAsync(MaintenanceWorkOrderTool obj)
        {
            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(MaintenanceWorkOrderTool obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Set obj
            currentObj.MaintenanceToolId = obj.MaintenanceToolId;
            currentObj.Quantity = obj.Quantity;

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
            var id = GetValue("GP_WEB_APP_110", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<MaintenanceWorkOrderTool> SetFullProperties(MaintenanceWorkOrderTool obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
                obj.MaintenanceTool = await _maintenanceToolRepository.GetAsync(obj.MaintenanceToolId);

            return obj;
        }

        public async Task<ICollection<MaintenanceWorkOrderTool>> SetFullProperties(ICollection<MaintenanceWorkOrderTool> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            var maintenanceToolIds = objs.GroupBy(x => x.MaintenanceToolId).Select(g => g.Key);
            var maintenanceTools = await _maintenanceToolRepository.GetAllWithIdsAsync(maintenanceToolIds);

            foreach (var maintenanceTool in maintenanceTools)
                objs.Where(x => x.MaintenanceToolId.Equals(maintenanceTool.Id)).ToList().ForEach(x => x.MaintenanceTool = maintenanceTool);

            return objs;
        }
    }
}
