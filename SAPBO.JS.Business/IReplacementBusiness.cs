using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IReplacementBusiness
    {
        Task<ICollection<Replacement>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, string searchText = "");

        Task<ICollection<Replacement>> GetAllWithIdsAsync(IEnumerable<string> ids);

        Task<Replacement> GetAsync(string id);
    }
}
