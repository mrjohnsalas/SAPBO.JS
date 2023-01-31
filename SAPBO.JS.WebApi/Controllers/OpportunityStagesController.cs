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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin + ", " + RoleNames.SalesEmployees)]
    public class OpportunityStagesController : Controller
    {
        private readonly IOpportunityStageBusiness repository;
        private readonly ILogger<OpportunityStagesController> logger;

        public OpportunityStagesController(IOpportunityStageBusiness repository, ILogger<OpportunityStagesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetOpportunityStages")]
        public async Task<ICollection<OpportunityStage>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetOpportunityStage")]
        public async Task<ActionResult<OpportunityStage>> Get(int id)
        {
            try
            {
                var stage = await repository.GetAsync(id);

                if (stage == null)
                    return NotFound();

                return stage;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
