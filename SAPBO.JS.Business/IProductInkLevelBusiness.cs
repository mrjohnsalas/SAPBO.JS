using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductInkLevelBusiness
    {
        Task<ICollection<ProductInkLevel>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<ProductInkLevel>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<ProductInkLevel> GetAsync(int id);

        Task CreateAsync(ProductInkLevel obj);

        Task UpdateAsync(ProductInkLevel obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
