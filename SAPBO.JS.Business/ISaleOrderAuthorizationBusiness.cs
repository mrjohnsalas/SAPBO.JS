using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ISaleOrderAuthorizationBusiness
    {
        Task<ICollection<SaleOrderAuthorization>> GetAllAsync(int year, int month);

        Task<ICollection<SaleOrderAuthorization>> GetAllBySaleEmployeeIdAsync(int saleEmployeeId, int year, int month);

        Task<ICollection<SaleOrderAuthorization>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month);

        Task<SaleOrderAuthorization> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<ApprovalListResult>> ApproveListAsync(List<int> ids, string updatedBy);

        Task ApproveAsync(int id, string updatedBy);

        Task RejectAsync(int id, string reason, string updatedBy);
    }
}
