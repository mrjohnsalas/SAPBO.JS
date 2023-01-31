using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IUnitOfMeasurementBusiness
    {
        Task<ICollection<UnitOfMeasurement>> GetAllAsync();

        Task<ICollection<UnitOfMeasurement>> GetAllWithIdsAsync(IEnumerable<string> ids);

        Task<UnitOfMeasurement> GetAsync(string id);
    }
}
