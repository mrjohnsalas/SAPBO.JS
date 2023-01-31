using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class SaleOpportunityStageBusiness : SapB1GenericRepository<SaleOpportunityStage>, ISaleOpportunityStageBusiness
    {
        private readonly IOpportunityStageBusiness _opportunityStageRepository;

        public SaleOpportunityStageBusiness(SapB1Context context, ISapB1AutoMapper<SaleOpportunityStage> mapper, IOpportunityStageBusiness opportunityStageRepository) : base(context, mapper)
        {
            _opportunityStageRepository = opportunityStageRepository;
        }

        public async Task<ICollection<SaleOpportunityStage>> GetAllAsync(int saleOpportunityId)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_369", new List<dynamic> { saleOpportunityId }));
        }

        public async Task<ICollection<SaleOpportunityStage>> GetAllWithIdsAsync(IEnumerable<int> saleOpportunityIds)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_370", new List<dynamic> { string.Join(",", saleOpportunityIds) }));
        }

        public async Task<SaleOpportunityStage> GetAsync(int saleOpportunityId, int id)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_371", new List<dynamic> { saleOpportunityId, id }));
        }

        public async Task<SaleOpportunityStage> SetFullProperties(SaleOpportunityStage obj)
        {
            if (obj == null) return obj;

            obj.OpportunityStage = await _opportunityStageRepository.GetAsync(obj.OpportunityStageId);

            return obj;
        }

        public async Task<ICollection<SaleOpportunityStage>> SetFullProperties(ICollection<SaleOpportunityStage> objs)
        {
            if (objs == null || !objs.Any()) return objs;

            var stageIds = objs.GroupBy(x => x.OpportunityStageId).Select(g => g.Key);
            var stages = await _opportunityStageRepository.GetAllWithIdsAsync(stageIds);

            foreach (var stage in stages)
                objs.Where(x => x.OpportunityStageId.Equals(stage.Id)).ToList().ForEach(x => x.OpportunityStage = stage);

            return objs;
        }
    }
}
