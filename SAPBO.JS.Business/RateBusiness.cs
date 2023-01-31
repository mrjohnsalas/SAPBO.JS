using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class RateBusiness : SapB1GenericRepository<Rate>, IRateBusiness
    {
        public RateBusiness(SapB1Context context, ISapB1AutoMapper<Rate> mapper) : base(context, mapper)
        {

        }

        public Task<ICollection<Rate>> GetAllForSalesAsync()
        {
            return GetAllAsync("GP_WEB_APP_420");
        }

        public Task<Rate> GetByDateAndCurrencyIdAsync(DateTime date, string currencyId)
        {
            return GetAsync("GP_WEB_APP_024", new List<dynamic> { date, currencyId });
        }
    }
}
