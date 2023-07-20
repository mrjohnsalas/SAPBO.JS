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
        RoleNames.CreditEmployees)]
    public class SaleOrderAuthorizationsController : Controller
    {
        private readonly ISaleOrderAuthorizationBusiness repository;
        private readonly ILogger<SaleOrderAuthorizationsController> logger;

        public SaleOrderAuthorizationsController(ISaleOrderAuthorizationBusiness repository, ILogger<SaleOrderAuthorizationsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetSaleOrderAuthorizations")]
        public async Task<ICollection<SaleOrderAuthorization>> Get(int year, int month)
        {
            return await repository.GetAllAsync(year, month);
        }

        // GET api/values
        [HttpGet("GetBySaleEmployeeId/{saleEmployeeId}", Name = "GetSaleOrderAuthorizationsBySaleEmployeeId")]
        public async Task<ICollection<SaleOrderAuthorization>> GetBySaleEmployeeId(int saleEmployeeId, int year, int month)
        {
            return await repository.GetAllBySaleEmployeeIdAsync(saleEmployeeId, year, month);
        }

        // GET api/values
        [HttpGet("GetByBusinessPartnerId/{businessPartnerId}", Name = "GetSaleOrderAuthorizationsByBusinessPartnerId")]
        public async Task<ICollection<SaleOrderAuthorization>> GetByBusinessPartnerId(string businessPartnerId, int year, int month)
        {
            return await repository.GetAllByBusinessPartnerIdAsync(businessPartnerId, year, month);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetSaleOrderAuthorization")]
        public async Task<ActionResult<SaleOrderAuthorization>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var saleOrderAuthorization = await repository.GetAsync(id, objectType);

                if (saleOrderAuthorization == null)
                    return NotFound();

                return saleOrderAuthorization;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values/end/5
        [HttpPost("ApproveList")]
        public async Task<ICollection<ApprovalListResult>> ApproveList([FromBody] List<int> ids, [FromQuery] string updatedBy)
        {
            return await repository.ApproveListAsync(ids, updatedBy);
        }

        // POST api/values/init/5
        [HttpPost("Approve/{id}")]
        public async Task<ActionResult> Approve(int id, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.ApproveAsync(id, updatedBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = updatedBy
                });
            }
        }

        // POST api/values/end/5
        [HttpPost("Reject/{id}")]
        public async Task<ActionResult> Reject(int id, [FromBody] RejectReason rejectReason, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.RejectAsync(id, rejectReason.Reason, updatedBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = updatedBy
                });
            }
        }
    }
}
