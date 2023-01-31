using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.WebApi.Controllers
{
    [Route(AppConfiguration.WebApiRoutePath)]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =
        RoleNames.Admin + ", " +
        RoleNames.Manager + ", " +
        RoleNames.Customer + ", " +
        RoleNames.Supplier + ", " +
        RoleNames.SalesEmployees + ", " +
        RoleNames.LogisticsEmployees + ", " +
        RoleNames.PurchaseEmployees + ", " +
        RoleNames.CreditEmployees)]
    public class ProductQuantityDiscountsController : Controller
    {
        private readonly IProductQuantityDiscountBusiness repository;
        private readonly ILogger<ProductQuantityDiscountsController> logger;

        public ProductQuantityDiscountsController(IProductQuantityDiscountBusiness repository, ILogger<ProductQuantityDiscountsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetProductQuantityDiscounts")]
        public async Task<ICollection<ProductQuantityDiscount>> Get(int year, int month)
        {
            return await repository.GetAllAsync(year, month);
        }

        // GET api/values
        [HttpGet(Name = "GetProductQuantityDiscountsByProductId")]
        public async Task<ICollection<ProductQuantityDiscount>> GetByProductId(int year, int month, string productId)
        {
            return await repository.GetAllByProductIdAsync(year, month, productId);
        }
    }
}
