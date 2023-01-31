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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin + ", " + RoleNames.MaintenanceEmployees + ", " + RoleNames.SalesEmployees)]
    public class CostCentersController : Controller
    {
        private readonly ICostCenterBusiness repository;
        private readonly ILogger<CostCentersController> logger;

        public CostCentersController(ICostCenterBusiness repository, ILogger<CostCentersController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetCostCenters")]
        public async Task<ICollection<CostCenter>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetCostCenter")]
        public async Task<ActionResult<CostCenter>> Get(string id)
        {
            try
            {
                var costCenter = await repository.GetAsync(id);

                if (costCenter == null)
                    return NotFound();

                return costCenter;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
