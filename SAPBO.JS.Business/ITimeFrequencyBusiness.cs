using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface ITimeFrequencyBusiness
    {
        Task<ICollection<TimeFrequency>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos);

        Task<ICollection<TimeFrequency>> GetAllWithIdsAsync(IEnumerable<int> ids);

        Task<TimeFrequency> GetAsync(int id);

        Task CreateAsync(TimeFrequency obj);

        Task UpdateAsync(TimeFrequency obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
