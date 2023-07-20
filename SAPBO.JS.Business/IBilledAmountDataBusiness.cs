using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Dto;

namespace SAPBO.JS.Business
{
    public interface IBilledAmountDataBusiness
    {
        Task<ICollection<BilledAmountData>> GetBilledAmountDataByBusinessPartnerIdAsync(string businessPartnerId);

        Task<ICollection<BilledAmountData>> GetBilledAmountDataBySaleEmployeeIdAsync(int saleEmployeeId);

        Task<ICollection<BilledAmountData>> GetBilledAmountDataByProductIdAsync(string productId);

        Task<ICollection<BilledAmountData>> GetBilledAmountDataByProductIdAndSaleEmployeeIdAsync(string productId, int saleEmployeeId);
    }
}
