using Microsoft.Extensions.Caching.Memory;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductClassBusiness : SapB1GenericRepository<ProductClass>, IProductClassBusiness
    {
        private const string _cacheName = "ProductClasses";

        private readonly IMemoryCache _memoryCache;

        public ProductClassBusiness(SapB1Context context, ISapB1AutoMapper<ProductClass> mapper, IMemoryCache memoryCache) : base(context, mapper)
        {
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<ProductClass>> GetCache()
        {
            ICollection<ProductClass> objs = null;

            if (!_memoryCache.TryGetValue(_cacheName, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_014");
                _memoryCache.Set(_cacheName, objs, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(6)));
            }

            return objs;
        }

        public async Task<ICollection<ProductClass>> GetAllAsync()
        {
            var objs = await GetCache();

            return objs;

            //return GetAllAsync("GP_WEB_APP_014");
        }

        public async Task<ICollection<ProductClass>> GetAllWithIdsAsync(IEnumerable<string> ids)
        {
            var objs = await GetCache();

            return objs.Where(x => ids.Any(y => y.Equals(x.Id))).ToList();

            //return GetAllAsync("GP_WEB_APP_386", new List<dynamic> { string.Join(",", ids) });
        }

        public async Task<ProductClass> GetAsync(string id)
        {
            var objs = await GetCache();

            return objs.FirstOrDefault(x => x.Id.Equals(id));

            //return GetAsync("GP_WEB_APP_015", new List<dynamic> { id });
        }
    }
}
