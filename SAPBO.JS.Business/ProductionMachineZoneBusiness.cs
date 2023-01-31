using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductionMachineZoneBusiness : SapB1GenericRepository<ProductionMachineZone>, IProductionMachineZoneBusiness
    {
        private const string _tableName = TableNames.ProductionMachineZone;

        public ProductionMachineZoneBusiness(SapB1Context context, ISapB1AutoMapper<ProductionMachineZone> mapper) : base(context, mapper, true)
        {

        }

        public Task<ICollection<ProductionMachineZone>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return statusType == Enums.StatusType.Todos
                ? GetAllAsync("GP_WEB_APP_450")
                : GetAllAsync("GP_WEB_APP_451", new List<dynamic> { (int)statusType });
        }

        public Task<ICollection<ProductionMachineZone>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            return GetAllAsync("GP_WEB_APP_452", new List<dynamic> { string.Join(",", ids) });
        }

        public Task<ICollection<ProductionMachineZone>> GetAllWithProductionMachineIdsAsync(IEnumerable<int> ids)
        {
            return GetAllAsync("GP_WEB_APP_455", new List<dynamic> { string.Join(",", ids) });
        }

        public Task<ICollection<ProductionMachineZone>> GetAllByProductionMachineIdAsync(int productionMachineId, Enums.StatusType statusType = Enums.StatusType.Activo)
        {
            return statusType == Enums.StatusType.Todos
                ? GetAllAsync("GP_WEB_APP_453", new List<dynamic> { productionMachineId })
                : GetAllAsync("GP_WEB_APP_454", new List<dynamic> { productionMachineId, (int)statusType });
        }

        public Task<ProductionMachineZone> GetAsync(int id)
        {
            return GetAsync("GP_WEB_APP_449", new List<dynamic> { id });
        }

        public async Task CreateAsync(ICollection<ProductionMachineZone> objs, int productionMachineId)
        {
            if (objs != null && objs.Any() && productionMachineId > 0)
            {
                var currentDate = DateTime.Now;
                var id = GetNewId();
                foreach (var obj in objs)
                {
                    obj.Id = id;
                    obj.StatusId = (int)Enums.StatusType.Activo;
                    obj.CreatedAt = currentDate;
                    obj.ProductionMachineId = productionMachineId;
                    await CreateAsync(_tableName, obj, obj.Id.ToString());
                    id++;
                }
            }
        }

        public async Task UpdateAsync(ICollection<ProductionMachineZone> objs, int productionMachineId, string updateBy)
        {
            var currentObjs = await GetAllByProductionMachineIdAsync(productionMachineId);
            var currentDate = DateTime.Now;

            if (objs == null || !objs.Any())
            {
                await DeleteAllAsync(currentObjs, updateBy);
            }
            else
            {
                //Create
                var createObjs = objs.Where(x => x.Id.Equals(0));
                await CreateAsync(createObjs.ToList(), productionMachineId);

                var updateObjs = objs.Where(x => !x.Id.Equals(0));
                var deleteObjs = currentObjs.Where(p => !updateObjs.Any(p2 => p2.Id == p.Id));

                //Delete
                await DeleteAllAsync(deleteObjs.ToList(), updateBy);

                //Update
                foreach (var obj in updateObjs)
                {
                    //Get obj
                    var currentObj = await GetAsync(obj.Id);
                    if (currentObj == null)
                        throw new Exception(AppMessages.NotFoundFromOperation);

                    //Set obj
                    currentObj.UpdatedBy = obj.UpdatedBy;

                    currentObj.Name = obj.Name;
                    currentObj.Description = obj.Description;
                    currentObj.StatusId = obj.StatusId;
                    currentObj.UpdatedAt = currentDate;

                    await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
                }
            }
        }

        public async Task DeleteAllAsync(ICollection<ProductionMachineZone> objs, string deleteBy)
        {
            foreach (var obj in objs)
            {
                obj.DeletedBy = deleteBy;

                CheckRules(obj, Enums.ObjectAction.Delete);

                obj.StatusId = (int)Enums.StatusType.Anulado;
                obj.DeletedAt = DateTime.Now;

                await SoftDeleteByIdAsync(_tableName, obj, obj.Id.ToString());
            }
        }

        private static void CheckRules(ProductionMachineZone obj, Enums.ObjectAction objectAction, ProductionMachineZone currentObj = null)
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
            var id = GetValue("GP_WEB_APP_448", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }
    }
}
