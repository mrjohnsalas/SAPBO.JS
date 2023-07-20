using SAPBO.JS.Model.Dto;

namespace SAPBO.JS.Business
{
    public interface ITopBilledProductBusiness
    {
        Task<ICollection<TopBilledProduct>> GetTopBilledProductAsync(int count);

        Task<ICollection<TopBilledProduct>> GetTopBilledProductBySaleEmployeeIdAsync(int saleEmployeeId, int count);

        Task<ICollection<TopBilledProduct>> GetTopBilledProductByBusinessPartnerIdAsync(string businessPartnerId, int count);
    }
}
