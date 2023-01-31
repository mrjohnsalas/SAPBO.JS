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
    public class ProductClassesController : Controller
    {
        private readonly IProductClassBusiness repository;
        private readonly ILogger<ProductClassesController> logger;

        public ProductClassesController(IProductClassBusiness repository, ILogger<ProductClassesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetProductClasses")]
        public async Task<ICollection<ProductClass>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetProductClass")]
        public async Task<ActionResult<ProductClass>> Get(string id)
        {
            try
            {
                var productClass = await repository.GetAsync(id);

                if (productClass == null)
                    return NotFound();

                return productClass;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
