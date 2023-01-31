using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class PurchaseOrderDetailBusiness : SapB1GenericRepository<PurchaseOrderDetail>, IPurchaseOrderDetailBusiness
    {
        private readonly IProductBusiness _productRepository;
        private readonly IWarehouseBusiness _warehouseRepository;

        public PurchaseOrderDetailBusiness(SapB1Context context, ISapB1AutoMapper<PurchaseOrderDetail> mapper, IProductBusiness productRepository, IWarehouseBusiness warehouseRepository) : base(context, mapper)
        {
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<ICollection<PurchaseOrderDetail>> GetAllAsync(int purchaseOrderId)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_397", new List<dynamic> { purchaseOrderId }));
        }

        public async Task<ICollection<PurchaseOrderDetail>> GetAllWithIdsAsync(IEnumerable<int> purchaseOrderIds)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_403", new List<dynamic> { string.Join(",", purchaseOrderIds) }));
        }

        public async Task<PurchaseOrderDetail> GetAsync(int id, int lineNum)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_396", new List<dynamic> { id, lineNum }));
        }

        public async Task<PurchaseOrderDetail> SetFullProperties(PurchaseOrderDetail obj)
        {
            if (obj == null) return null;

            obj.Product = await _productRepository.GetAsync(obj.ProductId, Enums.ObjectType.Only);
            obj.Warehouse = await _warehouseRepository.GetAsync(obj.WarehouseId);

            return obj;
        }

        public async Task<ICollection<PurchaseOrderDetail>> SetFullProperties(ICollection<PurchaseOrderDetail> objs)
        {
            if (objs == null || !objs.Any()) return objs;

            //Product
            var productIds = objs.GroupBy(x => x.ProductId).Select(g => g.Key);
            var products = await _productRepository.GetAllWithIdsAsync(productIds, Enums.ObjectType.Only);

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
