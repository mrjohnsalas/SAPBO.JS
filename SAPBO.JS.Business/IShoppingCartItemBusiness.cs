using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IShoppingCartItemBusiness
    {
        Task<ICollection<ShoppingCartItem>> GetAllAsync(string userId, DateTime? rateDate, string businessPartnerId = "", string currencyId = AppDefaultValues.CurrencyIdDolar);

        Task<ShoppingCartItem> GetAsync(int id, DateTime? rateDate, string businessPartnerId = "", string currencyId = AppDefaultValues.CurrencyIdDolar);

        Task CreateAsync(ShoppingCartItem obj);

        Task UpdateAsync(ShoppingCartItem obj);

        Task DeleteAsync(int id);

        Task DeleteAllAsync(string userId);
    }
}
