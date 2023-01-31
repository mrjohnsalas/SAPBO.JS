using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IPurchaseOrderDetailBusiness
    {
        Task<ICollection<PurchaseOrderDetail>> GetAllAsync(int purchaseOrderId);

        Task<ICollection<PurchaseOrderDetail>> GetAllWithIdsAsync(IEnumerable<int> saleOrderIds);

        Task<PurchaseOrderDetail> GetAsync(int id, int lineNum);
    }
}
