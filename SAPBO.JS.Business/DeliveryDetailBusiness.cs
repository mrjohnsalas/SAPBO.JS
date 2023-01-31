using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class DeliveryDetailBusiness : SapB1GenericRepository<DeliveryDetail>, IDeliveryDetailBusiness
    {
        private readonly IProductBusiness _productRepository;
        private readonly IWarehouseBusiness _warehouseRepository;

        public DeliveryDetailBusiness(SapB1Context context, ISapB1AutoMapper<DeliveryDetail> mapper, IProductBusiness productRepository, IWarehouseBusiness warehouseRepository) : base(context, mapper)
        {
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<ICollection<DeliveryDetail>> GetAllAsync(int deliveryId)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_461", new List<dynamic> { deliveryId }));
        }

        public async Task<ICollection<DeliveryDetail>> GetAllBySaleOrderIdAndLineNumAsync(int saleOrderId, int lineNum)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_478", new List<dynamic> { saleOrderId, lineNum }));
        }

        public async Task<ICollection<DeliveryDetail>> GetAllBySaleOrderIdAndWithIdsAsync(int saleOrderId, IEnumerable<int> lineNums)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_479", new List<dynamic> { saleOrderId, lineNums }));
        }

        public async Task<ICollection<DeliveryDetail>> GetAllWithIdsAsync(IEnumerable<int> deliveryIds)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_462", new List<dynamic> { string.Join(",", deliveryIds) }));
        }

        public async Task<DeliveryDetail> GetAsync(int deliveryId, int lineNum)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_460", new List<dynamic> { deliveryId, lineNum }));
        }

        private async Task<DeliveryDetail> SetFullProperties(DeliveryDetail obj)
        {
            if (obj == null) return null;

            obj.Product = await _productRepository.GetAsync(obj.ProductId);
            obj.Warehouse = await _warehouseRepository.GetAsync(obj.WarehouseId);

            return obj;
        }

        private async Task<ICollection<DeliveryDetail>> SetFullProperties(ICollection<DeliveryDetail> objs)
        {
            if (objs == null || !objs.Any()) return objs;

            //Product
            var productIds = objs.GroupBy(x => x.ProductId).Select(g => g.Key);
            var products = await _productRepository.GetAllWithIdsAsync(productIds);

            foreach (var product in products)
                objs.Where(x => x.ProductId.Equals(product.Id)).ToList().ForEach(x => x.Product = product);

            //Warehouse
            var warehouseIds = objs.GroupBy(x => x.WarehouseId).Select(g => g.Key);
            var warehouses = await _warehouseRepository.GetAllWithIdsAsync(warehouseIds);

            foreach (var warehouse in warehouses)
                objs.Where(x => x.WarehouseId.Equals(warehouse.Id)).ToList().ForEach(x => x.Warehouse = warehouse);

            return objs;
        }
    }
}
