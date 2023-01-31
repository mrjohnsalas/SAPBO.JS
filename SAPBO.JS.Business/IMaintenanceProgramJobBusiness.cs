using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMaintenanceProgramJobBusiness
    {
        Task<ICollection<MaintenanceProgramJob>> GetAllAsync(int maintenanceProgramId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<MaintenanceProgramJob>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<MaintenanceProgramJob> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(MaintenanceProgramJob obj);

        Task UpdateAsync(MaintenanceProgramJob obj);

        Task DeleteAsync(int id);

        Task DeleteByMaintenanceProgramIdAsync(int maintenanceProgramId);
    }
}
