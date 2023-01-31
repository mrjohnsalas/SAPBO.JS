using Microsoft.Extensions.Caching.Memory;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class WarehouseBusiness : SapB1GenericRepository<Warehouse>, IWarehouseBusiness
    {
        private const string _cacheName = "Warehouses";
        private readonly IMemoryCache _memoryCache;

        public WarehouseBusiness(SapB1Context context, ISapB1AutoMapper<Warehouse> mapper, IMemoryCache memoryCache) : base(context, mapper)
        {
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<Warehouse>> GetCache()
        {
            ICollection<Warehouse> objs = null;

            if (!_memoryCache.TryGetValue(_memoryCache, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_030");
                _memoryCache.Set(_cacheName, objs, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(24)));
            }

            return objs;
        }

        public async Task<ICollection<Warehouse>> GetAllAsync()
        {
            var objs = await GetCache();

            return objs;

            //return GetAllAsync("GP_WEB_APP_030");
        }

        public async Task<ICollection<Warehouse>> GetAllWithIdsAsync(IEnumerable<string> ids)
        {
            var objs = await GetCache();

            return objs.Where(x => ids.Any(y => y.Equals(x.Id))).ToList();

            //return GetAllAsync("GP_WEB_APP_388", new List<dynamic> { string.Join(",", ids) });
        }

        public async Task<Warehouse> GetAsync(string id)
        {
            var objs = await GetCache();

            return objs.FirstOrDefault(x => x.Id.Equals(id));

            //return GetAsync("GP_WEB_APP_031", new List<dynamic> { id });
        }
    }
}
