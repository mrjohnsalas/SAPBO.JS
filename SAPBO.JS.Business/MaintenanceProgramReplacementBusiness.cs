using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class MaintenanceProgramReplacementBusiness : SapB1GenericRepository<MaintenanceProgramReplacement>, IMaintenanceProgramReplacementBusiness
    {
        private readonly IReplacementBusiness _replacementRepository;
        private const string _tableName = TableNames.MaintenanceProgramReplacement;

        public MaintenanceProgramReplacementBusiness(SapB1Context context, ISapB1AutoMapper<MaintenanceProgramReplacement> mapper, IReplacementBusiness replacementRepository) : base(context, mapper)
        {
            _replacementRepository = replacementRepository;
        }

        public async Task<ICollection<MaintenanceProgramReplacement>> GetAllAsync(int maintenanceProgramId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_121", new List<dynamic> { maintenanceProgramId }), objectType);
        }

        public async Task<ICollection<MaintenanceProgramReplacement>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_313", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<MaintenanceProgramReplacement> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_120", new List<dynamic> { id }), objectType);
        }

        public Task CreateAsync(MaintenanceProgramReplacement obj)
        {
            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(MaintenanceProgramReplacement obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Set obj
            currentObj.ReplacementId = obj.ReplacementId;
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
            var id = GetValue("GP_WEB_APP_119", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<MaintenanceProgramReplacement> SetFullProperties(MaintenanceProgramReplacement obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
                obj.Replacement = await _replacementRepository.GetAsync(obj.ReplacementId);

            return obj;
        }

        public async Task<ICollection<MaintenanceProgramReplacement>> SetFullProperties(ICollection<MaintenanceProgramReplacement> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            var replacementIds = objs.GroupBy(x => x.ReplacementId).Select(g => g.Key);
            var replacements = await _replacementRepository.GetAllWithIdsAsync(replacementIds);

            foreach (var replacement in replacements)
                objs.Where(x => x.ReplacementId.Equals(replacement.Id)).ToList().ForEach(x => x.Replacement = replacement);

            return objs;
        }
    }
}
