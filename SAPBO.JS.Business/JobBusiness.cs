using Microsoft.Extensions.Caching.Memory;
using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    /// <summary>
    /// Info:
    /// Status: Activo, Anulado
    ///     New => Activo
    ///     Delete => Anulado
    /// </summary>
    public class JobBusiness : SapB1GenericRepository<Job>, IJobBusiness
    {
        private const string _tableName = TableNames.Job;
        private const string _cacheName = "Jobs";

        private readonly IBusinessUnitBusiness _businessUnitRepository;
        private readonly IMemoryCache _memoryCache;

        public JobBusiness(SapB1Context context, ISapB1AutoMapper<Job> mapper, IBusinessUnitBusiness businessUnitRepository, IMemoryCache memoryCache) : base(context, mapper, true)
        {
            _businessUnitRepository = businessUnitRepository;
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<Job>> GetCache()
        {
            ICollection<Job> objs = null;

            if (!_memoryCache.TryGetValue(_cacheName, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_341", new List<dynamic> { "" });
                _memoryCache.Set(_cacheName, objs);
            }

            return objs;
        }

        public async Task<ICollection<Job>> GetAllAsync(string businessUnitId = "", Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            var objs = await GetCache();

            if (statusType != Enums.StatusType.Todos)
                objs = objs.Where(x => x.StatusType == statusType).ToList();

            if (businessUnitId != "")
                objs = objs.Where(x => x.BusinessUnitId == businessUnitId).ToList();

            return await SetFullProperties(objs, objectType);

            //return await SetFullProperties(statusType == Enums.StatusType.Todos
            //    ? await GetAllAsync("GP_WEB_APP_341", new List<dynamic> { businessUnitId })
            //    : await GetAllAsync("GP_WEB_APP_346", new List<dynamic> { (int)statusType, businessUnitId }), objectType);
        }

        public async Task<ICollection<Job>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            var objs = await GetCache();

            return await SetFullProperties(objs.Where(x => ids.Any(y => y.Equals(x.Id))).ToList(), objectType);

            //return await SetFullProperties(await GetAllAsync("GP_WEB_APP_348", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<Job> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            var objs = await GetCache();

            return await SetFullProperties(objs.FirstOrDefault(x => x.Id.Equals(id)), objectType);

            //return await SetFullProperties(await GetAsync("GP_WEB_APP_340", new List<dynamic> { id }), objectType);
        }

        public Task CreateAsync(Job obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            //Remove cache
            _memoryCache.Remove(_cacheName);

            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(Job obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(obj, Enums.ObjectAction.Update, currentObj);

            //Set obj
            currentObj.UpdatedBy = obj.UpdatedBy;

            currentObj.Name = obj.Name;
            currentObj.Description = obj.Description;
            currentObj.BusinessUnitId = obj.BusinessUnitId;
            currentObj.CostHour = obj.CostHour;
            currentObj.UpdatedAt = DateTime.Now;

            //Remove cache
            _memoryCache.Remove(_cacheName);

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
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

            //Remove cache
            _memoryCache.Remove(_cacheName);

            await SoftDeleteByIdAsync(_tableName, obj, obj.Id.ToString());
        }

        private static void CheckRules(Job obj, Enums.ObjectAction objectAction, Job currentObj = null)
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
                    if (currentObj?.StatusType != Enums.StatusType.Activo)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Update
                    if (string.IsNullOrEmpty(obj.UpdatedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                case Enums.ObjectAction.Delete:
                    //Check Status
                    if (obj.StatusType != Enums.StatusType.Activo)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Delete
                    if (string.IsNullOrEmpty(obj.DeletedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectAction), objectAction, null);
            }
        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_090", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<Job> SetFullProperties(Job obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.BusinessUnit = await _businessUnitRepository.GetAsync(obj.BusinessUnitId);
            }

            return obj;
        }

        public async Task<ICollection<Job>> SetFullProperties(ICollection<Job> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                var buIds = objs.GroupBy(x => x.BusinessUnitId).Select(g => g.Key);
                var bus = await _businessUnitRepository.GetAllWithIdsAsync(buIds);

                foreach (var bu in bus)
                    objs.Where(x => x.BusinessUnitId.Equals(bu.Id)).ToList().ForEach(x => x.BusinessUnit = bu);
            }

            return objs;
        }
    }
}
