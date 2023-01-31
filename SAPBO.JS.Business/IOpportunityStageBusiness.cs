using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IOpportunityStageBusiness
    {
        Task<ICollection<OpportunityStage>> GetAllAsync();

        Task<ICollection<OpportunityStage>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<OpportunityStage> GetAsync(int id);

        Task<OpportunityStage> GetByStageAsync(Enums.OpportunityStages stage);
    }
}
