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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin + ", " + RoleNames.MaintenanceEmployees)]
    public class MaintenancePrioritiesController : ControllerBase
    {
        private readonly IMaintenancePriorityBusiness repository;
        private readonly ILogger<MaintenancePrioritiesController> logger;

        public MaintenancePrioritiesController(IMaintenancePriorityBusiness repository, ILogger<MaintenancePrioritiesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetMaintenancePriorities")]
        public async Task<ICollection<MaintenancePriority>> Get(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return await repository.GetAllAsync(statusType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetMaintenancePriority")]
        public async Task<ActionResult<MaintenancePriority>> Get(int id)
        {
            try
            {
                var maintenancePriority = await repository.GetAsync(id);

                if (maintenancePriority == null)
                    return NotFound();

                return maintenancePriority;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MaintenancePriority maintenancePriority)
        {
            try
            {
                await repository.CreateAsync(maintenancePriority);

                return new CreatedAtRouteResult("GetMaintenancePriority", new { id = maintenancePriority.Id }, maintenancePriority);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = maintenancePriority.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MaintenancePriority maintenancePriority)
        {
            try
            {
                if (!id.Equals(maintenancePriority.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = maintenancePriority.UpdatedBy
                    });

                await repository.UpdateAsync(maintenancePriority);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = maintenancePriority.UpdatedBy
                });
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromQuery] string deleteBy)
        {
            try
            {
                await repository.DeleteAsync(id, deleteBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = deleteBy
                });
            }
        }
    }
}
