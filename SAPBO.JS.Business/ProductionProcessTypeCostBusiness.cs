using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductionProcessTypeCostBusiness : SapB1GenericRepository<ProductionProcessTypeCost>, IProductionProcessTypeCostBusiness
    {
        private const string _tableName = TableNames.ProductionProcessTypeCost;

        public ProductionProcessTypeCostBusiness(SapB1Context context, ISapB1AutoMapper<ProductionProcessTypeCost> mapper) : base(context, mapper, true)
        {

        }

        public Task<ICollection<ProductionProcessTypeCost>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return statusType == Enums.StatusType.Todos
                ? GetAllAsync("GP_WEB_APP_183")
                : GetAllAsync("GP_WEB_APP_270", new List<dynamic> { (int)statusType });
        }

        public Task<ICollection<ProductionProcessTypeCost>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            return GetAllAsync("GP_WEB_APP_425", new List<dynamic> { string.Join(",", ids) });
        }

        public Task<ProductionProcessTypeCost> GetAsync(int id)
        {
            return GetAsync("GP_WEB_APP_182", new List<dynamic> { id });
        }

        public Task CreateAsync(ProductionProcessTypeCost obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(ProductionProcessTypeCost obj)
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

        private static void CheckRules(ProductionProcessTypeCost obj, Enums.ObjectAction objectAction, ProductionProcessTypeCost currentObj = null)
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
            var id = GetValue("GP_WEB_APP_181", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }
    }
}
