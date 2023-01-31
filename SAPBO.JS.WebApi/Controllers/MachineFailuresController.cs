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
    public class MachineFailuresController : ControllerBase
    {
        private readonly IMachineFailureBusiness repository;
        private readonly ILogger<MachineFailuresController> logger;

        public MachineFailuresController(IMachineFailureBusiness repository, ILogger<MachineFailuresController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetMachineFailures")]
        public async Task<ICollection<MachineFailure>> Get(int year, int month, int maintenanceWorkOrderId = 0, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return maintenanceWorkOrderId.Equals(0)
            ? await repository.GetAllAsync(year, month, objectType)
            : await repository.GetAllByMaintenanceWorkOrderIdAsync(maintenanceWorkOrderId, objectType);
        }

        // GET api/values
        [HttpGet("GetAllByProductionMachineId/{productionMachineId}", Name = "GetMachineFailuresByProductionMachineId")]
        public async Task<ICollection<MachineFailure>> GetAllByProductionMachineId(int productionMachineId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllByProductionMachineIdAsync(productionMachineId, objectType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetMachineFailure")]
        public async Task<ActionResult<MachineFailure>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var machineFailure = await repository.GetAsync(id, objectType);

                if (machineFailure == null)
                    return NotFound();

                return machineFailure;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MachineFailure machineFailure)
        {
            try
            {
                await repository.CreateAsync(machineFailure);

                return new CreatedAtRouteResult("GetMachineFailure", new { id = machineFailure.Id }, machineFailure);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = machineFailure.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MachineFailure machineFailure)
        {
            try
            {
                if (!id.Equals(machineFailure.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = machineFailure.UpdatedBy
                    });

                await repository.UpdateAsync(machineFailure);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = machineFailure.UpdatedBy
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
