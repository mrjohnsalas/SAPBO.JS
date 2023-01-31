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
    public class OpportunityLossReasonsController : Controller
    {
        private readonly IOpportunityLossReasonBusiness repository;
        private readonly ILogger<OpportunityLossReasonsController> logger;

        public OpportunityLossReasonsController(IOpportunityLossReasonBusiness repository, ILogger<OpportunityLossReasonsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetOpportunityLossReasons")]
        public async Task<ICollection<OpportunityLossReason>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetOpportunityLossReason")]
        public async Task<ActionResult<OpportunityLossReason>> Get(int id)
        {
            try
            {
                var lossReason = await repository.GetAsync(id);

                if (lossReason == null)
                    return NotFound();

                return lossReason;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
