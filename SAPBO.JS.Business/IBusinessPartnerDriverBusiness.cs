using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IBusinessPartnerDriverBusiness
    {
        Task<ICollection<BusinessPartnerDriver>> GetAllAsync(string businessPartnerId);

        Task<ICollection<BusinessPartnerDriver>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<BusinessPartnerDriver> GetAsync(int id);

        Task<BusinessPartnerDriver> GetByLicenseIdAsync(string licenseId);

        Task CreateAsync(BusinessPartnerDriver obj);

        Task UpdateAsync(BusinessPartnerDriver obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
