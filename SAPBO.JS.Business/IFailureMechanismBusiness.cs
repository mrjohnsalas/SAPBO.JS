using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IFailureMechanismBusiness
    {
        Task<ICollection<FailureMechanism>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<FailureMechanism>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<FailureMechanism> GetAsync(int id);

        Task CreateAsync(FailureMechanism obj);

        Task UpdateAsync(FailureMechanism obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
