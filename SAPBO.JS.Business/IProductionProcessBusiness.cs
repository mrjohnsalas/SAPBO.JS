using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductionProcessBusiness
    {
        Task<ICollection<ProductionProcess>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<ProductionProcess>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ProductionProcess> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(ProductionProcess obj);

        Task UpdateAsync(ProductionProcess obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
