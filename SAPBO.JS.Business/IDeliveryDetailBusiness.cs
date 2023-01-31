using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IDeliveryDetailBusiness
    {
        Task<ICollection<DeliveryDetail>> GetAllAsync(int deliveryId);

        Task<ICollection<DeliveryDetail>> GetAllBySaleOrderIdAndLineNumAsync(int saleOrderId, int lineNum);

        Task<ICollection<DeliveryDetail>> GetAllBySaleOrderIdAndWithIdsAsync(int saleOrderId, IEnumerable<int> lineNums);

        Task<ICollection<DeliveryDetail>> GetAllWithIdsAsync(IEnumerable<int> deliveryIds);

        Task<DeliveryDetail> GetAsync(int deliveryId, int lineNum);
    }
}
