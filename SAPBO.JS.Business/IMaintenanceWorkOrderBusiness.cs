using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IMaintenanceWorkOrderBusiness
    {
        Task<ICollection<MaintenanceWorkOrder>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<MaintenanceWorkOrder> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(MaintenanceWorkOrder obj);

        Task UpdateAsync(MaintenanceWorkOrder obj);

        Task DeleteAsync(int id, string deleteBy);

        Task InitAsync(int id, string updatedBy);

        Task EndAsync(int id, string updatedBy);
    }
}
