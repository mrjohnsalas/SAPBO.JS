using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMaintenanceTypeBusiness
    {
        Task<ICollection<MaintenanceType>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<MaintenanceType>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<MaintenanceType> GetAsync(int id);

        Task CreateAsync(MaintenanceType obj);

        Task UpdateAsync(MaintenanceType obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
