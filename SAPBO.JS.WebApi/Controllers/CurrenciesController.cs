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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.All)]
    public class CurrenciesController : Controller
    {
        private readonly ICurrencyBusiness repository;
        private readonly ILogger<CurrenciesController> logger;

        public CurrenciesController(ICurrencyBusiness repository, ILogger<CurrenciesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetCurrencies")]
        public async Task<ICollection<Currency>> Get()
        {
            return await repository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetCurrency")]
        public async Task<ActionResult<Currency>> Get(string id)
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
    }
}
