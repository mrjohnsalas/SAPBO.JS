using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ISaleOpportunityStageBusiness
    {
        Task<ICollection<SaleOpportunityStage>> GetAllAsync(int saleOpportunityId);

        Task<ICollection<SaleOpportunityStage>> GetAllWithIdsAsync(IEnumerable<int> saleOpportunityIds);

        Task<SaleOpportunityStage> GetAsync(int saleOpportunityId, int id);
    }
}
