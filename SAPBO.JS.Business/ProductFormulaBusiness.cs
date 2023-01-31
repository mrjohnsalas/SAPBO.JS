using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductFormulaBusiness : SapB1GenericRepository<ProductFormula>, IProductFormulaBusiness
    {
        private const string _tableName = TableNames.ProductFormula;
        private readonly IUnitOfMeasurementBusiness _unitOfMeasurementRepository;
        private readonly IProductSuperGroupBusiness _productSuperGroupRepository;
        private readonly IProductFormulaConsumptionFactorBusiness _productFormulaConsumptionFactorRepository;
        private readonly IProductFormulaProductionProcessBusiness _productFormulaProductionProcessRepository;

        public ProductFormulaBusiness(SapB1Context context, ISapB1AutoMapper<ProductFormula> mapper,
            IUnitOfMeasurementBusiness unitOfMeasurementRepository,
            IProductSuperGroupBusiness productSuperGroupRepository,
            IProductFormulaConsumptionFactorBusiness productFormulaConsumptionFactorRepository,
            IProductFormulaProductionProcessBusiness productFormulaProductionProcessRepository) : base(context, mapper, true)
        {
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _productSuperGroupRepository = productSuperGroupRepository;
            _productFormulaConsumptionFactorRepository = productFormulaConsumptionFactorRepository;
            _productFormulaProductionProcessRepository = productFormulaProductionProcessRepository;
        }

        public async Task<ICollection<ProductFormula>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(statusType == Enums.StatusType.Todos
                ? await GetAllAsync("GP_WEB_APP_219")
                : await GetAllAsync("GP_WEB_APP_432", new List<dynamic> { (int)statusType }), objectType);
        }

        public async Task<ICollection<ProductFormula>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_433", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<ProductFormula> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_218", new List<dynamic> { id }), objectType);
        }

        public async Task CreateAsync(ProductFormula obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            await CreateAsync(_tableName, obj, obj.Id.ToString());

            await _productFormulaConsumptionFactorRepository.CreateAsync(obj.ConsumptionFactors, obj.Id);

            await _productFormulaProductionProcessRepository.CreateAsync(obj.ProductionProcesses, obj.Id);
        }

        public async Task UpdateAsync(ProductFormula obj)
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
            currentObj.UnitOfMeasurementId = obj.UnitOfMeasurementId;
            currentObj.ProductSuperGroupId = obj.ProductSuperGroupId;
            currentObj.NroCopiasMinimo = obj.NroCopiasMinimo;
            currentObj.NroCopiasMaximo = obj.NroCopiasMaximo;
            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());

            await _productFormulaConsumptionFactorRepository.UpdateAsync(currentObj.ConsumptionFactors, currentObj.Id);

            await _productFormulaProductionProcessRepository.UpdateAsync(currentObj.ProductionProcesses, currentObj.Id);
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

            await SoftDeleteByIdAsync(_tableName, obj, obj.Id.ToString());
        }

        private static void CheckRules(ProductFormula obj, Enums.ObjectAction objectAction, ProductFormula currentObj = null)
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
            var id = GetValue("GP_WEB_APP_217", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<ProductFormula> SetFullProperties(ProductFormula obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.UnitOfMeasurement = await _unitOfMeasurementRepository.GetAsync(obj.UnitOfMeasurementId);
                obj.ProductSuperGroup = await _productSuperGroupRepository.GetAsync(obj.ProductSuperGroupId);

                if (objectType == Enums.ObjectType.Full)
                {
                    obj.ConsumptionFactors = await _productFormulaConsumptionFactorRepository.GetAllAsync(obj.Id, objectType);
                    obj.ProductionProcesses = await _productFormulaProductionProcessRepository.GetAllAsync(obj.Id, objectType);
                }
            }

            return obj;
        }

        public async Task<ICollection<ProductFormula>> SetFullProperties(ICollection<ProductFormula> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                var unitOfMeasurementIds = objs.GroupBy(x => x.UnitOfMeasurementId).Select(g => g.Key);
                var unitsOfMeasurement = await _unitOfMeasurementRepository.GetAllWithIdsAsync(unitOfMeasurementIds);

                var productSuperGroupIds = objs.GroupBy(x => x.ProductSuperGroupId).Select(g => g.Key);
                var productSuperGroups = await _productSuperGroupRepository.GetAllWithIdsAsync(productSuperGroupIds);

                foreach (var unitOfMeasurement in unitsOfMeasurement)
                    objs.Where(x => x.UnitOfMeasurementId.Equals(unitOfMeasurement.Id)).ToList().ForEach(x => x.UnitOfMeasurement = unitOfMeasurement);

                foreach (var productSuperGroup in productSuperGroups)
                    objs.Where(x => x.ProductSuperGroupId.Equals(productSuperGroup.Id)).ToList().ForEach(x => x.ProductSuperGroup = productSuperGroup);

                if (objectType == Enums.ObjectType.Full)
                {
                    //Details
                    var productFormulaIds = objs.Select(x => x.Id);

                    var consumptionFactors = await _productFormulaConsumptionFactorRepository.GetAllWithIdsAsync(productFormulaIds, objectType);
                    var productionProcesses = await _productFormulaProductionProcessRepository.GetAllWithIdsAsync(productFormulaIds, objectType);

                    foreach (var productFormula in objs)
                    {
                        productFormula.ConsumptionFactors = consumptionFactors.Where(x => x.ProductFormulaId.Equals(productFormula.Id)).ToList();
                        productFormula.ProductionProcesses = productionProcesses.Where(x => x.ProductFormulaId.Equals(productFormula.Id)).ToList();
                    }
                }
            }

            return objs;
        }
    }
}
