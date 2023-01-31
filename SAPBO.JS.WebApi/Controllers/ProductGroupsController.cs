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
    public class ProductGroupsController : Controller
    {
        private readonly IProductGroupBusiness repository;
        private readonly ILogger<ProductGroupsController> logger;

        public ProductGroupsController(IProductGroupBusiness repository, ILogger<ProductGroupsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetProductGroups")]
        public async Task<ICollection<ProductGroup>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values
        [HttpGet("{productSuperGroupId}", Name = "GetProductGroupsByProductSuperGroupId")]
        public async Task<ICollection<ProductGroup>> GetByProductSuperGroupId(string productSuperGroupId)
        {
            return await repository.GetAllByProductSuperGroupIdAsync(productSuperGroupId);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetProductGroup")]
        public async Task<ActionResult<ProductGroup>> Get(string id)
        {
            try
            {
                var productGroup = await repository.GetAsync(id);

                if (productGroup == null)
                    return NotFound();

                return productGroup;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
