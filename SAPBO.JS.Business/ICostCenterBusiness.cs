using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ICostCenterBusiness
    {
        Task<ICollection<CostCenter>> GetAllAsync();

        Task<ICollection<CostCenter>> GetAllWithIdsAsync(IEnumerable<string> ids);

        Task<CostCenter> GetAsync(string id);
    }
}
