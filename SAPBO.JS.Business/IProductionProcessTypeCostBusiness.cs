using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductionProcessTypeCostBusiness
    {
        Task<ICollection<ProductionProcessTypeCost>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<ProductionProcessTypeCost>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<ProductionProcessTypeCost> GetAsync(int id);

        Task CreateAsync(ProductionProcessTypeCost obj);

        Task UpdateAsync(ProductionProcessTypeCost obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
