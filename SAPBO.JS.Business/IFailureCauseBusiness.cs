using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IFailureCauseBusiness
    {
        Task<ICollection<FailureCause>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<FailureCause>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<FailureCause> GetAsync(int id);

        Task CreateAsync(FailureCause obj);

        Task UpdateAsync(FailureCause obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
