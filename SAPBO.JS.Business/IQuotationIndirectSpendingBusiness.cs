using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IQuotationIndirectSpendingBusiness
    {
        Task<ICollection<QuotationIndirectSpending>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<QuotationIndirectSpending>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<QuotationIndirectSpending> GetAsync(int id);

        Task CreateAsync(QuotationIndirectSpending obj);

        Task UpdateAsync(QuotationIndirectSpending obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
