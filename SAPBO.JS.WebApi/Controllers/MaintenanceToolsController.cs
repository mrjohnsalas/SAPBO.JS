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
    public class MaintenanceToolsController : ControllerBase
    {
        private readonly IMaintenanceToolBusiness repository;
        private readonly ILogger<MaintenanceToolsController> logger;

        public MaintenanceToolsController(IMaintenanceToolBusiness repository, ILogger<MaintenanceToolsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetMaintenanceTools")]
        public async Task<ICollection<MaintenanceTool>> Get(Enums.StatusType statusType = Enums.StatusType.Todos, string searchText = "")
        {
            return await repository.GetAllAsync(statusType, searchText);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetMaintenanceTool")]
        public async Task<ActionResult<MaintenanceTool>> Get(int id)
        {
            try
            {
                var maintenanceTool = await repository.GetAsync(id);

                if (maintenanceTool == null)
                    return NotFound();

                return maintenanceTool;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MaintenanceTool maintenanceTool)
        {
            try
            {
                await repository.CreateAsync(maintenanceTool);

                return new CreatedAtRouteResult("GetMaintenanceTool", new { id = maintenanceTool.Id }, maintenanceTool);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = maintenanceTool.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MaintenanceTool maintenanceTool)
        {
            try
            {
                if (!id.Equals(maintenanceTool.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = maintenanceTool.UpdatedBy
                    });

                await repository.UpdateAsync(maintenanceTool);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = maintenanceTool.UpdatedBy
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
