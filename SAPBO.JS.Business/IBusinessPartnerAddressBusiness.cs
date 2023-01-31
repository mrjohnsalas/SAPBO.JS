using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IBusinessPartnerAddressBusiness
    {
        Task<ICollection<BusinessPartnerAddress>> GetAllAsync(string businessPartnerId);

        Task<ICollection<BusinessPartnerAddress>> GetAllWithIdsAsync(string businessPartnerId, IEnumerable<string> ids);

        Task<BusinessPartnerAddress> GetAsync(string businessPartnerId, string id);
    }
}
