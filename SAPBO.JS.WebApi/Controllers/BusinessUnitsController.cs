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
    public class BusinessUnitsController : ControllerBase
    {
        private readonly IBusinessUnitBusiness repository;
        private readonly ILogger<BusinessUnitsController> logger;

        public BusinessUnitsController(IBusinessUnitBusiness repository, ILogger<BusinessUnitsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetBusinessUnits")]
        public async Task<ICollection<BusinessUnit>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetBusinessUnit")]
        public async Task<ActionResult<BusinessUnit>> Get(string id)
        {
            try
            {
                var businessUnit = await repository.GetAsync(id);

                if (businessUnit == null)
                    return NotFound();

                return businessUnit;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
