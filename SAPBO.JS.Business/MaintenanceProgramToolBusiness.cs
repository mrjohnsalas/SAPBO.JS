using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class MaintenanceProgramToolBusiness : SapB1GenericRepository<MaintenanceProgramTool>, IMaintenanceProgramToolBusiness
    {
        private readonly IMaintenanceToolBusiness _maintenanceToolRepository;
        private const string _tableName = TableNames.MaintenanceProgramTool;

        public MaintenanceProgramToolBusiness(SapB1Context context, ISapB1AutoMapper<MaintenanceProgramTool> mapper, IMaintenanceToolBusiness maintenanceToolRepository) : base(context, mapper)
        {
            _maintenanceToolRepository = maintenanceToolRepository;
        }

        public async Task<ICollection<MaintenanceProgramTool>> GetAllAsync(int maintenanceProgramId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_118", new List<dynamic> { maintenanceProgramId }), objectType);
        }

        public async Task<ICollection<MaintenanceProgramTool>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_312", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<MaintenanceProgramTool> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_117", new List<dynamic> { id }), objectType);
        }

        public Task CreateAsync(MaintenanceProgramTool obj)
        {
            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(MaintenanceProgramTool obj)
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

        public async Task DeleteByMaintenanceProgramIdAsync(int maintenanceProgramId)
        {
            var objs = await GetAllAsync(maintenanceProgramId);
            if (objs != null && objs.Any())
                foreach (var obj in objs)
                    await DeleteAsync(obj.Id);

        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_116", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<MaintenanceProgramTool> SetFullProperties(MaintenanceProgramTool obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
                obj.MaintenanceTool = await _maintenanceToolRepository.GetAsync(obj.MaintenanceToolId);

            return obj;
        }

        public async Task<ICollection<MaintenanceProgramTool>> SetFullProperties(ICollection<MaintenanceProgramTool> objs, Enums.ObjectType objectType)
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
