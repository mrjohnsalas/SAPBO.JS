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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.All)]
    public class ProductPricesController : Controller
    {
        private readonly IProductPriceBusiness repository;
        private readonly ILogger<ProductPricesController> logger;

        public ProductPricesController(IProductPriceBusiness repository, ILogger<ProductPricesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values/5
        [HttpGet(Name = "GetProductPrice")]
        public async Task<ActionResult<ProductPrice>> Get(string businessPartnerId, string productId, string currencyId, decimal quantity, DateTime rateDate, int saleEmployeeId)
        {
            try
            {
                var productPrice = await repository.GetAsync(businessPartnerId, productId, currencyId, quantity, rateDate, saleEmployeeId);

                if (productPrice == null)
                    return NotFound();

                return productPrice;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
