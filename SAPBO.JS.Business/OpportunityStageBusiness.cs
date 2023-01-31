using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class OpportunityStageBusiness : SapB1GenericRepository<OpportunityStage>, IOpportunityStageBusiness
    {
        public OpportunityStageBusiness(SapB1Context context, ISapB1AutoMapper<OpportunityStage> mapper) : base(context, mapper)
        {

        }

        public Task<ICollection<OpportunityStage>> GetAllAsync()
        {
            return GetAllAsync("GP_WEB_APP_366");
        }

        public Task<ICollection<OpportunityStage>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            return GetAllAsync("GP_WEB_APP_367", new List<dynamic> { string.Join(",", ids) });
        }

        public Task<OpportunityStage> GetAsync(int id)
        {
            return GetAsync("GP_WEB_APP_368", new List<dynamic> { id });
        }

        public Task<OpportunityStage> GetByStageAsync(Enums.OpportunityStages stage)
        {
            return GetAsync("GP_WEB_APP_379", new List<dynamic> { (int)stage });
        }
    }
}
