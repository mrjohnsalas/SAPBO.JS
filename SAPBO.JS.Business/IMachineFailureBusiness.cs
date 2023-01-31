using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMachineFailureBusiness
    {
        Task<ICollection<MachineFailure>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<MachineFailure>> GetAllByProductionMachineIdAsync(int productionMachineId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<MachineFailure>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<MachineFailure>> GetAllByMaintenanceWorkOrderIdAsync(int maintenanceWorkOrderId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<MachineFailure> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(MachineFailure obj);

        Task UpdateAsync(MachineFailure obj);

        Task UpdateByMaintenanceWorkOrderIdAsync(List<int> objs, string userId, int parentId);

        Task DeleteAsync(int id, string deleteBy);

        Task DeleteByMaintenanceWorkOrderIdAsync(int maintenanceWorkOrderId, string userId);
    }
}
