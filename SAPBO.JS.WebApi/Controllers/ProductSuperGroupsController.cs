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
    public class ProductSuperGroupsController : Controller
    {
        private readonly IProductSuperGroupBusiness repository;
        private readonly ILogger<ProductSuperGroupsController> logger;

        public ProductSuperGroupsController(IProductSuperGroupBusiness repository, ILogger<ProductSuperGroupsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetProductSuperGroups")]
        public async Task<ICollection<ProductSuperGroup>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetProductSuperGroup")]
        public async Task<ActionResult<ProductSuperGroup>> Get(string id)
        {
            try
            {
                var productSuperGroup = await repository.GetAsync(id);

                if (productSuperGroup == null)
                    return NotFound();

                return productSuperGroup;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
