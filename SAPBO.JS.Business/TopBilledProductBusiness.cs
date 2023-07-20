using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Dto;

namespace SAPBO.JS.Business
{
    public class TopBilledProductBusiness : SapB1GenericRepository<TopBilledProduct>, ITopBilledProductBusiness
    {
        private readonly IProductBusiness _productRepository;

        public TopBilledProductBusiness(SapB1Context context, ISapB1AutoMapper<TopBilledProduct> mapper, IProductBusiness productRepository) : base(context, mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<ICollection<TopBilledProduct>> GetTopBilledProductAsync(int count)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_519", new List<dynamic> { count }));
        }

        public async Task<ICollection<TopBilledProduct>> GetTopBilledProductBySaleEmployeeIdAsync(int saleEmployeeId, int count)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_518", new List<dynamic> { saleEmployeeId, count }));
        }

        public async Task<ICollection<TopBilledProduct>> GetTopBilledProductByBusinessPartnerIdAsync(string businessPartnerId, int count)
{
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_514", new List<dynamic> { businessPartnerId, count }));
        }

        public async Task<ICollection<TopBilledProduct>> SetFullProperties(ICollection<TopBilledProduct> objs)
        {
            if (objs == null || !objs.Any()) return objs;

            //Product
            var productIds = objs.GroupBy(x => x.ProductId).Select(g => g.Key);
            var products = await _productRepository.GetAllWithIdsAsync(productIds);

            foreach (var product in products)
                objs.Where(x => x.ProductId.Equals(product.Id)).ToList().ForEach(x => x.Product = product);

            return objs;
        }
    }
}
