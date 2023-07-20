using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Dto;

namespace SAPBO.JS.Business
{
    public class BilledAmountDataBusiness : SapB1GenericRepository<BilledAmountData>, IBilledAmountDataBusiness
    {
        public BilledAmountDataBusiness(SapB1Context context, ISapB1AutoMapper<BilledAmountData> mapper) : base(context, mapper)
        {
        }

        public Task<ICollection<BilledAmountData>> GetBilledAmountDataByBusinessPartnerIdAsync(string businessPartnerId)
        {
            return GetAllAsync("GP_WEB_APP_513", new List<dynamic> { businessPartnerId });
        }

        public Task<ICollection<BilledAmountData>> GetBilledAmountDataBySaleEmployeeIdAsync(int saleEmployeeId)
        {
            return GetAllAsync("GP_WEB_APP_491", new List<dynamic> { saleEmployeeId });
        }

        public Task<ICollection<BilledAmountData>> GetBilledAmountDataByProductIdAsync(string productId)
        {
            return GetAllAsync("GP_WEB_APP_522", new List<dynamic> { productId });
        }

        public Task<ICollection<BilledAmountData>> GetBilledAmountDataByProductIdAndSaleEmployeeIdAsync(string productId, int saleEmployeeId)
        {
            return GetAllAsync("GP_WEB_APP_523", new List<dynamic> { productId, saleEmployeeId });
        }
    }
}
