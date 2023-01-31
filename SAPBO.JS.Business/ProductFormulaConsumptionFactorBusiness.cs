using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductFormulaConsumptionFactorBusiness : SapB1GenericRepository<ProductFormulaConsumptionFactor>, IProductFormulaConsumptionFactorBusiness
    {
        private const string _tableName = TableNames.ProductFormulaConsumptionFactor;
        private readonly IProductMaterialTypeBusiness _productMaterialTypeRepository;

        public ProductFormulaConsumptionFactorBusiness(SapB1Context context, ISapB1AutoMapper<ProductFormulaConsumptionFactor> mapper,
            IProductMaterialTypeBusiness productMaterialTypeRepository) : base(context, mapper)
        {
            _productMaterialTypeRepository = productMaterialTypeRepository;
        }

        public async Task<ICollection<ProductFormulaConsumptionFactor>> GetAllAsync(int productFormulaId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_441", new List<dynamic> { productFormulaId }), objectType);
        }

        public async Task<ICollection<ProductFormulaConsumptionFactor>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_439", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<ICollection<ProductFormulaConsumptionFactor>> GetAllByProductFormulaIdAndProductMaterialTypeIdAsync(int productFormulaId, int productMaterialTypeId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_229", new List<dynamic> { productFormulaId, productMaterialTypeId }), objectType);
        }

        public async Task<ProductFormulaConsumptionFactor> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_206", new List<dynamic> { id }), objectType);
        }

        public async Task CreateAsync(ICollection<ProductFormulaConsumptionFactor> objs, int productFormulaId)
        {
            if (objs != null && objs.Any() && productFormulaId > 0)
            {
                var id = GetNewId();
                foreach (var obj in objs)
                {
                    obj.Id = id;
                    obj.ProductFormulaId = productFormulaId;
                    await CreateAsync(_tableName, obj, obj.Id.ToString());
                    id++;
                }
            }
        }

        public async Task UpdateAsync(ICollection<ProductFormulaConsumptionFactor> objs, int productFormulaId)
        {
            if (objs == null || !objs.Any())
            {
                await DeleteByProductFormulaIdAsync(productFormulaId);
            }
            else
            {
                //Create
                var createObjs = objs.Where(x => x.Id.Equals(0));
                await CreateAsync(createObjs.ToList(), productFormulaId);

                var updateObjs = objs.Where(x => !x.Id.Equals(0));
                var currentObjs = await GetAllAsync(productFormulaId);
                var deleteObjs = currentObjs.Where(p => !updateObjs.Any(p2 => p2.Id == p.Id));

                //Delete
                await DeleteAllWithIdsAsync(deleteObjs.Select(x => x.Id));

                //Update
                foreach (var obj in updateObjs)
                {
                    //Get obj
                    var currentObj = await GetAsync(obj.Id);
                    if (currentObj == null)
                        throw new Exception(AppMessages.NotFoundFromOperation);

                    //Set obj
                    currentObj.ProductMaterialTypeId = obj.ProductMaterialTypeId;
                    currentObj.From = obj.From;
                    currentObj.Until = obj.Until;
                    currentObj.Factor = obj.Factor;

                    await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            //Get obj
            var obj = await GetAsync(id);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            await DeleteByIdAsync(_tableName, obj, obj.Id.ToString());
        }

        public Task DeleteAllWithIdsAsync(IEnumerable<int> ids)
        {
            return DeleteAllAsync("GP_WEB_APP_434", new List<dynamic> { string.Join(",", ids) });
        }

        public Task DeleteByProductFormulaIdAsync(int productFormulaId)
        {
            return DeleteAllAsync("GP_WEB_APP_435", new List<dynamic> { productFormulaId });
        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_205", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        protected static void CheckRules(ProductFormulaConsumptionFactor obj, Enums.ObjectAction objectAction, ProductFormulaConsumptionFactor currentObj = null)
        {
            //General
            if (obj.From >= obj.Until)
                throw new Exception(string.Format(AppMessages.ValueLessFieldErrorMessage, obj.From, obj.Until));

            if (obj.Factor.Equals(0))
                throw new Exception(string.Format(AppMessages.ValueGreaterZeroFieldErrorMessage, "Factor"));

            //switch (objectAction)
            //{
            //    case Enums.ObjectAction.Insert:
            //        //Check User - Create
            //        if (string.IsNullOrEmpty(obj.CreatedBy))
            //            throw new Exception(AppMessages.UserError);
            //        break;
            //    case Enums.ObjectAction.Update:
            //        //Check Status
            //        if (currentObj?.StatusType != Enums.StatusType.Activo)
            //            throw new Exception(AppMessages.StatusError);

            //        //Check User - Update
            //        if (string.IsNullOrEmpty(obj.UpdatedBy))
            //            throw new Exception(AppMessages.UserError);
            //        break;
            //    case Enums.ObjectAction.Delete:
            //        //Check Status
            //        if (obj.StatusType != Enums.StatusType.Activo)
            //            throw new Exception(AppMessages.StatusError);

            //        //Check User - Delete
            //        if (string.IsNullOrEmpty(obj.DeletedBy))
            //            throw new Exception(AppMessages.UserError);
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException(nameof(objectAction), objectAction, null);
            //}
        }

        public async Task<ProductFormulaConsumptionFactor> SetFullProperties(ProductFormulaConsumptionFactor obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.ProductMaterialType = await _productMaterialTypeRepository.GetAsync(obj.ProductMaterialTypeId);
            }

            return obj;
        }

        public async Task<ICollection<ProductFormulaConsumptionFactor>> SetFullProperties(ICollection<ProductFormulaConsumptionFactor> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                var productMaterialTypeIds = objs.GroupBy(x => x.ProductMaterialTypeId).Select(g => g.Key);
                var productMaterialTypes = await _productMaterialTypeRepository.GetAllWithIdsAsync(productMaterialTypeIds);

                foreach (var productMaterialType in productMaterialTypes)
                    objs.Where(x => x.ProductMaterialTypeId.Equals(productMaterialType.Id)).ToList().ForEach(x => x.ProductMaterialType = productMaterialType);
            }

            return objs;
        }
    }
}
