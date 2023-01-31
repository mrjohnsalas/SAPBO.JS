using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMaintenanceWorkOrderToolBusiness
    {
        Task<ICollection<MaintenanceWorkOrderTool>> GetAllAsync(int maintenanceWorkOrderId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<MaintenanceWorkOrderTool>> GetAllWithIdsAsync(IEnumerable<int> maintenanceWorkOrderIds, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<MaintenanceWorkOrderTool> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(MaintenanceWorkOrderTool obj);

        Task UpdateAsync(MaintenanceWorkOrderTool obj);

        Task DeleteAsync(int id);

        Task DeleteByMaintenanceWorkOrderIdAsync(int maintenanceWorkOrderId);
    }
}
