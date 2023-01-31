using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMaintenanceProgramBusiness
    {
        Task<ICollection<MaintenanceProgram>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<MaintenanceProgram> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(MaintenanceProgram obj);

        Task UpdateAsync(MaintenanceProgram obj);

        Task DeleteAsync(int id, string deleteBy);

        Task InitAsync(int id, string updatedBy);

        Task PauseAsync(int id, string updatedBy);
    }
}
