using Microsoft.Extensions.Caching.Memory;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class CurrencyBusiness : SapB1GenericRepository<Currency>, ICurrencyBusiness
    {
        private readonly IMemoryCache _memoryCache;
        private const string _cacheName = "Currencies";

        public CurrencyBusiness(SapB1Context context, ISapB1AutoMapper<Currency> mapper, IMemoryCache memoryCache) : base(context, mapper)
        {
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<Currency>> GetCache()
        {
            ICollection<Currency> objs = null;

            if (!_memoryCache.TryGetValue(_cacheName, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_001");
                _memoryCache.Set(_cacheName, objs, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(24)));
            }

            return objs;
        }

        public async Task<ICollection<Currency>> GetAllAsync()
        {
            var objs = await GetCache();

            return objs;

            //return GetAllAsync("GP_WEB_APP_001");
        }

        public async Task<Currency> GetAsync(string id)
        {
            var objs = await GetCache();

            return objs.FirstOrDefault(x => x.Id.Equals(id));

            //return GetAsync("GP_WEB_APP_002", new List<dynamic> { id });
        }
    }
}
