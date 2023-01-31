using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class QuotationAccessoryBusiness : SapB1GenericRepository<QuotationAccessory>, IQuotationAccessoryBusiness
    {
        private const string _tableName = TableNames.QuotationAccessory;

        public QuotationAccessoryBusiness(SapB1Context context, ISapB1AutoMapper<QuotationAccessory> mapper) : base(context, mapper, true)
        {

        }

        public Task<ICollection<QuotationAccessory>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return statusType == Enums.StatusType.Todos
                ? GetAllAsync("GP_WEB_APP_177")
                : GetAllAsync("GP_WEB_APP_271", new List<dynamic> { (int)statusType });
        }

        public Task<ICollection<QuotationAccessory>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            return GetAllAsync("GP_WEB_APP_421", new List<dynamic> { string.Join(",", ids) });
        }

        public Task<QuotationAccessory> GetAsync(int id)
        {
            return GetAsync("GP_WEB_APP_176", new List<dynamic> { id });
        }

        public Task CreateAsync(QuotationAccessory obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(QuotationAccessory obj)
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
            currentObj.ValueXje = obj.ValueXje;
            currentObj.Index = obj.Index;
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

        private static void CheckRules(QuotationAccessory obj, Enums.ObjectAction objectAction, QuotationAccessory currentObj = null)
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
            var id = GetValue("GP_WEB_APP_175", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }
    }
}
