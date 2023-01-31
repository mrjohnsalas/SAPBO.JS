using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ICountryBusiness
    {
        Task<ICollection<Country>> GetAllAsync();

        Task<ICollection<Country>> GetAllWithIdsAsync(IEnumerable<string> ids);

        Task<Country> GetAsync(string id);
    }
}
