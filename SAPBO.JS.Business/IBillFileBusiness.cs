using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IBillFileBusiness
    {
        Task<ICollection<BillFile>> GetAllAsync(int fileId);

        Task<ICollection<BillFile>> GetAllWithIdsAsync(IEnumerable<int> fileIds);
    }
}
