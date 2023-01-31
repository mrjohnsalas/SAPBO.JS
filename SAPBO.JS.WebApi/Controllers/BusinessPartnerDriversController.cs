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
        RoleNames.Supplier + ", " +
        RoleNames.LogisticsEmployees + ", " +
        RoleNames.PurchaseEmployees)]
    public class BusinessPartnerDriversController : Controller
    {
        private readonly IBusinessPartnerDriverBusiness repository;
        private readonly ILogger<BusinessPartnerDriversController> logger;

        public BusinessPartnerDriversController(IBusinessPartnerDriverBusiness repository, ILogger<BusinessPartnerDriversController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetBusinessPartnerDrivers")]
        public async Task<ICollection<BusinessPartnerDriver>> Get(string businessPartnerId)
        {
            return await repository.GetAllAsync(businessPartnerId);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetBusinessPartnerDriver")]
        public async Task<ActionResult<BusinessPartnerDriver>> Get(int id)
        {
            try
            {
                var contact = await repository.GetAsync(id);

                if (contact == null)
                    return NotFound();

                return contact;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BusinessPartnerDriver driver)
        {
            try
            {
                await repository.CreateAsync(driver);

                return new CreatedAtRouteResult("GetBusinessPartnerDriver", new { id = driver.Id }, driver);
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
        public async Task<ActionResult> Put(int id, [FromBody] BusinessPartnerDriver driver)
        {
            try
            {
                if (!id.Equals(driver.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}"
                    });

                await repository.UpdateAsync(driver);

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
    }
}
