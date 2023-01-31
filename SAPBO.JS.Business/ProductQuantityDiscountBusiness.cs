using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductQuantityDiscountBusiness : SapB1GenericRepository<ProductQuantityDiscount>, IProductQuantityDiscountBusiness
    {
        public ProductQuantityDiscountBusiness(SapB1Context context, ISapB1AutoMapper<ProductQuantityDiscount> mapper) : base(context, mapper)
        {

        }

        public Task<ICollection<ProductQuantityDiscount>> GetAllAsync(int year, int month)
        {
            return GetAllAsync("GP_WEB_APP_417", new List<dynamic> { year, month });
        }

        public Task<ICollection<ProductQuantityDiscount>> GetAllByProductIdAsync(int year, int month, string productId)
        {
            return GetAllAsync("GP_WEB_APP_418", new List<dynamic> { year, month, productId });
        }

        public Task<ICollection<ProductQuantityDiscount>> GetAllWithIdsAsync(int year, int month, IEnumerable<string> ids)
        {
            return GetAllAsync("GP_WEB_APP_419", new List<dynamic> { year, month, string.Join(",", ids) });
        }
    }
}
