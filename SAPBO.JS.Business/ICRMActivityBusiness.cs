using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ICRMActivityBusiness
    {
        Task<ICollection<CRMActivity>> GetAllAsync(string businessPartnerId);

        Task<ICollection<CRMActivity>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<ICollection<CRMActivity>> GetAllWithIdsAsync(IEnumerable<string> businessPartnerIds);

        Task<CRMActivity> GetAsync(int id);

        Task<int> GetCountBySaleEmployeeIdAsync(int saleEmployeeId);

        Task CreateAsync(CRMActivity obj);

        Task UpdateAsync(CRMActivity obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
