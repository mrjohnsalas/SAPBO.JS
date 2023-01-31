using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class BusinessPartnerContactBusiness : BusinessPartnerContactRepository, IBusinessPartnerContactBusiness
    {
        public BusinessPartnerContactBusiness(SapB1Context context, ISapB1AutoMapper<BusinessPartnerContact> mapper) : base(context, mapper)
        {

        }

        public Task<ICollection<BusinessPartnerContact>> GetAllAsync(string businessPartnerId)
        {
            return GetAllAsync("GP_WEB_APP_338", new List<dynamic> { businessPartnerId });
        }

        public Task<ICollection<BusinessPartnerContact>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            return GetAllAsync("GP_WEB_APP_328", new List<dynamic> { string.Join(",", ids) });
        }

        public Task<ICollection<BusinessPartnerContact>> GetAllWithIdsAsync(IEnumerable<string> businessPartnerIds)
        {
            return GetAllAsync("GP_WEB_APP_330", new List<dynamic> { string.Join(",", businessPartnerIds) });
        }

        public Task<BusinessPartnerContact> GetAsync(int id)
        {
            return GetAsync("GP_WEB_APP_339", new List<dynamic> { id });
        }

        public Task CreateAsync(BusinessPartnerContact obj)
        {
            return Task.Run(() => BusinessPartnerContact(obj, Enums.OperationType.Create));
        }

        public async Task UpdateAsync(BusinessPartnerContact obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            obj.LineNum = currentObj.LineNum;
            await Task.Run(() => BusinessPartnerContact(obj, Enums.OperationType.Update));
        }
    }
}
