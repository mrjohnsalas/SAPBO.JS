using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductionProcessBusiness : SapB1GenericRepository<ProductionProcess>, IProductionProcessBusiness
    {
        private const string _tableName = TableNames.ProductionProcess;
        private readonly IProductionProcessTypeCostBusiness _productionProcessTypeCostRepository;

        public ProductionProcessBusiness(SapB1Context context, ISapB1AutoMapper<ProductionProcess> mapper, IProductionProcessTypeCostBusiness productionProcessTypeCostRepository) : base(context, mapper, true)
        {
            _productionProcessTypeCostRepository = productionProcessTypeCostRepository;
        }

        public async Task<ICollection<ProductionProcess>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(statusType == Enums.StatusType.Todos
                ? await GetAllAsync("GP_WEB_APP_186")
                : await GetAllAsync("GP_WEB_APP_273", new List<dynamic> { (int)statusType }), objectType);
        }

        public async Task<ICollection<ProductionProcess>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_430", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<ProductionProcess> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_185", new List<dynamic> { id }), objectType);
        }

        public Task CreateAsync(ProductionProcess obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(ProductionProcess obj)
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
            currentObj.ProductionProcessTypeCostId = obj.ProductionProcessTypeCostId;
            currentObj.Costo = obj.Costo;
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

        private static void CheckRules(ProductionProcess obj, Enums.ObjectAction objectAction, ProductionProcess currentObj = null)
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
            var id = GetValue("GP_WEB_APP_184", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<ProductionProcess> SetFullProperties(ProductionProcess obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.ProductionProcessTypeCost = await _productionProcessTypeCostRepository.GetAsync(obj.ProductionProcessTypeCostId);
            }

            return obj;
        }

        public async Task<ICollection<ProductionProcess>> SetFullProperties(ICollection<ProductionProcess> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                var typeCostIds = objs.GroupBy(x => x.ProductionProcessTypeCostId).Select(g => g.Key);
                var typeCosts = await _productionProcessTypeCostRepository.GetAllWithIdsAsync(typeCostIds);

                foreach (var typeCost in typeCosts)
                    objs.Where(x => x.ProductionProcessTypeCostId.Equals(typeCost.Id)).ToList().ForEach(x => x.ProductionProcessTypeCost = typeCost);
            }

            return objs;
        }
    }
}
