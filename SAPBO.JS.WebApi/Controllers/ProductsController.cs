using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Helper;

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
    public class ProductsController : Controller
    {
        private readonly IProductBusiness repository;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IProductBusiness repository, ILogger<ProductsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetProducts")]
        public async Task<ICollection<Product>> Get(Enums.ObjectType objectType = Enums.ObjectType.FullHeader, string businessPartnerId = "")
        {
            return await repository.GetAllAsync(objectType, businessPartnerId);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> Get(string id, Enums.ObjectType objectType = Enums.ObjectType.Full, string businessPartnerId = "")
        {
            try
            {
                var product = await repository.GetAsync(id, objectType, businessPartnerId);

                if (product == null)
                    return NotFound();

                return product;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
