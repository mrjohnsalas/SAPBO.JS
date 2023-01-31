using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductFormulaProductionProcessBusiness : SapB1GenericRepository<ProductFormulaProductionProcess>, IProductFormulaProductionProcessBusiness
    {
        private const string _tableName = TableNames.ProductFormulaProductionProcess;
        private readonly IProductMaterialTypeBusiness _productMaterialTypeRepository;
        private readonly IProductionProcessBusiness _productionProcessRepository;

        public ProductFormulaProductionProcessBusiness(SapB1Context context, ISapB1AutoMapper<ProductFormulaProductionProcess> mapper,
            IProductMaterialTypeBusiness productMaterialTypeRepository,
            IProductionProcessBusiness productionProcessRepository) : base(context, mapper)
        {
            _productMaterialTypeRepository = productMaterialTypeRepository;
            _productionProcessRepository = productionProcessRepository;
        }

        public async Task<ICollection<ProductFormulaProductionProcess>> GetAllAsync(int productFormulaId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_440", new List<dynamic> { productFormulaId }), objectType);
        }

        public async Task<ICollection<ProductFormulaProductionProcess>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_438", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<ICollection<ProductFormulaProductionProcess>> GetAllByProductFormulaIdAndProductMaterialTypeIdAsync(int productFormulaId, int productMaterialTypeId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_228", new List<dynamic> { productFormulaId, productMaterialTypeId }), objectType);
        }

        public async Task<ProductFormulaProductionProcess> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_212", new List<dynamic> { id }), objectType);
        }

        public async Task CreateAsync(ICollection<ProductFormulaProductionProcess> objs, int productFormulaId)
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

        public async Task UpdateAsync(ICollection<ProductFormulaProductionProcess> objs, int productFormulaId)
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
                    currentObj.ProductionProcessId = obj.ProductionProcessId;
                    currentObj.PreparationTime = obj.PreparationTime;
                    currentObj.Performance = obj.Performance;

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
            return DeleteAllAsync("GP_WEB_APP_436", new List<dynamic> { string.Join(",", ids) });
        }

        public Task DeleteByProductFormulaIdAsync(int productFormulaId)
        {
            return DeleteAllAsync("GP_WEB_APP_437", new List<dynamic> { productFormulaId });
        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_211", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        protected static void CheckRules(ProductFormulaProductionProcess obj, Enums.ObjectAction objectAction, ProductFormulaProductionProcess currentObj = null)
        {
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

        public async Task<ProductFormulaProductionProcess> SetFullProperties(ProductFormulaProductionProcess obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.ProductMaterialType = await _productMaterialTypeRepository.GetAsync(obj.ProductMaterialTypeId);
                obj.ProductionProcess = await _productionProcessRepository.GetAsync(obj.ProductionProcessId);
            }

            return obj;
        }

        public async Task<ICollection<ProductFormulaProductionProcess>> SetFullProperties(ICollection<ProductFormulaProductionProcess> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                var productMaterialTypeIds = objs.GroupBy(x => x.ProductMaterialTypeId).Select(g => g.Key);
                var productMaterialTypes = await _productMaterialTypeRepository.GetAllWithIdsAsync(productMaterialTypeIds);

                var productionProcessIds = objs.GroupBy(x => x.ProductionProcessId).Select(g => g.Key);
                var productionProcesses = await _productionProcessRepository.GetAllWithIdsAsync(productionProcessIds);

                foreach (var productMaterialType in productMaterialTypes)
                    objs.Where(x => x.ProductMaterialTypeId.Equals(productMaterialType.Id)).ToList().ForEach(x => x.ProductMaterialType = productMaterialType);

                foreach (var productionProcess in productionProcesses)
                    objs.Where(x => x.ProductionProcessId.Equals(productionProcess.Id)).ToList().ForEach(x => x.ProductionProcess = productionProcess);
            }

            return objs;
        }
    }
}
