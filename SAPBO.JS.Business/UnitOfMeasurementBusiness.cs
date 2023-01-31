using Microsoft.Extensions.Caching.Memory;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class UnitOfMeasurementBusiness : SapB1GenericRepository<UnitOfMeasurement>, IUnitOfMeasurementBusiness
    {
        private const string _cacheName = "UnitOfMeasurements";
        private readonly IMemoryCache _memoryCache;

        public UnitOfMeasurementBusiness(SapB1Context context, ISapB1AutoMapper<UnitOfMeasurement> mapper, IMemoryCache memoryCache) : base(context, mapper)
        {
            _memoryCache = memoryCache;
        }

        private async Task<ICollection<UnitOfMeasurement>> GetCache()
        {
            ICollection<UnitOfMeasurement> objs = null;

            if (!_memoryCache.TryGetValue(_memoryCache, out objs))
            {
                objs = await GetAllAsync("GP_WEB_APP_187");
                _memoryCache.Set(_memoryCache, objs);
            }

            return objs;
        }

        public async Task<ICollection<UnitOfMeasurement>> GetAllAsync()
        {
            var objs = await GetCache();

            return objs;

            //return GetAllAsync("GP_WEB_APP_187");
        }

        public async Task<ICollection<UnitOfMeasurement>> GetAllWithIdsAsync(IEnumerable<string> ids)
        {
            var objs = await GetCache();

            return objs.Where(x => ids.Any(y => y.Equals(x.Id))).ToList();

            //return GetAllAsync("GP_WEB_APP_431", new List<dynamic> { string.Join(",", ids) });
        }

        public async Task<UnitOfMeasurement> GetAsync(string id)
        {
            var objs = await GetCache();

            return objs.FirstOrDefault(x => x.Id.Equals(id));

            //return GetAsync("GP_WEB_APP_188", new List<dynamic> { id });
        }
    }
}
