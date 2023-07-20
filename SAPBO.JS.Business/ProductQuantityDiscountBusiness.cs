using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class ProductQuantityDiscountBusiness : SapB1GenericRepository<ProductQuantityDiscount>, IProductQuantityDiscountBusiness
    {
        private readonly IBusinessPartnerBusiness _businessPartnerRepository;
        //private readonly IProductBusiness _productRepository;

        public ProductQuantityDiscountBusiness(SapB1Context context, ISapB1AutoMapper<ProductQuantityDiscount> mapper,
            IBusinessPartnerBusiness businessPartnerRepository) : base(context, mapper)
        {
            //_productRepository = productRepository;
            _businessPartnerRepository = businessPartnerRepository;
        }

        public async Task<ICollection<ProductQuantityDiscount>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_417", new List<dynamic> { year, month }), objectType);
        }

        public async Task<ICollection<ProductQuantityDiscount>> GetAllBySaleEmployeeIdAsync(int saleEmployeeId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_507", new List<dynamic> { year, month }), objectType);
        }

        public async Task<ICollection<ProductQuantityDiscount>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_508", new List<dynamic> { year, month }), objectType);
        }

        public async Task<ICollection<ProductQuantityDiscount>> GetAllByProductIdAsync(int year, int month, string productId, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_418", new List<dynamic> { year, month, productId }), objectType);
        }

        public async Task<ICollection<ProductQuantityDiscount>> GetAllBySaleEmployeeIdAndProductIdAsync(int saleEmployeeId, int year, int month, string productId, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_509", new List<dynamic> { year, month, productId }), objectType);
        }

        public async Task<ICollection<ProductQuantityDiscount>> GetAllByBusinessPartnerIdAndProductIdAsync(string businessPartnerId, int year, int month, string productId, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_510", new List<dynamic> { year, month, productId }), objectType);
        }

        public async Task<ICollection<ProductQuantityDiscount>> GetAllWithIdsAsync(int year, int month, IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_419", new List<dynamic> { year, month, string.Join(",", ids) }), objectType);
        }

        public async Task<ICollection<ProductQuantityDiscount>> GetAllBySaleEmployeeIdWithIdsAsync(int saleEmployeeId, int year, int month, IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_511", new List<dynamic> { year, month, string.Join(",", ids) }), objectType);
        }

        public async Task<ICollection<ProductQuantityDiscount>> GetAllByBusinessPartnerIdWithIdsAsync(string businessPartnerId, int year, int month, IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_512", new List<dynamic> { year, month, string.Join(",", ids) }), objectType);
        }

        public async Task<ProductQuantityDiscount> SetFullProperties(ProductQuantityDiscount obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                //obj.Product = await _productRepository.GetAsync(obj.ProductId, Enums.ObjectType.Only);

                if (!string.IsNullOrEmpty(obj.BusinessPartnerId))
                    obj.BusinessPartner = await _businessPartnerRepository.GetAsync(obj.BusinessPartnerId, Enums.ObjectType.Only);
            }

            return obj;
        }

        public async Task<ICollection<ProductQuantityDiscount>> SetFullProperties(ICollection<ProductQuantityDiscount> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                ////Product
                //var productIds = objs.GroupBy(x => x.ProductId).Select(g => g.Key);
                //var products = await _productRepository.GetAllAsync(Enums.ObjectType.Only);

                //foreach (var product in products)
                //    objs.Where(x => x.ProductId.Equals(product.Id)).ToList().ForEach(x => x.Product = product);

                //BusinessPartner
                var businessPartnerIds = objs.Where(x => !string.IsNullOrEmpty(x.BusinessPartnerId)).GroupBy(x => x.BusinessPartnerId).Select(g => g.Key);
                var businessPartners = await _businessPartnerRepository.GetAllWithIdsAsync(businessPartnerIds);

                foreach (var businessPartner in businessPartners)
                    objs.Where(x => x.BusinessPartnerId.Equals(businessPartner.Id)).ToList().ForEach(x => x.BusinessPartner = businessPartner);
            }

            return objs;
        }
    }
}
