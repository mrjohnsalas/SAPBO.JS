using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IBillDetailBusiness
    {
        Task<ICollection<BillDetail>> GetAllAsync(int billId);

        Task<ICollection<BillDetail>> GetAllWithIdsAsync(IEnumerable<int> billIds);
    }
}
