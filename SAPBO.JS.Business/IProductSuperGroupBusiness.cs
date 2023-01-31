using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductSuperGroupBusiness
    {
        Task<ICollection<ProductSuperGroup>> GetAllAsync();

        Task<ICollection<ProductSuperGroup>> GetAllWithIdsAsync(IEnumerable<string> ids);

        Task<ProductSuperGroup> GetAsync(string id);
    }
}
