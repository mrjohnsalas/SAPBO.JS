using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IBusinessUnitBusiness
    {
        Task<ICollection<BusinessUnit>> GetAllAsync();

        Task<ICollection<BusinessUnit>> GetAllWithIdsAsync(IEnumerable<string> ids);

        Task<BusinessUnit> GetAsync(string id);
    }
}
