using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMaintenanceWorkOrderEmployeeBusiness
    {
        Task<ICollection<MaintenanceWorkOrderEmployee>> GetAllAsync(int maintenanceWorkOrderId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<MaintenanceWorkOrderEmployee>> GetAllWithIdsAsync(IEnumerable<int> maintenanceWorkOrderIds, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<MaintenanceWorkOrderEmployee> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(MaintenanceWorkOrderEmployee obj);

        Task UpdateAsync(MaintenanceWorkOrderEmployee obj);

        Task DeleteAsync(int id);

        Task DeleteByMaintenanceWorkOrderIdAsync(int maintenanceWorkOrderId);
    }
}
