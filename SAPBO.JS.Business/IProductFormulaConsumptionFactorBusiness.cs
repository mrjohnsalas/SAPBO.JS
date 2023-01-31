using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductFormulaConsumptionFactorBusiness
    {
        Task<ICollection<ProductFormulaConsumptionFactor>> GetAllAsync(int productFormulaId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<ProductFormulaConsumptionFactor>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<ProductFormulaConsumptionFactor>> GetAllByProductFormulaIdAndProductMaterialTypeIdAsync(int productFormulaId, int productMaterialTypeId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ProductFormulaConsumptionFactor> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(ICollection<ProductFormulaConsumptionFactor> objs, int productFormulaId);

        Task UpdateAsync(ICollection<ProductFormulaConsumptionFactor> objs, int productFormulaId);

        Task DeleteAsync(int id);

        Task DeleteAllWithIdsAsync(IEnumerable<int> ids);

        Task DeleteByProductFormulaIdAsync(int productFormulaId);
    }
}
