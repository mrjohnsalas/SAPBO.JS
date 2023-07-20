using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Dto;

namespace SAPBO.JS.Business
{
    public interface ITopBilledBusinessPartnerBusiness
    {
        Task<ICollection<TopBilledBusinessPartner>> GetTopBilledBusinessPartnerBySaleEmployeeIdAsync(int saleEmployeeId, int count);

        Task<ICollection<TopBilledBusinessPartner>> GetTopBilledBusinessPartnerByProductIdAsync(string productId, int count);

        Task<ICollection<TopBilledBusinessPartner>> GetTopBilledBusinessPartnerByProductIdAndSaleEmployeeIdAsync(string productId, int saleEmployeeId, int count);
    }
}
