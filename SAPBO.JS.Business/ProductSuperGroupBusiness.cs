using Microsoft.Extensions.Caching.Memory;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductSuperGroupBusiness : SapB1GenericRepository<ProductSuperGroup>, IProductSuperGroupBusiness
    {
        private const string _cacheName = "ProductSuperGroups";

        private readonly IMemoryCache _memoryCache;

        public ProductSuperGroupBusiness(SapB1Context context, ISapB1AutoMapper<ProductSuperGroup> mapper, IMemoryCache memoryCache) : base(context, mapper)
        {
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<ProductSuperGroup>> GetCache()
        {
            ICollection<ProductSuperGroup> objs = null;

            if (!_memoryCache.TryGetValue(_cacheName, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_022");
                _memoryCache.Set(_cacheName, objs, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(24)));
            }

            return objs;
        }

        public async Task<ICollection<ProductSuperGroup>> GetAllAsync()
        {
            var objs = await GetCache();

            return objs;

            //return GetAllAsync("GP_WEB_APP_022");
        }

        public async Task<ICollection<ProductSuperGroup>> GetAllWithIdsAsync(IEnumerable<string> ids)
        {
            var objs = await GetCache();

            return objs.Where(x => ids.Any(y => y.Equals(x.Id))).ToList();

            //return GetAllAsync("GP_WEB_APP_385", new List<dynamic> { string.Join(",", ids) });
        }

        public async Task<ProductSuperGroup> GetAsync(string id)
        {
            var objs = await GetCache();

            return objs.FirstOrDefault(x => x.Id.Equals(id));

            //return GetAsync("GP_WEB_APP_023", new List<dynamic> { id });
        }
    }
}
