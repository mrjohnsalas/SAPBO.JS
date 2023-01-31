using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductPriceBusiness
    {
        Task<ProductPrice> GetAsync(string businessPartnerId, string productId, string currencyId, decimal quantity, DateTime rateDate, int saleEmployeeId = 0);
    }
}
