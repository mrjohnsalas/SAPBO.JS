using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class SaleOpportunityLossReasonBusiness : SapB1GenericRepository<SaleOpportunityLossReason>, ISaleOpportunityLossReasonBusiness
    {
        private readonly IOpportunityLossReasonBusiness _opportunityLossReasonRepository;

        public SaleOpportunityLossReasonBusiness(SapB1Context context, ISapB1AutoMapper<SaleOpportunityLossReason> mapper, IOpportunityLossReasonBusiness opportunityLossReasonRepository) : base(context, mapper)
        {
            _opportunityLossReasonRepository = opportunityLossReasonRepository;
        }

        public async Task<ICollection<SaleOpportunityLossReason>> GetAllAsync(int saleOpportunityId)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_372", new List<dynamic> { saleOpportunityId }));
        }

        public async Task<ICollection<SaleOpportunityLossReason>> GetAllWithIdsAsync(IEnumerable<int> saleOpportunityIds)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_373", new List<dynamic> { string.Join(",", saleOpportunityIds) }));
        }

        public async Task<SaleOpportunityLossReason> GetAsync(int saleOpportunityId, int id)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_374", new List<dynamic> { saleOpportunityId, id }));
        }

        public async Task<SaleOpportunityLossReason> SetFullProperties(SaleOpportunityLossReason obj)
        {
            if (obj == null) return obj;

            obj.LossReason = await _opportunityLossReasonRepository.GetAsync(obj.LossReasonId);

            return obj;
        }

        public async Task<ICollection<SaleOpportunityLossReason>> SetFullProperties(ICollection<SaleOpportunityLossReason> objs)
        {
            if (objs == null || !objs.Any()) return objs;

            var reasonIds = objs.GroupBy(x => x.LossReasonId).Select(g => g.Key);
            var reasons = await _opportunityLossReasonRepository.GetAllWithIdsAsync(reasonIds);

            foreach (var reason in reasons)
                objs.Where(x => x.LossReasonId.Equals(reason.Id)).ToList().ForEach(x => x.LossReason = reason);

            return objs;
        }
    }
}
