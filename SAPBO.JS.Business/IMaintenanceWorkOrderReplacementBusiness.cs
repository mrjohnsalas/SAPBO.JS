using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMaintenanceWorkOrderReplacementBusiness
    {
        Task<ICollection<MaintenanceWorkOrderReplacement>> GetAllAsync(int maintenanceWorkOrderId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<MaintenanceWorkOrderReplacement>> GetAllWithIdsAsync(IEnumerable<int> maintenanceWorkOrderIds, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<MaintenanceWorkOrderReplacement> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(MaintenanceWorkOrderReplacement obj);

        Task UpdateAsync(MaintenanceWorkOrderReplacement obj);

        Task DeleteAsync(int id);

        Task DeleteByMaintenanceWorkOrderIdAsync(int maintenanceWorkOrderId);
    }
}
