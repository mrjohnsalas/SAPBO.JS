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
    public class BusinessPartnerPaymentsController : ControllerBase
    {
        private readonly IBusinessPartnerPaymentBusiness repository;
        private readonly ILogger<BusinessPartnerPaymentsController> logger;

        public BusinessPartnerPaymentsController(IBusinessPartnerPaymentBusiness repository, ILogger<BusinessPartnerPaymentsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetBusinessPartnerPayments")]
        public async Task<ICollection<BusinessPartnerPayment>> Get(string businessPartnerId)
        {
            return await repository.GetAllAsync(businessPartnerId);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetBusinessPartnerPayment")]
        public async Task<ActionResult<BusinessPartnerPayment>> Get(int id)
        {
            try
            {
                var payment = await repository.GetAsync(id);

                if (payment == null)
                    return NotFound();

                return payment;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
