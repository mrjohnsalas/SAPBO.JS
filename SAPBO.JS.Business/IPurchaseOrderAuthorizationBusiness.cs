using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IPurchaseOrderAuthorizationBusiness
    {
        Task<ICollection<PurchaseOrderAuthorization>> GetAllAsync(int year, int month);

        Task<PurchaseOrderAuthorization> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<ApprovalListResult>> ApproveListAsync(List<int> ids, string updatedBy);

        Task ApproveAsync(int id, string updatedBy);

        Task OverrideAsync(int id, string updatedBy);

        Task RejectAsync(int id, string reason, string updatedBy);
    }
}
