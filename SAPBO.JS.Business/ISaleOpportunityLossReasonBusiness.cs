using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ISaleOpportunityLossReasonBusiness
    {
        Task<ICollection<SaleOpportunityLossReason>> GetAllAsync(int saleOpportunityId);

        Task<ICollection<SaleOpportunityLossReason>> GetAllWithIdsAsync(IEnumerable<int> saleOpportunityIds);

        Task<SaleOpportunityLossReason> GetAsync(int saleOpportunityId, int id);
    }
}
