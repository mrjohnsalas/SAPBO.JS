using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class BusinessPartnerDriverBusiness : SapB1GenericRepository<BusinessPartnerDriver>, IBusinessPartnerDriverBusiness
    {
        private const string _tableName = TableNames.Driver;

        public BusinessPartnerDriverBusiness(SapB1Context context, ISapB1AutoMapper<BusinessPartnerDriver> mapper) : base(context, mapper)
        {
            
        }

        public Task<ICollection<BusinessPartnerDriver>> GetAllAsync(string businessPartnerId)
        {
            return GetAllAsync("GP_WEB_APP_150", new List<dynamic> { businessPartnerId });
        }

        public Task<ICollection<BusinessPartnerDriver>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            return GetAllAsync("GP_WEB_APP_481", new List<dynamic> { string.Join(",", ids) });
        }

        public Task<BusinessPartnerDriver> GetAsync(int id)
        {
            return GetAsync("GP_WEB_APP_151", new List<dynamic> { id });
        }

        public Task<BusinessPartnerDriver> GetByLicenseIdAsync(string licenseId)
        {
            return GetAsync("GP_WEB_APP_259", new List<dynamic> { licenseId });
        }

        public async Task CreateAsync(BusinessPartnerDriver obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            //Check License
            var driver = await GetByLicenseIdAsync(obj.LicenseId);
            if (driver != null)
                throw new Exception(AppMessages.Driver_LicenseId);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            await CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(BusinessPartnerDriver obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(obj, Enums.ObjectAction.Update, currentObj);

            //Check License
            var driver = await GetByLicenseIdAsync(obj.LicenseId);
            if (driver != null && driver.Id != obj.Id)
                throw new Exception(AppMessages.Driver_LicenseId);

            //Set obj
            currentObj.UpdatedBy = obj.UpdatedBy;

            currentObj.FirstName = obj.FirstName;
            currentObj.LastName = obj.LastName;
            currentObj.LicenseId = obj.LicenseId;
            currentObj.LicenseExpirationDate = obj.LicenseExpirationDate;
            currentObj.Phone = obj.Phone;
            currentObj.Email = obj.Email;
            currentObj.BusinessPartnerId = obj.BusinessPartnerId;
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

        private static void CheckRules(BusinessPartnerDriver obj, Enums.ObjectAction objectAction, BusinessPartnerDriver currentObj = null)
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
            var id = GetValue("GP_WEB_APP_262", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }
    }
}
