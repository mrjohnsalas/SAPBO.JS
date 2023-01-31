using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductMaterialBusiness
    {
        Task<ICollection<ProductMaterial>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<ProductMaterial>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<ProductMaterial>> GetAllByProductMaterialTypeIdAndGramajeIdAsync(int productFormulaId, int gramajeId, int copies, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ProductMaterial> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(ProductMaterial obj);

        Task UpdateAsync(ProductMaterial obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
