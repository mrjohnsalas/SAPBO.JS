using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ISaleOpportunityBusiness
    {
        Task<ICollection<SaleOpportunity>> GetAllAsync(string businessPartnerId);

        Task<ICollection<SaleOpportunity>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<ICollection<SaleOpportunity>> GetAllWithIdsAsync(IEnumerable<string> businessPartnerIds);

        Task<SaleOpportunity> GetAsync(int id);

        Task<int> GetCountBySaleEmployeeIdAsync(int saleEmployeeId);

        Task CreateAsync(SaleOpportunity obj);

        Task UpdateAsync(SaleOpportunity obj);

        Task DeleteAsync(int id, string deleteBy);

        Task WonAsync(int id, string updateBy);

        Task LostAsync(int id, List<SaleOpportunityLossReason> lossReasons, string updateBy);
    }
}
