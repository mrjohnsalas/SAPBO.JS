using Microsoft.Extensions.Caching.Memory;
using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductFormatBusiness : SapB1GenericRepository<ProductFormat>, IProductFormatBusiness
    {
        private const string _tableName = TableNames.ProductFormat;
        private const string _cacheName = "ProductFormats";
        private readonly IUnitOfMeasurementBusiness _unitOfMeasurementRepository;

        private readonly IMemoryCache _memoryCache;

        public ProductFormatBusiness(SapB1Context context, ISapB1AutoMapper<ProductFormat> mapper, IMemoryCache memoryCache, IUnitOfMeasurementBusiness unitOfMeasurementRepository) : base(context, mapper, true)
        {
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<ProductFormat>> GetCache()
        {
            ICollection<ProductFormat> objs = null;

            if (!_memoryCache.TryGetValue(_cacheName, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_171");
                _memoryCache.Set(_cacheName, objs);
            }

            return objs;
        }

        public async Task<ICollection<ProductFormat>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var objs = await GetCache();

            if (statusType != Enums.StatusType.Todos)
                objs = objs.Where(x => x.StatusType == statusType).ToList();

            return await SetFullProperties(objs, objectType);

            //return await SetFullProperties(statusType == Enums.StatusType.Todos
            //    ? await GetAllAsync("GP_WEB_APP_171")
            //    : await GetAllAsync("GP_WEB_APP_272", new List<dynamic> { (int)statusType }), objectType);
        }

        public async Task<ICollection<ProductFormat>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var objs = await GetCache();

            return await SetFullProperties(objs.Where(x => ids.Any(y => y.Equals(x.Id))).ToList(), objectType);

            //return await SetFullProperties(await GetAllAsync("GP_WEB_APP_422", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<ICollection<ProductFormat>> GetAllByProductFormulaIdAndProductMaterialTypeIdAsync(int productFormulaId, int productMaterialTypeId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_282", new List<dynamic> { productFormulaId, productMaterialTypeId, (int)Enums.StatusType.Activo }), objectType);
        }

        public async Task<ProductFormat> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            var objs = await GetCache();

            return await SetFullProperties(objs.FirstOrDefault(x => x.Id.Equals(id)), objectType);

            //return await SetFullProperties(await GetAsync("GP_WEB_APP_170", new List<dynamic> { id }), objectType);
        }

        public Task CreateAsync(ProductFormat obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            //Remove cache
            _memoryCache.Remove(_cacheName);

            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(ProductFormat obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(obj, Enums.ObjectAction.Update, currentObj);

            //Set obj
            currentObj.UpdatedBy = obj.UpdatedBy;

            currentObj.Name = obj.Name;
            currentObj.UnitOfMeasurementId = obj.UnitOfMeasurementId;
            currentObj.Ancho = obj.Ancho;
            currentObj.Largo = obj.Largo;
            currentObj.Panol = obj.Panol;
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

        private static void CheckRules(ProductFormat obj, Enums.ObjectAction objectAction, ProductFormat currentObj = null)
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
            var id = GetValue("GP_WEB_APP_169", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<ProductFormat> SetFullProperties(ProductFormat obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.UnitOfMeasurement = await _unitOfMeasurementRepository.GetAsync(obj.UnitOfMeasurementId);
            }

            return obj;
        }

        public async Task<ICollection<ProductFormat>> SetFullProperties(ICollection<ProductFormat> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                var unitOfMeasurementIds = objs.GroupBy(x => x.UnitOfMeasurementId).Select(g => g.Key);
                var unitsOfMeasurement = await _unitOfMeasurementRepository.GetAllWithIdsAsync(unitOfMeasurementIds);

                foreach (var unitOfMeasurement in unitsOfMeasurement)
                    objs.Where(x => x.UnitOfMeasurementId.Equals(unitOfMeasurement.Id)).ToList().ForEach(x => x.UnitOfMeasurement = unitOfMeasurement);
            }

            return objs;
        }
    }
}
