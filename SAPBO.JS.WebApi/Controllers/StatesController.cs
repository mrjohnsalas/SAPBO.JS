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
    public class StatesController : ControllerBase
    {
        private readonly IStateBusiness repository;
        private readonly ILogger<StatesController> logger;

        public StatesController(IStateBusiness repository, ILogger<StatesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetStates")]
        public async Task<ICollection<State>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetState")]
        public async Task<ActionResult<State>> Get(string id)
        {
            try
            {
                var state = await repository.GetAsync(id);

                if (state == null)
                    return NotFound();

                return state;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values
        [HttpGet("GetStatesByCountryId/{countryId}", Name = "GetStatesByCountryId")]
        public async Task<ICollection<State>> GetByCountryId(string countryId)
        {
            return await repository.GetAllByCountryIdAsync(countryId);
        }
    }
}
