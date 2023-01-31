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
        RoleNames.Customer + ", " +
        RoleNames.Supplier + ", " +
        RoleNames.CreditEmployees + ", " +
        RoleNames.ProductionEmployees + ", " +
        RoleNames.LogisticsEmployees + ", " +
        RoleNames.PurchaseEmployees + ", " +
        RoleNames.SalesEmployees)]
    public class BusinessPartnerAddressesController : ControllerBase
    {
        private readonly IBusinessPartnerAddressBusiness repository;
        private readonly ILogger<BusinessPartnerAddressesController> logger;

        public BusinessPartnerAddressesController(IBusinessPartnerAddressBusiness repository, ILogger<BusinessPartnerAddressesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetBusinessPartnerAddresses")]
        public async Task<ICollection<BusinessPartnerAddress>> Get(string businessPartnerId)
        {
            return await repository.GetAllAsync(businessPartnerId);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetBusinessPartnerAddress")]
        public async Task<ActionResult<BusinessPartnerAddress>> Get(string businessPartnerId, string id)
        {
            try
            {
                var address = await repository.GetAsync(businessPartnerId, id);

                if (address == null)
                    return NotFound();

                return address;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
