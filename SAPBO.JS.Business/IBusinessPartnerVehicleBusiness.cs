using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IBusinessPartnerVehicleBusiness
    {
        Task<ICollection<BusinessPartnerVehicle>> GetAllAsync(string businessPartnerId);

        Task<ICollection<BusinessPartnerVehicle>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<BusinessPartnerVehicle> GetAsync(int id);

        Task<BusinessPartnerVehicle> GetByPlacaIdAsync(string placa);

        Task CreateAsync(BusinessPartnerVehicle obj);

        Task UpdateAsync(BusinessPartnerVehicle obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
