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
    public class ProductionMachineBusiness : SapB1GenericRepository<ProductionMachine>, IProductionMachineBusiness
    {
        private const string _tableName = TableNames.ProductionMachine;

        private readonly IProductionMachineZoneBusiness _productionMachineZoneRepository;

        public ProductionMachineBusiness(SapB1Context context, ISapB1AutoMapper<ProductionMachine> mapper, IProductionMachineZoneBusiness productionMachineZoneRepository) : base(context, mapper, true)
        {
            _productionMachineZoneRepository = productionMachineZoneRepository;
        }

        public async Task<ICollection<ProductionMachine>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(statusType == Enums.StatusType.Todos
                ? await GetAllAsync("GP_WEB_APP_066")
                : await GetAllAsync("GP_WEB_APP_243", new List<dynamic> { (int)statusType }), objectType);
        }

        public async Task<ICollection<ProductionMachine>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_306", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<ProductionMachine> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_065", new List<dynamic> { id }), objectType);
        }

        public async Task CreateAsync(ProductionMachine obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            await CreateAsync(_tableName, obj, obj.Id.ToString());

            await _productionMachineZoneRepository.CreateAsync(obj.Zones, obj.Id);
        }

        public async Task UpdateAsync(ProductionMachine obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(obj, Enums.ObjectAction.Update, currentObj);

            //Set obj
            currentObj.UpdatedBy = obj.UpdatedBy;

            currentObj.Name = obj.Name;
            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());

            await _productionMachineZoneRepository.UpdateAsync(obj.Zones, currentObj.Id, obj.UpdatedBy);
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

        private static void CheckRules(ProductionMachine obj, Enums.ObjectAction objectAction, ProductionMachine currentObj = null)
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
            var id = GetValue("GP_WEB_APP_064", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<ProductionMachine> SetFullProperties(ProductionMachine obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            if (objectType == Enums.ObjectType.Full)
            {
                obj.Zones = await _productionMachineZoneRepository.GetAllByProductionMachineIdAsync(obj.Id);
            }

            return obj;
        }

        public async Task<ICollection<ProductionMachine>> SetFullProperties(ICollection<ProductionMachine> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full)
            {
                //Details
                var productionMachineIds = objs.Select(x => x.Id);

                var zones = await _productionMachineZoneRepository.GetAllWithProductionMachineIdsAsync(productionMachineIds);

                foreach (var productionMachine in objs)
                {
                    productionMachine.Zones = zones.Where(x => x.ProductionMachineId.Equals(productionMachine.Id)).ToList();
                }
            }

            return objs;
        }
    }
}
