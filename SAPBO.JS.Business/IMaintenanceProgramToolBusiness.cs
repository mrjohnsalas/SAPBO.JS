using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMaintenanceProgramToolBusiness
    {
        Task<ICollection<MaintenanceProgramTool>> GetAllAsync(int maintenanceProgramId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<MaintenanceProgramTool>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<MaintenanceProgramTool> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(MaintenanceProgramTool obj);

        Task UpdateAsync(MaintenanceProgramTool obj);

        Task DeleteAsync(int id);

        Task DeleteByMaintenanceProgramIdAsync(int maintenanceProgramId);
    }
}
