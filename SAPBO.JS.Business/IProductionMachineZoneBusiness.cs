using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductionMachineZoneBusiness
    {
        Task<ICollection<ProductionMachineZone>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<ProductionMachineZone>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<ICollection<ProductionMachineZone>> GetAllWithProductionMachineIdsAsync(IEnumerable<int> ids);

        Task<ICollection<ProductionMachineZone>> GetAllByProductionMachineIdAsync(int productionMachineId, Enums.StatusType statusType = Enums.StatusType.Activo);

        Task<ProductionMachineZone> GetAsync(int id);

        Task CreateAsync(ICollection<ProductionMachineZone> objs, int productionMachineId);

        Task UpdateAsync(ICollection<ProductionMachineZone> objs, int productionMachineId, string updateBy);

        Task DeleteAllAsync(ICollection<ProductionMachineZone> objs, string deleteBy);
    }
}
