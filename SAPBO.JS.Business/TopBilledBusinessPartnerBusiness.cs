using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Dto;

namespace SAPBO.JS.Business
{
    public class TopBilledBusinessPartnerBusiness : SapB1GenericRepository<TopBilledBusinessPartner>, ITopBilledBusinessPartnerBusiness
    {
        public TopBilledBusinessPartnerBusiness(SapB1Context context, ISapB1AutoMapper<TopBilledBusinessPartner> mapper) : base(context, mapper)
        {
        }

        public Task<ICollection<TopBilledBusinessPartner>> GetTopBilledBusinessPartnerBySaleEmployeeIdAsync(int saleEmployeeId, int count)
        {
            return GetAllAsync("GP_WEB_APP_502", new List<dynamic> { saleEmployeeId, count });
        }

        public Task<ICollection<TopBilledBusinessPartner>> GetTopBilledBusinessPartnerByProductIdAsync(string productId, int count)
        {
            return GetAllAsync("GP_WEB_APP_520", new List<dynamic> { productId, count });
        }

        public Task<ICollection<TopBilledBusinessPartner>> GetTopBilledBusinessPartnerByProductIdAndSaleEmployeeIdAsync(string productId, int saleEmployeeId, int count)
        {
            return GetAllAsync("GP_WEB_APP_521", new List<dynamic> { productId, saleEmployeeId, count });
        }
    }
}
