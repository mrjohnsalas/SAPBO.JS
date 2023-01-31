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
    public class MaintenanceProgramsController : ControllerBase
    {
        private readonly IMaintenanceProgramBusiness repository;
        private readonly ILogger<MaintenanceProgramsController> logger;

        public MaintenanceProgramsController(IMaintenanceProgramBusiness repository, ILogger<MaintenanceProgramsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetMaintenancePrograms")]
        public async Task<ICollection<MaintenanceProgram>> Get(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(statusType, objectType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetMaintenanceProgram")]
        public async Task<ActionResult<MaintenanceProgram>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var maintenanceProgram = await repository.GetAsync(id, objectType);

                if (maintenanceProgram == null)
                    return NotFound();

                return maintenanceProgram;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MaintenanceProgram maintenanceProgram)
        {
            try
            {
                await repository.CreateAsync(maintenanceProgram);

                return new CreatedAtRouteResult("GetMaintenanceProgram", new { id = maintenanceProgram.Id }, maintenanceProgram);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = maintenanceProgram.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MaintenanceProgram maintenanceProgram)
        {
            try
            {
                if (!id.Equals(maintenanceProgram.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = maintenanceProgram.UpdatedBy
                    });

                await repository.UpdateAsync(maintenanceProgram);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = maintenanceProgram.UpdatedBy
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

        // POST api/values/init/5
        [HttpPost("Init/{id}")]
        public async Task<ActionResult> Init(int id, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.InitAsync(id, updatedBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = updatedBy
                });
            }
        }

        // POST api/values/pause/5
        [HttpPost("Pause/{id}")]
        public async Task<ActionResult> Pause(int id, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.PauseAsync(id, updatedBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = updatedBy
                });
            }
        }
    }
}
