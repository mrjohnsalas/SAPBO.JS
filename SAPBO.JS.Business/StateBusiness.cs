using Microsoft.Extensions.Caching.Memory;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class StateBusiness : SapB1GenericRepository<State>, IStateBusiness
    {
        private const string _cacheName = "States";

        private readonly IMemoryCache _memoryCache;

        public StateBusiness(SapB1Context context, ISapB1AutoMapper<State> mapper, IMemoryCache memoryCache) : base(context, mapper)
        {
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<State>> GetCache()
        {
            ICollection<State> objs = null;

            if (!_memoryCache.TryGetValue(_cacheName, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_355");
                _memoryCache.Set(_cacheName, objs, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(24)));
            }

            return objs;
        }

        public async Task<ICollection<State>> GetAllAsync()
        {
            var objs = await GetCache();

            return objs;

            //return GetAllAsync("GP_WEB_APP_355");
        }

        public async Task<ICollection<State>> GetAllByCountryIdAsync(string countryId)
        {
            var objs = await GetCache();

            objs = objs.Where(x => x.CountryId == countryId).ToList();

            return objs;

            //return GetAllAsync("GP_WEB_APP_356", new List<dynamic> { countryId.Trim() });
        }

        public async Task<ICollection<State>> GetAllWithIdsAsync(IEnumerable<string> ids)
        {
            var objs = await GetCache();

            return objs.Where(x => ids.Any(y => y.Equals(x.Id))).ToList();

            //return GetAllAsync("GP_WEB_APP_354", new List<dynamic> { string.Join(",", ids) });
        }

        public async Task<State> GetAsync(string id)
        {
            var objs = await GetCache();

            return objs.FirstOrDefault(x => x.Id.Equals(id));

            //return GetAsync("GP_WEB_APP_353", new List<dynamic> { id });
        }
    }
}
