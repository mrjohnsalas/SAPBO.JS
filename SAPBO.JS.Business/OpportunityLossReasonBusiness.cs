using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class OpportunityLossReasonBusiness : SapB1GenericRepository<OpportunityLossReason>, IOpportunityLossReasonBusiness
    {
        public OpportunityLossReasonBusiness(SapB1Context context, ISapB1AutoMapper<OpportunityLossReason> mapper) : base(context, mapper)
        {

        }

        public Task<ICollection<OpportunityLossReason>> GetAllAsync()
        {
            return GetAllAsync("GP_WEB_APP_363");
        }

        public Task<ICollection<OpportunityLossReason>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            return GetAllAsync("GP_WEB_APP_364", new List<dynamic> { string.Join(",", ids) });
        }

        public Task<OpportunityLossReason> GetAsync(int id)
        {
            return GetAsync("GP_WEB_APP_365", new List<dynamic> { id });
        }
    }
}
