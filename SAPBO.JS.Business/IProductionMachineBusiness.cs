using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductionMachineBusiness
    {
        Task<ICollection<ProductionMachine>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<ProductionMachine>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ProductionMachine> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(ProductionMachine obj);

        Task UpdateAsync(ProductionMachine obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
