using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IPurchaseOrderBusiness
    {
        Task<ICollection<PurchaseOrder>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<PurchaseOrder>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<PurchaseOrder>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<PurchaseOrder> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);
    }
}
