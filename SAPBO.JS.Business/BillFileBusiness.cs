using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class BillFileBusiness : SapB1GenericRepository<BillFile>, IBillFileBusiness
    {
        public BillFileBusiness(SapB1Context context, ISapB1AutoMapper<BillFile> mapper) : base(context, mapper)
        {
            
        }

        public async Task<ICollection<BillFile>> GetAllAsync(int fileId)
        {
            return await GetAllAsync("GP_WEB_APP_493", new List<dynamic> { fileId });
        }

        public async Task<ICollection<BillFile>> GetAllWithIdsAsync(IEnumerable<int> fileIds)
        {
            return await GetAllAsync("GP_WEB_APP_494", new List<dynamic> { string.Join(",", fileIds) });
        }
    }
}
