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
    public class WarehousesController : Controller
    {
        private readonly IWarehouseBusiness repository;
        private readonly ILogger<WarehousesController> logger;

        public WarehousesController(IWarehouseBusiness repository, ILogger<WarehousesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetWarehouses")]
        public async Task<ICollection<Warehouse>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetWarehouse")]
        public async Task<ActionResult<Warehouse>> Get(string id)
        {
            try
            {
                var warehouse = await repository.GetAsync(id);

                if (warehouse == null)
                    return NotFound();

                return warehouse;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
