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
    public class FailureTypeBusiness : SapB1GenericRepository<FailureType>, IFailureTypeBusiness
    {
        private const string _tableName = TableNames.FailureType;
        private const string _cacheName = "FailureTypes";

        private readonly IMemoryCache _memoryCache;

        public FailureTypeBusiness(SapB1Context context, ISapB1AutoMapper<FailureType> mapper, IMemoryCache memoryCache) : base(context, mapper, true)
        {
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<FailureType>> GetCache()
        {
            ICollection<FailureType> objs = null;

            if (!_memoryCache.TryGetValue(_cacheName, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_071");
                _memoryCache.Set(_cacheName, objs);
            }

            return objs;
        }

        public async Task<ICollection<FailureType>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            var objs = await GetCache();

            if (statusType != Enums.StatusType.Todos)
                objs = objs.Where(x => x.StatusType == statusType).ToList();

            return objs;

            //return statusType == Enums.StatusType.Todos
            //    ? GetAllAsync("GP_WEB_APP_071")
            //    : GetAllAsync("GP_WEB_APP_240", new List<dynamic> { (int)statusType });
        }

        public async Task<ICollection<FailureType>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            var objs = await GetCache();

            return objs.Where(x => ids.Any(y => y.Equals(x.Id))).ToList();

            //return GetAllAsync("GP_WEB_APP_305", new List<dynamic> { string.Join(",", ids) });
        }

        public async Task<FailureType> GetAsync(int id)
        {
            var objs = await GetCache();

            return objs.FirstOrDefault(x => x.Id.Equals(id));

            //return GetAsync("GP_WEB_APP_070", new List<dynamic> { id });
        }

        public Task CreateAsync(FailureType obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            //Remove cache
            _memoryCache.Remove(_cacheName);

            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(FailureType obj)
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

        private static void CheckRules(FailureType obj, Enums.ObjectAction objectAction, FailureType currentObj = null)
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
            var id = GetValue("GP_WEB_APP_069", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }
    }
}
