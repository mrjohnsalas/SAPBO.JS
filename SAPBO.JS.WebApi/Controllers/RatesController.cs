using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.WebApi.Controllers
{
    [Route(AppConfiguration.WebApiRoutePath)]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = 
        RoleNames.Admin + ", " +
        RoleNames.Manager + ", " +
        RoleNames.Customer + ", " +
        RoleNames.SalesEmployees)]
    public class RatesController : Controller
    {
        private readonly IRateBusiness repository;
        private readonly ILogger<RatesController> logger;

        public RatesController(IRateBusiness repository, ILogger<RatesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetAllForSales")]
        public async Task<ICollection<Rate>> Get()
        {
            return await repository.GetAllForSalesAsync();
        }

        // GET api/values/5
        [HttpGet("GetTodayUsdRate", Name = "GetTodayUsdRate")]
        public async Task<ActionResult<Rate>> GetTodayUsdRate()
        {
            return await repository.GetByDateAndCurrencyIdAsync(DateTime.Now, "USD");
        }

        // GET api/values/5
        [HttpGet("GetTodayRate", Name = "GetTodayRate")]
        public async Task<ICollection<Rate>> GetTodayRate()
        {
            return await repository.GetTodayAsync();
        }
    }
}
