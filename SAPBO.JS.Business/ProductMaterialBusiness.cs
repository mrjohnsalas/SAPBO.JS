using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductMaterialBusiness : SapB1GenericRepository<ProductMaterial>, IProductMaterialBusiness
    {
        private const string _tableName = TableNames.ProductMaterial;
        private readonly IUnitOfMeasurementBusiness _unitOfMeasurementRepository;
        private readonly IProductGrammageBusiness _productGrammageRepository;
        private readonly IProductionProcessTypeCostBusiness _productionProcessTypeCostRepository;
        private readonly IProductMaterialTypeBusiness _productMaterialTypeRepository;

        public ProductMaterialBusiness(SapB1Context context, ISapB1AutoMapper<ProductMaterial> mapper, IUnitOfMeasurementBusiness unitOfMeasurementRepository, IProductGrammageBusiness productGrammageRepository, IProductionProcessTypeCostBusiness productionProcessTypeCostRepository, IProductMaterialTypeBusiness productMaterialTypeRepository) : base(context, mapper, true)
        {
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _productGrammageRepository = productGrammageRepository;
            _productionProcessTypeCostRepository = productionProcessTypeCostRepository;
            _productMaterialTypeRepository = productMaterialTypeRepository;
        }

        public async Task<ICollection<ProductMaterial>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(statusType == Enums.StatusType.Todos
                ? await GetAllAsync("GP_WEB_APP_191")
                : await GetAllAsync("GP_WEB_APP_280", new List<dynamic> { (int)statusType }), objectType);
        }

        public async Task<ICollection<ProductMaterial>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_429", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<ICollection<ProductMaterial>> GetAllByProductMaterialTypeIdAndGramajeIdAsync(int productFormulaId, int gramajeId, int copies, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var materials = await GetAllAsync("GP_WEB_APP_230", new List<dynamic> { productFormulaId, gramajeId });
            //9   CFB - 50 GRS
            //10  CB - 56 GRS
            //11  CF - 56 GRS
            if (productFormulaId.Equals(2))
            {
                switch (copies)
                {
                    case 2:
                        materials = (ICollection<ProductMaterial>)materials.Where(x => x.Id.Equals(10) || x.Id.Equals(11));
                        break;
                    case 3:
                    case 4:
                        materials = (ICollection<ProductMaterial>)materials.Where(x => x.Id.Equals(9) || x.Id.Equals(10) || x.Id.Equals(11));
                        break;
                    default:
                        materials = (ICollection<ProductMaterial>)materials.Where(x => x.Id.Equals(9) || x.Id.Equals(10));
                        break;
                }
                foreach (var material in materials)
                {
                    material.NroCopias = 1;
                    if (material.Id.Equals(9))
                        material.NroCopias = copies <= 4 ? copies - 2 : copies - 1;
                }
            }
            else
                foreach (var material in materials)
                    material.NroCopias = copies;

            return await SetFullProperties(materials, objectType);
        }

        public async Task<ProductMaterial> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_190", new List<dynamic> { id }), objectType);
        }

        public Task CreateAsync(ProductMaterial obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(ProductMaterial obj)
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
            currentObj.PrecioLocal = obj.PrecioLocal;
            currentObj.PrecioImportado = obj.PrecioImportado;
            currentObj.PrecioLicitacion = obj.PrecioLicitacion;
            currentObj.Stock = obj.Stock;
            currentObj.NroCopias = obj.NroCopias;
            currentObj.ProductGrammageId = obj.ProductGrammageId;
            currentObj.ProductionProcessTypeCostId = obj.ProductionProcessTypeCostId;
            currentObj.UpdatedAt = DateTime.Now;

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

            await SoftDeleteByIdAsync(_tableName, obj, obj.Id.ToString());
        }

        private static void CheckRules(ProductMaterial obj, Enums.ObjectAction objectAction, ProductMaterial currentObj = null)
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
            var id = GetValue("GP_WEB_APP_189", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<ProductMaterial> SetFullProperties(ProductMaterial obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.UnitOfMeasurement = await _unitOfMeasurementRepository.GetAsync(obj.UnitOfMeasurementId);
                obj.ProductGrammage = await _productGrammageRepository.GetAsync(obj.ProductGrammageId);
                obj.ProductionProcessTypeCost = await _productionProcessTypeCostRepository.GetAsync(obj.ProductionProcessTypeCostId);
                obj.ProductMaterialType = await _productMaterialTypeRepository.GetAsync(obj.ProductMaterialTypeId);
            }

            return obj;
        }

        public async Task<ICollection<ProductMaterial>> SetFullProperties(ICollection<ProductMaterial> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                var unitOfMeasurementIds = objs.GroupBy(x => x.UnitOfMeasurementId).Select(g => g.Key);
                var unitsOfMeasurement = await _unitOfMeasurementRepository.GetAllWithIdsAsync(unitOfMeasurementIds);

                var productGrammageIds = objs.GroupBy(x => x.ProductGrammageId).Select(g => g.Key);
                var productGrammages = await _productGrammageRepository.GetAllWithIdsAsync(productGrammageIds);

                var typeCostIds = objs.GroupBy(x => x.ProductionProcessTypeCostId).Select(g => g.Key);
                var typeCosts = await _productionProcessTypeCostRepository.GetAllWithIdsAsync(typeCostIds);

                var productMaterialTypeIds = objs.GroupBy(x => x.ProductMaterialTypeId).Select(g => g.Key);
                var productMaterialTypes = await _productMaterialTypeRepository.GetAllWithIdsAsync(productMaterialTypeIds);

                foreach (var unitOfMeasurement in unitsOfMeasurement)
                    objs.Where(x => x.UnitOfMeasurementId.Equals(unitOfMeasurement.Id)).ToList().ForEach(x => x.UnitOfMeasurement = unitOfMeasurement);

                foreach (var productGrammage in productGrammages)
                    objs.Where(x => x.ProductGrammageId.Equals(productGrammage.Id)).ToList().ForEach(x => x.ProductGrammage = productGrammage);

                foreach (var typeCost in typeCosts)
                    objs.Where(x => x.ProductionProcessTypeCostId.Equals(typeCost.Id)).ToList().ForEach(x => x.ProductionProcessTypeCost = typeCost);

                foreach (var productMaterialType in productMaterialTypes)
                    objs.Where(x => x.ProductMaterialTypeId.Equals(productMaterialType.Id)).ToList().ForEach(x => x.ProductMaterialType = productMaterialType);
            }

            return objs;
        }
    }
}
