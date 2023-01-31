using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class SaleOrderDetailBusiness : SapB1GenericRepository<SaleOrderDetail>, ISaleOrderDetailBusiness
    {
        private readonly IProductBusiness _productRepository;
        private readonly IWarehouseBusiness _warehouseRepository;

        public SaleOrderDetailBusiness(SapB1Context context, ISapB1AutoMapper<SaleOrderDetail> mapper, IProductBusiness productRepository, IWarehouseBusiness warehouseRepository) : base(context, mapper)
        {
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<ICollection<SaleOrderDetail>> GetAllAsync(int saleOrderId)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_409", new List<dynamic> { saleOrderId }));
        }

        public async Task<ICollection<SaleOrderDetail>> GetAllPendingAsync()
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_474"));
        }

        public async Task<ICollection<SaleOrderDetail>> GetAllPendingBySaleEmployeeIdAsync(int saleEmployeeId)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_475", new List<dynamic> { saleEmployeeId }));
        }

        public async Task<ICollection<SaleOrderDetail>> GetAllPendingByBusinessPartnerIdAsync(string businessPartnerId)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_476", new List<dynamic> { businessPartnerId }));
        }

        public async Task<ICollection<SaleOrderDetail>> GetAllWithIdsAsync(IEnumerable<int> saleOrderIds)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_410", new List<dynamic> { string.Join(",", saleOrderIds) }));
        }

        public async Task<SaleOrderDetail> GetAsync(int id, int lineNum)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_408", new List<dynamic> { id, lineNum }));
        }

        public async Task<SaleOrderDetail> SetFullProperties(SaleOrderDetail obj)
        {
            if (obj == null) return null;

            obj.Product = await _productRepository.GetAsync(obj.ProductId);
            obj.Warehouse = await _warehouseRepository.GetAsync(obj.WarehouseId);

            return obj;
        }

        public async Task<ICollection<SaleOrderDetail>> SetFullProperties(ICollection<SaleOrderDetail> objs)
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
