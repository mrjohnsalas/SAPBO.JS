using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductFormulaProductionProcessBusiness
    {
        Task<ICollection<ProductFormulaProductionProcess>> GetAllAsync(int productFormulaId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<ProductFormulaProductionProcess>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<ProductFormulaProductionProcess>> GetAllByProductFormulaIdAndProductMaterialTypeIdAsync(int productFormulaId, int productMaterialTypeId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ProductFormulaProductionProcess> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(ICollection<ProductFormulaProductionProcess> objs, int productFormulaId);

        Task UpdateAsync(ICollection<ProductFormulaProductionProcess> objs, int productFormulaId);

        Task DeleteAsync(int id);

        Task DeleteAllWithIdsAsync(IEnumerable<int> ids);

        Task DeleteByProductFormulaIdAsync(int productFormulaId);


    }
}
