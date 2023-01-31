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
    public class UnitsOfMeasurementController : Controller
    {
        private readonly IUnitOfMeasurementBusiness repository;
        private readonly ILogger<UnitsOfMeasurementController> logger;

        public UnitsOfMeasurementController(IUnitOfMeasurementBusiness repository, ILogger<UnitsOfMeasurementController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetUnitOfMeasurements")]
        public async Task<ICollection<UnitOfMeasurement>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetUnitOfMeasurement")]
        public async Task<ActionResult<UnitOfMeasurement>> Get(string id)
        {
            try
            {
                var failureType = await repository.GetAsync(id);

                if (failureType == null)
                    return NotFound();

                return failureType;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
