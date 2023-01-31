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
    public class BusinessPartnerContactsController : ControllerBase
    {
        private readonly IBusinessPartnerContactBusiness repository;
        private readonly ILogger<BusinessPartnerContactsController> logger;

        public BusinessPartnerContactsController(IBusinessPartnerContactBusiness repository, ILogger<BusinessPartnerContactsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetBusinessPartnerContacts")]
        public async Task<ICollection<BusinessPartnerContact>> Get(string businessPartnerId)
        {
            return await repository.GetAllAsync(businessPartnerId);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetBusinessPartnerContact")]
        public async Task<ActionResult<BusinessPartnerContact>> Get(int id)
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
        public async Task<ActionResult> Post([FromBody] BusinessPartnerContact contact)
        {
            try
            {
                await repository.CreateAsync(contact);

                return new CreatedAtRouteResult("GetBusinessPartnerContact", new { id = contact.Id }, contact);
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
        public async Task<ActionResult> Put(int id, [FromBody] BusinessPartnerContact contact)
        {
            try
            {
                if (!id.Equals(contact.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}"
                    });

                await repository.UpdateAsync(contact);

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
