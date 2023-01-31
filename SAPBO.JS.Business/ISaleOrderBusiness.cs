using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ISaleOrderBusiness
    {
        Task<ICollection<SaleOrder>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<SaleOrder>> GetAllPendingAsync();

        Task<ICollection<SaleOrder>> GetAllBySaleEmployeeIdAsync(int saleEmployeeId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<SaleOrder>> GetAllPendingBySaleEmployeeIdAsync(int saleEmployeeId);

        Task<ICollection<SaleOrder>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<SaleOrder>> GetAllPendingByBusinessPartnerIdAsync(string businessPartnerId);

        Task<ICollection<SaleOrder>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<SaleOrder> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(ShoppingCart obj);

        Task DeleteAsync(int id);

        Task AddFileAttachmentAsync(int saleOrderId, string path, string fileNameWithoutExtension, string fileExtensionWithoutDot);
    }
}
