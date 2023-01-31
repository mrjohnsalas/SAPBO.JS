using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IFailureSeverityBusiness
    {
        Task<ICollection<FailureSeverity>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<FailureSeverity>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<FailureSeverity> GetAsync(int id);

        Task CreateAsync(FailureSeverity obj);

        Task UpdateAsync(FailureSeverity obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
