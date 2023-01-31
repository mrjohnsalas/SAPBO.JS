using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IStateBusiness
    {
        Task<ICollection<State>> GetAllAsync();

        Task<ICollection<State>> GetAllByCountryIdAsync(string countryId);

        Task<ICollection<State>> GetAllWithIdsAsync(IEnumerable<string> ids);

        Task<State> GetAsync(string id);
    }
}
