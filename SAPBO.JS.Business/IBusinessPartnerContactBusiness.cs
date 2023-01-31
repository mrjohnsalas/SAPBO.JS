using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IBusinessPartnerContactBusiness
    {
        Task<ICollection<BusinessPartnerContact>> GetAllAsync(string businessPartnerId);

        Task<ICollection<BusinessPartnerContact>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<ICollection<BusinessPartnerContact>> GetAllWithIdsAsync(IEnumerable<string> businessPartnerIds);

        Task<BusinessPartnerContact> GetAsync(int id);

        Task CreateAsync(BusinessPartnerContact obj);

        Task UpdateAsync(BusinessPartnerContact obj);
    }
}
