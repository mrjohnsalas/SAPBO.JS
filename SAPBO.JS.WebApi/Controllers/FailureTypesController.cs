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
    public class FailureTypesController : ControllerBase
    {
        private readonly IFailureTypeBusiness repository;
        private readonly ILogger<FailureTypesController> logger;

        public FailureTypesController(IFailureTypeBusiness repository, ILogger<FailureTypesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetFailureTypes")]
        public async Task<ICollection<FailureType>> Get(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return await repository.GetAllAsync(statusType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetFailureType")]
        public async Task<ActionResult<FailureType>> Get(int id)
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

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FailureType failureType)
        {
            try
            {
                await repository.CreateAsync(failureType);

                return new CreatedAtRouteResult("GetFailureType", new { id = failureType.Id }, failureType);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = failureType.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] FailureType failureType)
        {
            try
            {
                // if (!ModelState.IsValid) { throw new Exception(Helpers.Utilities.GetErrorsByModel(ModelState)); }

                if (!id.Equals(failureType.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = failureType.UpdatedBy
                    });

                await repository.UpdateAsync(failureType);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = failureType.UpdatedBy
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
