using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IQuotationAccessoryBusiness
    {
        Task<ICollection<QuotationAccessory>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<QuotationAccessory>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<QuotationAccessory> GetAsync(int id);

        Task CreateAsync(QuotationAccessory obj);

        Task UpdateAsync(QuotationAccessory obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
