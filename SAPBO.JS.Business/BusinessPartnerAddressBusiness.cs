using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class BusinessPartnerAddressBusiness : SapB1GenericRepository<BusinessPartnerAddress>, IBusinessPartnerAddressBusiness
    {
        public BusinessPartnerAddressBusiness(SapB1Context context, ISapB1AutoMapper<BusinessPartnerAddress> mapper) : base(context, mapper)
        {

        }

        public Task<ICollection<BusinessPartnerAddress>> GetAllAsync(string businessPartnerId)
        {
            return GetAllAsync("GP_WEB_APP_003", new List<dynamic> { businessPartnerId });
        }

        public Task<ICollection<BusinessPartnerAddress>> GetAllWithIdsAsync(string businessPartnerId, IEnumerable<string> ids)
        {
            return GetAllAsync("GP_WEB_APP_361", new List<dynamic> { businessPartnerId, string.Join(",", ids) });
        }

        public Task<BusinessPartnerAddress> GetAsync(string businessPartnerId, string id)
        {
            return GetAsync("GP_WEB_APP_042", new List<dynamic> { businessPartnerId, id });
        }
    }
}
