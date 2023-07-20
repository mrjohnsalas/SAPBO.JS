using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IRateBusiness
    {
        Task<ICollection<Rate>> GetAllForSalesAsync();

        Task<Rate> GetByDateAndCurrencyIdAsync(DateTime date, string currencyId);

        Task<ICollection<Rate>> GetTodayAsync();
    }
}
