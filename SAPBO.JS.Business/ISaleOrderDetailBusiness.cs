using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ISaleOrderDetailBusiness
    {
        Task<ICollection<SaleOrderDetail>> GetAllAsync(int saleOrderId);

        Task<ICollection<SaleOrderDetail>> GetAllPendingAsync();

        Task<ICollection<SaleOrderDetail>> GetAllPendingBySaleEmployeeIdAsync(int saleEmployeeId);

        Task<ICollection<SaleOrderDetail>> GetAllPendingByBusinessPartnerIdAsync(string businessPartnerId);

        Task<ICollection<SaleOrderDetail>> GetAllWithIdsAsync(IEnumerable<int> saleOrderIds);

        Task<SaleOrderDetail> GetAsync(int id, int lineNum);

        Task<SaleOrderDetail> SetFullProperties(SaleOrderDetail obj);

        Task<ICollection<SaleOrderDetail>> SetFullProperties(ICollection<SaleOrderDetail> objs);
    }
}
