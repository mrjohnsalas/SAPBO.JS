using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductBusiness : SapB1GenericRepository<Product>, IProductBusiness
    {
        private readonly IProductGroupBusiness _productGroupRepository;
        private readonly IProductSuperGroupBusiness _productSuperGroupRepository;
        private readonly IProductClassBusiness _productClassRepository;
        private readonly IProductPriceBusiness _productPriceRepository;
        private readonly IProductQuantityDiscountBusiness _productQuantityDiscountRepository;
        private readonly IWarehouseBusiness _warehouseRepository;

        public ProductBusiness(SapB1Context context, ISapB1AutoMapper<Product> mapper,
            IProductGroupBusiness productGroupRepository,
            IProductSuperGroupBusiness productSuperGroupRepository,
            IProductClassBusiness productClassRepository,
            IProductPriceBusiness productPriceRepository,
            IProductQuantityDiscountBusiness productQuantityDiscountRepository,
            IWarehouseBusiness warehouseRepository) : base(context, mapper)
        {
            _productGroupRepository = productGroupRepository;
            _productSuperGroupRepository = productSuperGroupRepository;
            _productClassRepository = productClassRepository;
            _productPriceRepository = productPriceRepository;
            _productQuantityDiscountRepository = productQuantityDiscountRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<ICollection<Product>> GetAllAsync(Enums.ObjectType objectType = Enums.ObjectType.FullHeader, string businessPartnerId = "")
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_019", new List<dynamic> { "%" }), objectType, null, businessPartnerId);
        }

        public async Task<ICollection<Product>> GetAllWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.FullHeader, string businessPartnerId = "")
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_393", new List<dynamic> { string.Join(",", ids) }), objectType, null, businessPartnerId);
        }

        public async Task<Product> GetAsync(string id, Enums.ObjectType objectType = Enums.ObjectType.Full, string businessPartnerId = "")
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_020", new List<dynamic> { id }), objectType, null, businessPartnerId);
        }

        //public Task<ProductStock> GetProductStockAsync(string productId)
        //{
        //    return GetAsync("GP_WEB_APP_389", new List<dynamic> { productId });
        //}

        public async Task<Product> SetFullProperties(Product obj, Enums.ObjectType objectType, DateTime? rateDate, string businessPartnerId = "", string currencyId = AppDefaultValues.CurrencyIdDolar, decimal quantity = 1)
        {
            if (obj == null) return null;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.ProductClass = await _productClassRepository.GetAsync(obj.ProductClassId);
                obj.ProductGroup = await _productGroupRepository.GetAsync(obj.ProductGroupId);
                obj.ProductSuperGroup = await _productSuperGroupRepository.GetAsync(obj.ProductSuperGroupId);
                obj.DefaultWarehouse = await _warehouseRepository.GetAsync(obj.DefaultWarehouseId);

                if (objectType == Enums.ObjectType.Full)
                {
                    var systemDate = DateTime.Now;
                    obj.ProductQuantityDiscounts = await _productQuantityDiscountRepository.GetAllByProductIdAsync(systemDate.Year, systemDate.Month, obj.Id);

                    if (!string.IsNullOrEmpty(businessPartnerId))
                        obj.ProductPrice = await _productPriceRepository.GetAsync(businessPartnerId, obj.Id, currencyId, quantity, rateDate ?? DateTime.Now);
                }
            }

            return obj;
        }

        public async Task<ICollection<Product>> SetFullProperties(ICollection<Product> objs, Enums.ObjectType objectType, DateTime? rateDate, string businessPartnerId = "", string currencyId = AppDefaultValues.CurrencyIdDolar, decimal quantity = 1)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                var productClassIds = objs.GroupBy(x => x.ProductClassId).Select(g => g.Key);
                var productClasses = await _productClassRepository.GetAllWithIdsAsync(productClassIds);

                var productGroupIds = objs.GroupBy(x => x.ProductGroupId).Select(g => g.Key);
                var productGroups = await _productGroupRepository.GetAllWithIdsAsync(productGroupIds);

                var productSuperGroupIds = objs.GroupBy(x => x.ProductSuperGroupId).Select(g => g.Key);
                var productSuperGroups = await _productSuperGroupRepository.GetAllWithIdsAsync(productSuperGroupIds);

                var warehouseIds = objs.GroupBy(x => x.DefaultWarehouseId).Select(g => g.Key);
                var warehouses = await _warehouseRepository.GetAllWithIdsAsync(warehouseIds);

                foreach (var productClass in productClasses)
                    objs.Where(x => x.ProductClassId.Equals(productClass.Id)).ToList().ForEach(x => x.ProductClass = productClass);

                foreach (var productGroup in productGroups)
                    objs.Where(x => x.ProductGroupId.Equals(productGroup.Id)).ToList().ForEach(x => x.ProductGroup = productGroup);

                foreach (var productSuperGroup in productSuperGroups)
                    objs.Where(x => x.ProductSuperGroupId.Equals(productSuperGroup.Id)).ToList().ForEach(x => x.ProductSuperGroup = productSuperGroup);

                foreach (var warehouse in warehouses)
                    objs.Where(x => x.DefaultWarehouseId.Equals(warehouse.Id)).ToList().ForEach(x => x.DefaultWarehouse = warehouse);

                if (objectType == Enums.ObjectType.Full)
                {
                    var systemDate = DateTime.Now;

                    foreach (var obj in objs)
                        obj.ProductQuantityDiscounts = await _productQuantityDiscountRepository.GetAllByProductIdAsync(systemDate.Year, systemDate.Month, obj.Id);

                    if (!string.IsNullOrEmpty(businessPartnerId))
                        foreach (var obj in objs)
                            obj.ProductPrice = await _productPriceRepository.GetAsync(businessPartnerId, obj.Id, currencyId, quantity, rateDate ?? DateTime.Now);
                }
            }

            return objs;
        }
    }
}
