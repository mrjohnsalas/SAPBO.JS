using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMaintenancePriorityBusiness
    {
        Task<ICollection<MaintenancePriority>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<MaintenancePriority>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<MaintenancePriority> GetAsync(int id);

        Task CreateAsync(MaintenancePriority obj);

        Task UpdateAsync(MaintenancePriority obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
