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
        RoleNames.SalesEmployees + ", " +
        RoleNames.ProductionEmployees)]
    public class QuotationAccessoriesController : Controller
    {
        private readonly IQuotationAccessoryBusiness repository;
        private readonly ILogger<QuotationAccessoriesController> logger;

        public QuotationAccessoriesController(IQuotationAccessoryBusiness repository, ILogger<QuotationAccessoriesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetQuotationAccessories")]
        public async Task<ICollection<QuotationAccessory>> Get(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return await repository.GetAllAsync(statusType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetQuotationAccessory")]
        public async Task<ActionResult<QuotationAccessory>> Get(int id)
        {
            try
            {
                var quotationAccessory = await repository.GetAsync(id);

                if (quotationAccessory == null)
                    return NotFound();

                return quotationAccessory;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] QuotationAccessory quotationAccessory)
        {
            try
            {
                await repository.CreateAsync(quotationAccessory);

                return new CreatedAtRouteResult("GetQuotationAccessory", new { id = quotationAccessory.Id }, quotationAccessory);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = quotationAccessory.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] QuotationAccessory quotationAccessory)
        {
            try
            {
                // if (!ModelState.IsValid) { throw new Exception(Helpers.Utilities.GetErrorsByModel(ModelState)); }

                if (!id.Equals(quotationAccessory.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = quotationAccessory.UpdatedBy
                    });

                await repository.UpdateAsync(quotationAccessory);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = quotationAccessory.UpdatedBy
                });
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromQuery] string deleteBy)
        {
            try
            {
                await repository.DeleteAsync(id, deleteBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = deleteBy
                });
            }
        }
    }
}
