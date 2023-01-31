using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ISaleQuotationDetailBusiness
    {
        Task<ICollection<SaleQuotationDetail>> GetAllAsync(int saleQuotationId, Enums.ObjectType objectType = Enums.ObjectType.FullHeader);

        Task<ICollection<SaleQuotationDetail>> GetAllWithIdsAsync(IEnumerable<int> saleQuotationIds, Enums.ObjectType objectType = Enums.ObjectType.FullHeader);

        Task<SaleQuotationDetail> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.FullHeader);

        Task CreateAsync(ICollection<SaleQuotationDetail> objs, int saleQuotationId);

        Task UpdateAsync(ICollection<SaleQuotationDetail> objs, int saleQuotationId);

        Task UpdateMiniAsync(int id, int productInkLevelId, int productFormatId, decimal quantity, decimal ancho, decimal alto, decimal panol, int nroCopias, int priceTypeId);

        Task UpdateAsync(ICollection<SaleQuotationDetail> objs);

        Task UpdateStatusBySaleQuotationIdAsync(int saleQuotationId, Enums.StatusType statusType);

        Task DeleteAsync(int id);

        Task DeleteAllWithIdsAsync(IEnumerable<int> ids);

        Task DeleteBySaleQuotationIdAsync(int saleQuotationId);

    }
}
