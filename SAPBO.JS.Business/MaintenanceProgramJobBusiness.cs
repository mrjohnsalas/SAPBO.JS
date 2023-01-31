using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class MaintenanceProgramJobBusiness : SapB1GenericRepository<MaintenanceProgramJob>, IMaintenanceProgramJobBusiness
    {
        private readonly IJobBusiness _jobRepository;
        private const string _tableName = TableNames.MaintenanceProgramJob;

        public MaintenanceProgramJobBusiness(SapB1Context context, ISapB1AutoMapper<MaintenanceProgramJob> mapper, IJobBusiness jobRepository) : base(context, mapper)
        {
            _jobRepository = jobRepository;
        }

        public async Task<ICollection<MaintenanceProgramJob>> GetAllAsync(int maintenanceProgramId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_124", new List<dynamic> { maintenanceProgramId }), objectType);
        }

        public async Task<ICollection<MaintenanceProgramJob>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_314", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<MaintenanceProgramJob> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_123", new List<dynamic> { id }), objectType);
        }

        public Task CreateAsync(MaintenanceProgramJob obj)
        {
            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(MaintenanceProgramJob obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Set obj
            currentObj.JobId = obj.JobId;
            currentObj.EstimatedTime = obj.EstimatedTime;
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
            var id = GetValue("GP_WEB_APP_122", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<MaintenanceProgramJob> SetFullProperties(MaintenanceProgramJob obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
                obj.Job = await _jobRepository.GetAsync(obj.JobId);

            return obj;
        }

        public async Task<ICollection<MaintenanceProgramJob>> SetFullProperties(ICollection<MaintenanceProgramJob> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            var ids = objs.GroupBy(x => x.JobId).Select(g => g.Key);
            var jobs = await _jobRepository.GetAllWithIdsAsync(ids);

            foreach (var job in jobs)
                objs.Where(x => x.JobId.Equals(job.Id)).ToList().ForEach(x => x.Job = job);

            return objs;
        }
    }
}
