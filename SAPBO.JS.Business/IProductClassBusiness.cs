using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductClassBusiness
    {
        Task<ICollection<ProductClass>> GetAllAsync();

        Task<ICollection<ProductClass>> GetAllWithIdsAsync(IEnumerable<string> ids);

        Task<ProductClass> GetAsync(string id);
    }
}
