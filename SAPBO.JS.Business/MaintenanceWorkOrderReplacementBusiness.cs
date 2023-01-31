using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class MaintenanceWorkOrderReplacementBusiness : SapB1GenericRepository<MaintenanceWorkOrderReplacement>, IMaintenanceWorkOrderReplacementBusiness
    {
        private readonly IReplacementBusiness _replacementRepository;
        private readonly ITimeFrequencyBusiness _timeFrequencyRepository;
        private const string _tableName = TableNames.MaintenanceWorkOrderReplacement;

        public MaintenanceWorkOrderReplacementBusiness(SapB1Context context, ISapB1AutoMapper<MaintenanceWorkOrderReplacement> mapper, IReplacementBusiness replacementRepository, ITimeFrequencyBusiness timeFrequencyRepository) : base(context, mapper)
        {
            _replacementRepository = replacementRepository;
            _timeFrequencyRepository = timeFrequencyRepository;
        }

        public async Task<ICollection<MaintenanceWorkOrderReplacement>> GetAllAsync(int maintenanceWorkOrderId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_109", new List<dynamic> { maintenanceWorkOrderId }), objectType);
        }

        public async Task<ICollection<MaintenanceWorkOrderReplacement>> GetAllWithIdsAsync(IEnumerable<int> maintenanceWorkOrderIds, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_316", new List<dynamic> { string.Join(",", maintenanceWorkOrderIds) }), objectType);
        }

        public async Task<MaintenanceWorkOrderReplacement> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_108", new List<dynamic> { id }), objectType);
        }

        public Task CreateAsync(MaintenanceWorkOrderReplacement obj)
        {
            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(MaintenanceWorkOrderReplacement obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Set obj
            currentObj.ReplacementId = obj.ReplacementId;
            currentObj.PlannedQuantity = obj.PlannedQuantity;
            currentObj.TimeFrequencyId = obj.TimeFrequencyId;
            currentObj.TimeFrequencyValue = obj.TimeFrequencyValue;

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
                    if (obj.Id > 0) //NOT SALIDAS
                        await DeleteAsync(obj.Id);

        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_107", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<MaintenanceWorkOrderReplacement> SetFullProperties(MaintenanceWorkOrderReplacement obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.Replacement = await _replacementRepository.GetAsync(obj.ReplacementId);
                obj.TimeFrequency = await _timeFrequencyRepository.GetAsync(obj.TimeFrequencyId);
            }

            return obj;
        }

        public async Task<ICollection<MaintenanceWorkOrderReplacement>> SetFullProperties(ICollection<MaintenanceWorkOrderReplacement> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            //Replacement
            var replacementIds = objs.GroupBy(x => x.ReplacementId).Select(g => g.Key);
            var replacements = await _replacementRepository.GetAllWithIdsAsync(replacementIds);

            foreach (var replacement in replacements)
                objs.Where(x => x.ReplacementId.Equals(replacement.Id)).ToList().ForEach(x => x.Replacement = replacement);

            //TimeFrequency
            var timeFrequencyIdIds = objs.GroupBy(x => x.TimeFrequencyId).Select(g => g.Key);
            var timeFrequencies = await _timeFrequencyRepository.GetAllWithIdsAsync(timeFrequencyIdIds);

            foreach (var timeFrequency in timeFrequencies)
                objs.Where(x => x.TimeFrequencyId.Equals(timeFrequency.Id)).ToList().ForEach(x => x.TimeFrequency = timeFrequency);

            return objs;
        }
    }
}
