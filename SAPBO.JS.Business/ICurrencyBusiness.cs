using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ICurrencyBusiness
    {
        Task<ICollection<Currency>> GetAllAsync();

        Task<Currency> GetAsync(string id);
    }
}
