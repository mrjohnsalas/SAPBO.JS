using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductBusiness
    {
        Task<ICollection<Product>> GetAllAsync(Enums.ObjectType objectType = Enums.ObjectType.FullHeader, string businessPartnerId = "");

        Task<ICollection<Product>> GetAllWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.FullHeader, string businessPartnerId = "");

        Task<Product> GetAsync(string id, Enums.ObjectType objectType = Enums.ObjectType.Full, string businessPartnerId = "");
    }
}
