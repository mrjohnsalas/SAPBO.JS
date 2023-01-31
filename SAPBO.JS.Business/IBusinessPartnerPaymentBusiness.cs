using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IBusinessPartnerPaymentBusiness
    {
        Task<ICollection<BusinessPartnerPayment>> GetAllAsync(string businessPartnerId);

        Task<BusinessPartnerPayment> GetAsync(int id);

        Task<ICollection<BusinessPartnerPayment>> GetAllWithIdsAsync(IEnumerable<int> ids);
    }
}
