using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IDeliveryBusiness
    {
        Task<ICollection<Delivery>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Delivery>> GetAllByCarrierIdAsync(string carrierId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Delivery>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<Delivery> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<Delivery>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Delivery>> GetAllBySaleOrderIdAndLineNumAsync(int saleOrderId, int lineNum);

        Task<ICollection<Delivery>> GetAllBySaleOrderIdAndWithIdsAsync(int saleOrderId, IEnumerable<int> lineNums);

        Task DispatchedAsync(int id, string updatedBy);

        Task DeliveredAsync(int id, string updatedBy);

        Task<ICollection<ApprovalListResult>> SetCarrierAsync(List<int> ids, string carrierId, string addressId);
    }
}
