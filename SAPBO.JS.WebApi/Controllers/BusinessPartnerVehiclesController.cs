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
        RoleNames.Supplier + ", " +
        RoleNames.LogisticsEmployees + ", " +
        RoleNames.PurchaseEmployees)]
    public class BusinessPartnerVehiclesController : Controller
    {
        private readonly IBusinessPartnerVehicleBusiness repository;
        private readonly ILogger<BusinessPartnerVehiclesController> logger;

        public BusinessPartnerVehiclesController(IBusinessPartnerVehicleBusiness repository, ILogger<BusinessPartnerVehiclesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetBusinessPartnerVehicles")]
        public async Task<ICollection<BusinessPartnerVehicle>> Get(string businessPartnerId)
        {
            return await repository.GetAllAsync(businessPartnerId);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetBusinessPartnerVehicle")]
        public async Task<ActionResult<BusinessPartnerVehicle>> Get(int id)
        {
            try
            {
                var vehicle = await repository.GetAsync(id);

                if (vehicle == null)
                    return NotFound();

                return vehicle;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BusinessPartnerVehicle vehicle)
        {
            try
            {
                await repository.CreateAsync(vehicle);

                return new CreatedAtRouteResult("GetBusinessPartnerVehicle", new { id = vehicle.Id }, vehicle);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}"
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] BusinessPartnerVehicle vehicle)
        {
            try
            {
                if (!id.Equals(vehicle.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}"
                    });

                await repository.UpdateAsync(vehicle);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}"
                });
            }
        }
    }
}
