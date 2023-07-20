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
        RoleNames.SalesEmployees)]
    public class CRMActivitiesController : ControllerBase
    {
        private readonly ICRMActivityBusiness repository;
        private readonly ILogger<CRMActivitiesController> logger;

        public CRMActivitiesController(ICRMActivityBusiness repository, ILogger<CRMActivitiesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetCRMActivities")]
        public async Task<ICollection<CRMActivity>> Get(string businessPartnerId)
        {
            return await repository.GetAllAsync(businessPartnerId);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetCRMActivity")]
        public async Task<ActionResult<CRMActivity>> Get(int id)
        {
            try
            {
                var activity = await repository.GetAsync(id);

                if (activity == null)
                    return NotFound();

                return activity;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values/5
        [HttpGet("GetCountBySaleEmployeeId/{saleEmployeeId}", Name = "GetCRMActivitiesCountBySaleEmployeeId")]
        public Task<int> GetCountBySaleEmployeeId(int saleEmployeeId)
        {
            return repository.GetCountBySaleEmployeeIdAsync(saleEmployeeId);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CRMActivity activity)
        {
            try
            {
                await repository.CreateAsync(activity);

                return new CreatedAtRouteResult("GetCRMActivity", new { id = activity.Id }, activity);
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
        public async Task<ActionResult> Put(int id, [FromBody] CRMActivity activity)
        {
            try
            {
                if (!id.Equals(activity.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}"
                    });

                await repository.UpdateAsync(activity);

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
