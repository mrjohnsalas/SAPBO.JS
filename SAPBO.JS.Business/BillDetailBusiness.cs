using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class BillDetailBusiness : SapB1GenericRepository<BillDetail>, IBillDetailBusiness
    {
        private readonly IProductBusiness _productRepository;
        private readonly IWarehouseBusiness _warehouseRepository;

        public BillDetailBusiness(SapB1Context context, ISapB1AutoMapper<BillDetail> mapper, 
            IProductBusiness productRepository, 
            IWarehouseBusiness warehouseRepository) : base(context, mapper)
        {
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<ICollection<BillDetail>> GetAllAsync(int billId)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_495", new List<dynamic> { billId }));
        }

        public async Task<ICollection<BillDetail>> GetAllWithIdsAsync(IEnumerable<int> billIds)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_496", new List<dynamic> { string.Join(",", billIds) }));
        }

        public async Task<BillDetail> SetFullProperties(BillDetail obj)
        {
            if (obj == null) return null;

            obj.Product = await _productRepository.GetAsync(obj.ProductId);
            obj.Warehouse = await _warehouseRepository.GetAsync(obj.WarehouseId);

            return obj;
        }

        public async Task<ICollection<BillDetail>> SetFullProperties(ICollection<BillDetail> objs)
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
