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
    public class FailureSeveritiesController : ControllerBase
    {
        private readonly IFailureSeverityBusiness repository;
        private readonly ILogger<FailureSeveritiesController> logger;

        public FailureSeveritiesController(IFailureSeverityBusiness repository, ILogger<FailureSeveritiesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetFailureSeverities")]
        public async Task<ICollection<FailureSeverity>> Get(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return await repository.GetAllAsync(statusType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetFailureSeverity")]
        public async Task<ActionResult<FailureSeverity>> Get(int id)
        {
            try
            {
                var failureSeverity = await repository.GetAsync(id);

                if (failureSeverity == null)
                    return NotFound();

                return failureSeverity;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FailureSeverity failureSeverity)
        {
            try
            {
                await repository.CreateAsync(failureSeverity);

                return new CreatedAtRouteResult("GetFailureSeverity", new { id = failureSeverity.Id }, failureSeverity);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = failureSeverity.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] FailureSeverity failureSeverity)
        {
            try
            {
                if (!id.Equals(failureSeverity.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = failureSeverity.UpdatedBy
                    });

                await repository.UpdateAsync(failureSeverity);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = failureSeverity.UpdatedBy
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
