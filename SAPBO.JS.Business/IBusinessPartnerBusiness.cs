using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IBusinessPartnerBusiness
    {
        Task<ICollection<BusinessPartner>> GetAllAsync(Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<BusinessPartner>> GetAllWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<BusinessPartner>> GetAllBySaleEmployeeIdAsync(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only, bool includeLeads = false);

        Task<ICollection<BusinessPartner>> GetAllWithTempAsync(Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<BusinessPartner> GetWithTempAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<BusinessPartner>> GetAllWithTempWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<BusinessPartner>> GetAllWithTempBySaleEmployeeIdAsync(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<BusinessPartner> GetAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<BusinessPartner> GetByRUCAsync(string ruc, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<decimal> GetCreditLineAvailableAsync(string businessPartnerId);

        Task<string> GetCreditStatusAsync(string businessPartnerId);

        Task<ICollection<BusinessPartner>> GetProviderAllAsync(Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<BusinessPartner>> GetProviderAllWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<BusinessPartner> GetProviderAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<BusinessPartner>> GetCarrierAllAsync(Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<BusinessPartner>> GetCarrierAllWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<BusinessPartner> GetCarrierAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<BusinessPartner>> GetTempAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<BusinessPartner>> GetTempAllBySaleEmployeeIdAsync(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<BusinessPartner> GetTempAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<BusinessPartner> GetTempByRUCAsync(string ruc, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task CreateAsync(BusinessPartner obj);

        Task UpdateAsync(BusinessPartner obj);

        Task<ICollection<BusinessPartner>> GetTransportAgencyAllAsync(Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<BusinessPartner>> GetTransportAgencyAllWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<BusinessPartner> GetTransportAgencyAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only);
    }
}
