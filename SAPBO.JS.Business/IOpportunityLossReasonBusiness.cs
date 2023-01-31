using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IOpportunityLossReasonBusiness
    {
        Task<ICollection<OpportunityLossReason>> GetAllAsync();

        Task<ICollection<OpportunityLossReason>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<OpportunityLossReason> GetAsync(int id);
    }
}
