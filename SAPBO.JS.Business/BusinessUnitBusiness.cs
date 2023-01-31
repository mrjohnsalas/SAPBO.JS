using Microsoft.Extensions.Caching.Memory;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class BusinessUnitBusiness : SapB1GenericRepository<BusinessUnit>, IBusinessUnitBusiness
    {
        private readonly IMemoryCache _memoryCache;
        private const string _cacheName = "BusinessUnits";

        public BusinessUnitBusiness(SapB1Context context, ISapB1AutoMapper<BusinessUnit> mapper, IMemoryCache memoryCache) : base(context, mapper)
        {
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<BusinessUnit>> GetCache()
        {
            ICollection<BusinessUnit> objs = null;

            if (!_memoryCache.TryGetValue(_cacheName, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_332");
                _memoryCache.Set(_cacheName, objs, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(24)));
            }

            return objs;
        }

        public async Task<ICollection<BusinessUnit>> GetAllAsync()
        {
            var objs = await GetCache();

            return objs;

            //return GetAllAsync("GP_WEB_APP_332");
        }

        public async Task<ICollection<BusinessUnit>> GetAllWithIdsAsync(IEnumerable<string> ids)
        {
            var objs = await GetCache();

            return objs.Where(x => ids.Any(y => y.Equals(x.Id))).ToList();

            //return GetAllAsync("GP_WEB_APP_333", new List<dynamic> { string.Join(",", ids) });
        }

        public async Task<BusinessUnit> GetAsync(string id)
        {
            var objs = await GetCache();

            return objs.FirstOrDefault(x => x.Id.Equals(id));

            //return GetAsync("GP_WEB_APP_331", new List<dynamic> { id });
        }
    }
}
