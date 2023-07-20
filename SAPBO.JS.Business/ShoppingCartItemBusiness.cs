using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ShoppingCartItemBusiness : SapB1GenericRepository<ShoppingCartItem>, IShoppingCartItemBusiness
    {
        private const string _tableName = TableNames.ShoppingCart;

        private readonly IProductBusiness _productRepository;
        private readonly IProductPriceBusiness _productPriceRepository;
        private readonly IUserBusiness _userRepository;

        public ShoppingCartItemBusiness(SapB1Context context, ISapB1AutoMapper<ShoppingCartItem> mapper, IProductBusiness productRepository, IProductPriceBusiness productPriceRepository, IUserBusiness userRepository) : base(context, mapper)
        {
            _productRepository = productRepository;
            _productPriceRepository = productPriceRepository;
            _userRepository = userRepository;
        }

        public async Task<ICollection<ShoppingCartItem>> GetAllAsync(string userId, DateTime? rateDate, string businessPartnerId = "", string currencyId = AppDefaultValues.CurrencyIdDolar)
        {
            return await SetFullProperties(await GetAllAsync(userId), rateDate, businessPartnerId, currencyId);
        }

        private Task<ICollection<ShoppingCartItem>> GetAllAsync(string userId)
        {
            return GetAllAsync("GP_WEB_APP_008", new List<dynamic> { userId });
        }

        public async Task<ShoppingCartItem> GetAsync(int id, DateTime? rateDate, string businessPartnerId = "", string currencyId = AppDefaultValues.CurrencyIdDolar)
        {
            return await SetFullProperties(await GetAsync(id), rateDate, businessPartnerId, currencyId);
        }

        public Task<int> GetCountByUserIdAsync(string userId)
        {
            return Task.FromResult(GetValue("GP_WEB_APP_517", "NRO_PD", new List<dynamic> { userId }));
        }

        private Task<ShoppingCartItem> GetAsync(int id)
        {
            return GetAsync("GP_WEB_APP_010", new List<dynamic> { id });
        }

        public async Task CreateAsync(ShoppingCartItem obj)
        {
            await CheckRulesAsync(obj, Enums.ObjectAction.Insert);

            obj.Id = GetNewId();
            await CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(ShoppingCartItem obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            await CheckRulesAsync(obj, Enums.ObjectAction.Update, currentObj);

            //Set obj
            currentObj.ProductDetail = obj.ProductDetail;
            currentObj.Quantity = obj.Quantity;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        public async Task DeleteAsync(int id)
        {
            var obj = await GetAsync(id);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            await DeleteByIdAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task DeleteAllAsync(string userId)
        {
            var items = await GetAllAsync(userId);
            if (items != null && items.Any())
                foreach (var item in items)
                    await DeleteByIdAsync(_tableName, item, item.Id.ToString());
        }

        private async Task CheckRulesAsync(ShoppingCartItem obj, Enums.ObjectAction objectAction, ShoppingCartItem currentObj = null)
        {
            if (objectAction == Enums.ObjectAction.Update)
                if (!obj.UserId.Equals(currentObj.UserId))
                    throw new Exception(AppMessages.CheckUserError);

            if (obj.Quantity <= 0)
                throw new Exception(AppMessages.QuantityGreaterZero);

            obj.Product = await _productRepository.GetAsync(obj.ProductId);
            if (obj.Quantity < obj.Product.CantidadMinimaVenta)
                throw new Exception(string.Format(AppMessages.QuantityGreaterMinQuantity, obj.Product.CantidadMinimaVenta));
            if (obj.Quantity % obj.Product.MultiploCantidad != 0)
                throw new Exception(string.Format(AppMessages.QuantityNotEqualMultipleQuantity, obj.Product.MultiploCantidad));

            var user = await _userRepository.GetUserByEmail(obj.UserId);
            if (user.Roles.Exists(x => x.Name.Equals(RoleNames.Customer)))
            {
                var maxCustomerQuantityValue = Utilities.GetMaxCustomerQuantityValue(obj.Product.MaxCustomerQuantity, obj.Product.MultiploCantidad);
                if (obj.Quantity > maxCustomerQuantityValue)
                    throw new Exception(string.Format(AppMessages.ProductMaxQuantityErrorMessage, obj.Product.MultiploCantidad));
            }
        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_011", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<ShoppingCartItem> SetFullProperties(ShoppingCartItem obj, DateTime? rateDate, string businessPartnerId = "", string currencyId = AppDefaultValues.CurrencyIdDolar)
        {
            if (obj == null) return null;

            obj.Product = await _productRepository.GetAsync(obj.ProductId, Enums.ObjectType.Only);
            obj.Product.ProductPrice = await _productPriceRepository.GetAsync(businessPartnerId, obj.Product.Id, currencyId, obj.Quantity, rateDate ?? DateTime.Now);

            return obj;
        }

        public async Task<ICollection<ShoppingCartItem>> SetFullProperties(ICollection<ShoppingCartItem> objs, DateTime? rateDate, string businessPartnerId = "", string currencyId = AppDefaultValues.CurrencyIdDolar)
        {
            if (objs == null || !objs.Any()) return objs;

            var productIds = objs.GroupBy(x => x.ProductId).Select(g => g.Key);
            var products = await _productRepository.GetAllWithIdsAsync(productIds, Enums.ObjectType.Only);

            foreach (var product in products)
                objs.Where(x => x.ProductId.Equals(product.Id)).ToList().ForEach(x => x.Product = product);

            foreach (var obj in objs)
                obj.Product.ProductPrice = await _productPriceRepository.GetAsync(businessPartnerId, obj.Product.Id, currencyId, obj.Quantity, rateDate ?? DateTime.Now);

            return objs;
        }
    }
}
