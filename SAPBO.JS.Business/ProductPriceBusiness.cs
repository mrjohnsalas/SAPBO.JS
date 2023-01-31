using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductPriceBusiness : SapB1GenericRepository<ProductPrice>, IProductPriceBusiness
    {
        public ProductPriceBusiness(SapB1Context context, ISapB1AutoMapper<ProductPrice> mapper) : base(context, mapper)
        {

        }

        public Task<ProductPrice> GetAsync(string businessPartnerId, string productId, string currencyId, decimal quantity, DateTime rateDate, int saleEmployeeId = 0)
        {
            return GetAsync("GP_WEB_APP_021", new List<dynamic> { businessPartnerId, productId, currencyId, quantity, rateDate, saleEmployeeId });
        }
    }
}
