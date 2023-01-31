using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductGrammageBusiness
    {
        Task<ICollection<ProductGrammage>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<ProductGrammage>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<ICollection<ProductGrammage>> GetAllByProductMaterialTypeIdAsync(int productMaterialTypeId);

        Task<ICollection<ProductGrammage>> GetAllByProductFormulaIdAndProductMaterialTypeIdAsync(int productFormulaId, int productMaterialTypeId);

        Task<ProductGrammage> GetAsync(int id);

        Task CreateAsync(ProductGrammage obj);

        Task UpdateAsync(ProductGrammage obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
