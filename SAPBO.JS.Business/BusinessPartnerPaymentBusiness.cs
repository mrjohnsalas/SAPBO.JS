using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class BusinessPartnerPaymentBusiness : SapB1GenericRepository<BusinessPartnerPayment>, IBusinessPartnerPaymentBusiness
    {
        public BusinessPartnerPaymentBusiness(SapB1Context context, ISapB1AutoMapper<BusinessPartnerPayment> mapper) : base(context, mapper)
        {

        }

        public Task<ICollection<BusinessPartnerPayment>> GetAllAsync(string businessPartnerId)
        {
            return GetAllAsync("GP_WEB_APP_337", new List<dynamic> { businessPartnerId });
        }

        public Task<BusinessPartnerPayment> GetAsync(int id)
        {
            return GetAsync("GP_WEB_APP_005", new List<dynamic> { id });
        }

        public Task<ICollection<BusinessPartnerPayment>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            return GetAllAsync("GP_WEB_APP_402", new List<dynamic> { string.Join(",", ids) });
        }
    }
}
