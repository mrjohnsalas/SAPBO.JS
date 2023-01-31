using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductGroupBusiness
    {
        Task<ICollection<ProductGroup>> GetAllAsync();

        Task<ICollection<ProductGroup>> GetAllByProductSuperGroupIdAsync(string productSuperGroupId);

        Task<ICollection<ProductGroup>> GetAllWithIdsAsync(IEnumerable<string> ids);

        Task<ProductGroup> GetAsync(string id);
    }
}
