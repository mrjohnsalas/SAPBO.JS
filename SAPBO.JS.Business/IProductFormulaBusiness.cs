using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductFormulaBusiness
    {
        Task<ICollection<ProductFormula>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<ProductFormula>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ProductFormula> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(ProductFormula obj);

        Task UpdateAsync(ProductFormula obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
