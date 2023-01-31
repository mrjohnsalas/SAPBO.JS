using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ReplacementBusiness : SapB1GenericRepository<Replacement>, IReplacementBusiness
    {
        public ReplacementBusiness(SapB1Context context, ISapB1AutoMapper<Replacement> mapper) : base(context, mapper)
        {

        }

        public Task<ICollection<Replacement>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, string searchText = "")
        {
            return GetAllAsync("GP_WEB_APP_345", new List<dynamic> { (int)statusType, searchText.Trim() });
        }

        public Task<ICollection<Replacement>> GetAllWithIdsAsync(IEnumerable<string> ids)
        {
            return GetAllAsync("GP_WEB_APP_311", new List<dynamic> { string.Join(",", ids) });
        }

        public Task<Replacement> GetAsync(string id)
        {
            return GetAsync("GP_WEB_APP_129", new List<dynamic> { id });
        }
    }
}
