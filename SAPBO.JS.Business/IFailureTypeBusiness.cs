using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IFailureTypeBusiness
    {
        Task<ICollection<FailureType>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<FailureType>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<FailureType> GetAsync(int id);

        Task CreateAsync(FailureType obj);

        Task UpdateAsync(FailureType obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
