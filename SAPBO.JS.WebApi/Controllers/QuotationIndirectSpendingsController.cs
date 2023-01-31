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
    public class QuotationIndirectSpendingsController : Controller
    {
        private readonly IQuotationIndirectSpendingBusiness repository;
        private readonly ILogger<QuotationIndirectSpendingsController> logger;

        public QuotationIndirectSpendingsController(IQuotationIndirectSpendingBusiness repository, ILogger<QuotationIndirectSpendingsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetQuotationIndirectSpendings")]
        public async Task<ICollection<QuotationIndirectSpending>> Get(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return await repository.GetAllAsync(statusType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetQuotationIndirectSpending")]
        public async Task<ActionResult<QuotationIndirectSpending>> Get(int id)
        {
            try
            {
                var quotationIndirectSpending = await repository.GetAsync(id);

                if (quotationIndirectSpending == null)
                    return NotFound();

                return quotationIndirectSpending;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] QuotationIndirectSpending quotationIndirectSpending)
        {
            try
            {
                await repository.CreateAsync(quotationIndirectSpending);

                return new CreatedAtRouteResult("GetQuotationIndirectSpending", new { id = quotationIndirectSpending.Id }, quotationIndirectSpending);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = quotationIndirectSpending.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] QuotationIndirectSpending quotationIndirectSpending)
        {
            try
            {
                // if (!ModelState.IsValid) { throw new Exception(Helpers.Utilities.GetErrorsByModel(ModelState)); }

                if (!id.Equals(quotationIndirectSpending.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = quotationIndirectSpending.UpdatedBy
                    });

                await repository.UpdateAsync(quotationIndirectSpending);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = quotationIndirectSpending.UpdatedBy
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
