using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public interface IProductQuantityDiscountBusiness
    {
        Task<ICollection<ProductQuantityDiscount>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<ProductQuantityDiscount>> GetAllBySaleEmployeeIdAsync(int saleEmployeeId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<ProductQuantityDiscount>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<ProductQuantityDiscount>> GetAllByProductIdAsync(int year, int month, string productId, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<ProductQuantityDiscount>> GetAllBySaleEmployeeIdAndProductIdAsync(int saleEmployeeId, int year, int month, string productId, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<ProductQuantityDiscount>> GetAllByBusinessPartnerIdAndProductIdAsync(string businessPartnerId, int year, int month, string productId, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<ProductQuantityDiscount>> GetAllWithIdsAsync(int year, int month, IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<ProductQuantityDiscount>> GetAllBySaleEmployeeIdWithIdsAsync(int saleEmployeeId, int year, int month, IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Full);

        Task<ICollection<ProductQuantityDiscount>> GetAllByBusinessPartnerIdWithIdsAsync(string businessPartnerId, int year, int month, IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Full);
    }
}
