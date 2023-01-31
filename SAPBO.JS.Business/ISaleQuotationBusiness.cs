using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ISaleQuotationBusiness
    {
        Task<ICollection<SaleQuotation>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<SaleQuotation>> GetAllBySaleEmployeeIdAsync(int saleEmployeeId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<SaleQuotation>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<SaleQuotation> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(SaleQuotation obj);

        Task UpdateAsync(SaleQuotation obj);

        Task DeleteAsync(int id, string deleteBy);

        Task EndAsync(int id, string updatedBy);

        Task ApproveAsync(int id, List<int> acceptedProducts, string updatedBy);

        Task RejectAsync(int id, string rejectReason, string updatedBy);

        Task PendingAsync(int id, string updatedBy);
    }
}
