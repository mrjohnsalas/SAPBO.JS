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
    public class ReplacementsController : ControllerBase
    {
        private readonly IReplacementBusiness repository;
        private readonly ILogger<ReplacementsController> logger;

        public ReplacementsController(IReplacementBusiness repository, ILogger<ReplacementsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetReplacements")]
        public async Task<ICollection<Replacement>> Get(Enums.StatusType statusType = Enums.StatusType.Todos, string searchText = "")
        {
            return await repository.GetAllAsync(statusType, searchText);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetReplacement")]
        public async Task<ActionResult<Replacement>> Get(string id)
        {
            try
            {
                var replacement = await repository.GetAsync(id);

                if (replacement == null)
                    return NotFound();

                return replacement;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
