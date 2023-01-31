using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IEmployeeBusiness
    {
        Task<ICollection<Employee>> GetAllAsync(string businessUnitId = "", Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Employee>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Employee>> GetAllSupersAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Employee>> GetAllSaleEmployeesAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Employee>> GetAllWithSaleEmployeeIdsAsync(IEnumerable<int> saleEmployeeIds, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<ICollection<Employee>> GetAllWithPurchaseEmployeeIdsAsync(IEnumerable<int> purchaseEmployeeIds, Enums.ObjectType objectType = Enums.ObjectType.Only);

        Task<Employee> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<Employee> GetByEmailAsync(string email, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<Employee> GetBySaleEmployeeIdAsync(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<Employee> GetByPurchaseEmployeeIdAsync(int purchaseEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task CreateAsync(Employee obj);

        Task UpdateAsync(Employee obj);

        Task DeleteAsync(int id, string deleteBy);
    }
}
