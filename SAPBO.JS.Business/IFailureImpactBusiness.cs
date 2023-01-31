using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IFailureImpactBusiness
    {
        Task<ICollection<FailureImpact>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<FailureImpact>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<FailureImpact> GetAsync(int id);

        Task CreateAsync(FailureImpact obj);

        Task UpdateAsync(FailureImpact obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
