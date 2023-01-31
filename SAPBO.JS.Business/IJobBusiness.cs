using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IJobBusiness
    {
        Task<ICollection<Job>> GetAllAsync(string businessUnitId, Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<Job>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<Job> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(Job obj);

        Task UpdateAsync(Job obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
