using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductMaterialTypeBusiness
    {
        Task<ICollection<ProductMaterialType>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<ProductMaterialType>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<ICollection<ProductMaterialType>> GetAllByProductFormulaIdAsync(int productFormulaId);

        Task<ICollection<ProductMaterialType>> GetOnlyPaperAsync();

        Task<ProductMaterialType> GetAsync(int id);

        Task CreateAsync(ProductMaterialType obj);

        Task UpdateAsync(ProductMaterialType obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
