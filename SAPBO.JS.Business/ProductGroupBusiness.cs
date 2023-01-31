using Microsoft.Extensions.Caching.Memory;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductGroupBusiness : SapB1GenericRepository<ProductGroup>, IProductGroupBusiness
    {
        private const string _cacheName = "ProductGroups";

        private readonly IMemoryCache _memoryCache;

        public ProductGroupBusiness(SapB1Context context, ISapB1AutoMapper<ProductGroup> mapper, IMemoryCache memoryCache) : base(context, mapper)
        {
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<ProductGroup>> GetCache()
        {
            ICollection<ProductGroup> objs = null;

            if (!_memoryCache.TryGetValue(_cacheName, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_016");
                _memoryCache.Set(_cacheName, objs, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(6)));
            }

            return objs;
        }

        public async Task<ICollection<ProductGroup>> GetAllAsync()
        {
            var objs = await GetCache();

            return objs;

            //return GetAllAsync("GP_WEB_APP_016");
        }

        public async Task<ICollection<ProductGroup>> GetAllByProductSuperGroupIdAsync(string productSuperGroupId)
        {
            var objs = await GetCache();

            if (productSuperGroupId != "")
                objs = objs.Where(x => x.ProductSuperGroupId == productSuperGroupId).ToList();

            return objs;

            //return GetAllAsync("GP_WEB_APP_017", new List<dynamic> { productSuperGroupId });
        }

        public async Task<ICollection<ProductGroup>> GetAllWithIdsAsync(IEnumerable<string> ids)
        {
            var objs = await GetCache();

            return objs.Where(x => ids.Any(y => y.Equals(x.Id))).ToList();

            //return GetAllAsync("GP_WEB_APP_387", new List<dynamic> { string.Join(",", ids) });
        }

        public async Task<ProductGroup> GetAsync(string id)
        {
            var objs = await GetCache();

            return objs.FirstOrDefault(x => x.Id.Equals(id));

            //return GetAsync("GP_WEB_APP_018", new List<dynamic> { id });
        }
    }
}
