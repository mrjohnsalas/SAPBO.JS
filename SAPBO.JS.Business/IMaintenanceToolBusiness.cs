using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMaintenanceToolBusiness
    {
        Task<ICollection<MaintenanceTool>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, string searchText = "");

        Task<ICollection<MaintenanceTool>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<MaintenanceTool> GetAsync(int id);

        Task CreateAsync(MaintenanceTool obj);

        Task UpdateAsync(MaintenanceTool obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
