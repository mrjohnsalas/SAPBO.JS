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
    public class CountriesController : ControllerBase
    {
        private readonly ICountryBusiness repository;
        private readonly ILogger<CountriesController> logger;

        public CountriesController(ICountryBusiness repository, ILogger<CountriesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetCountries")]
        public async Task<ICollection<Country>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetCountry")]
        public async Task<ActionResult<Country>> Get(string id)
        {
            try
            {
                var country = await repository.GetAsync(id);

                if (country == null)
                    return NotFound();

                return country;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
