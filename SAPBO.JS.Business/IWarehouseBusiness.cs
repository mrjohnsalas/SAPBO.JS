using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IWarehouseBusiness
    {
        Task<ICollection<Warehouse>> GetAllAsync();

        Task<ICollection<Warehouse>> GetAllWithIdsAsync(IEnumerable<string> ids);

        Task<Warehouse> GetAsync(string id);
    }
}
