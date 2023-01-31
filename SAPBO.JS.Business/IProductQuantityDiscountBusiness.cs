using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductQuantityDiscountBusiness
    {
        Task<ICollection<ProductQuantityDiscount>> GetAllAsync(int year, int month);

        Task<ICollection<ProductQuantityDiscount>> GetAllByProductIdAsync(int year, int month, string productId);

        Task<ICollection<ProductQuantityDiscount>> GetAllWithIdsAsync(int year, int month, IEnumerable<string> ids);
    }
}
