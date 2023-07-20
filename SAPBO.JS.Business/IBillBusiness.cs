using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IBillBusiness
    {
        Task<ICollection<Bill>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Bill>> GetAllBySaleEmployeeIdAsync(int saleEmployeeId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Bill>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Bill>> GetAllPendingAsync(Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Bill>> GetAllPendingBySaleEmployeeIdAsync(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Bill>> GetAllPendingByBusinessPartnerIdAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Bill>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<Bill> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);
    }
}
