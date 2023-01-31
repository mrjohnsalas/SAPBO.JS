using Microsoft.Extensions.Caching.Memory;
using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductGrammageBusiness : SapB1GenericRepository<ProductGrammage>, IProductGrammageBusiness
    {
        private const string _tableName = TableNames.ProductGrammage;
        private const string _cacheName = "ProductGrammages";

        private readonly IMemoryCache _memoryCache;

        public ProductGrammageBusiness(SapB1Context context, ISapB1AutoMapper<ProductGrammage> mapper, IMemoryCache memoryCache) : base(context, mapper, true)
        {
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<ProductGrammage>> GetCache()
        {
            ICollection<ProductGrammage> objs = null;

            if (!_memoryCache.TryGetValue(_cacheName, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_195");
                _memoryCache.Set(_cacheName, objs);
            }

            return objs;
        }

        public async Task<ICollection<ProductGrammage>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            var objs = await GetCache();

            if (statusType != Enums.StatusType.Todos)
                objs = objs.Where(x => x.StatusType == statusType).ToList();

            return objs;

            //return statusType == Enums.StatusType.Todos
            //    ? GetAllAsync("GP_WEB_APP_195")
            //    : GetAllAsync("GP_WEB_APP_279", new List<dynamic> { (int)statusType });
        }

        public async Task<ICollection<ProductGrammage>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            var objs = await GetCache();

            return objs.Where(x => ids.Any(y => y.Equals(x.Id))).ToList();

            //return GetAllAsync("GP_WEB_APP_426", new List<dynamic> { string.Join(",", ids) });
        }

        public Task<ICollection<ProductGrammage>> GetAllByProductMaterialTypeIdAsync(int productMaterialTypeId)
        {
            return GetAllAsync("GP_WEB_APP_223", new List<dynamic> { productMaterialTypeId });
        }

        public Task<ICollection<ProductGrammage>> GetAllByProductFormulaIdAndProductMaterialTypeIdAsync(int productFormulaId, int productMaterialTypeId)
        {
            return GetAllAsync("GP_WEB_APP_281", new List<dynamic> { productFormulaId, productMaterialTypeId });
        }

        public async Task<ProductGrammage> GetAsync(int id)
        {
            var objs = await GetCache();

            return objs.FirstOrDefault(x => x.Id.Equals(id));

            //return GetAsync("GP_WEB_APP_194", new List<dynamic> { id });
        }

        public Task CreateAsync(ProductGrammage obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            //Remove cache
            _memoryCache.Remove(_cacheName);

            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(ProductGrammage obj)
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
            currentObj.Value = obj.Value;
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

        private static void CheckRules(ProductGrammage obj, Enums.ObjectAction objectAction, ProductGrammage currentObj = null)
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
            var id = GetValue("GP_WEB_APP_193", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }
    }
}
