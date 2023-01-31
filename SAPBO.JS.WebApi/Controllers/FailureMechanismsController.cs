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
    public class FailureMechanismsController : ControllerBase
    {
        private readonly IFailureMechanismBusiness repository;
        private readonly ILogger<FailureMechanismsController> logger;

        public FailureMechanismsController(IFailureMechanismBusiness repository, ILogger<FailureMechanismsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetFailureMechanisms")]
        public async Task<ICollection<FailureMechanism>> Get(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return await repository.GetAllAsync(statusType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetFailureMechanism")]
        public async Task<ActionResult<FailureMechanism>> Get(int id)
        {
            try
            {
                var failureMechanism = await repository.GetAsync(id);

                if (failureMechanism == null)
                    return NotFound();

                return failureMechanism;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FailureMechanism failureMechanism)
        {
            try
            {
                await repository.CreateAsync(failureMechanism);

                return new CreatedAtRouteResult("GetFailureMechanism", new { id = failureMechanism.Id }, failureMechanism);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = failureMechanism.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] FailureMechanism failureMechanism)
        {
            try
            {
                if (!id.Equals(failureMechanism.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = failureMechanism.UpdatedBy
                    });

                await repository.UpdateAsync(failureMechanism);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = failureMechanism.UpdatedBy
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
