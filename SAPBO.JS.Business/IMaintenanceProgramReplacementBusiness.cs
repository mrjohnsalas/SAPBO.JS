using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMaintenanceProgramReplacementBusiness
    {
        Task<ICollection<MaintenanceProgramReplacement>> GetAllAsync(int maintenanceProgramId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<MaintenanceProgramReplacement>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<MaintenanceProgramReplacement> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(MaintenanceProgramReplacement obj);

        Task UpdateAsync(MaintenanceProgramReplacement obj);

        Task DeleteAsync(int id);

        Task DeleteByMaintenanceProgramIdAsync(int maintenanceProgramId);
    }
}
