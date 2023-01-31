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
        RoleNames.PurchaseEmployees)]
    public class PurchaseOrderAuthorizationsController : Controller
    {
        private readonly IPurchaseOrderAuthorizationBusiness repository;
        private readonly ILogger<PurchaseOrderAuthorizationsController> logger;

        public PurchaseOrderAuthorizationsController(IPurchaseOrderAuthorizationBusiness repository, ILogger<PurchaseOrderAuthorizationsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetPurchaseOrderAuthorizations")]
        public async Task<ICollection<PurchaseOrderAuthorization>> Get(int year, int month)
        {
            return await repository.GetAllAsync(year, month);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetPurchaseOrderAuthorization")]
        public async Task<ActionResult<PurchaseOrderAuthorization>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var purchaseOrderAuthorization = await repository.GetAsync(id, objectType);

                if (purchaseOrderAuthorization == null)
                    return NotFound();

                return purchaseOrderAuthorization;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
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
        [HttpPost("ApproveList")]
        public async Task<ICollection<ApprovalListResult>> ApproveList([FromBody] List<int> ids, [FromQuery] string updatedBy)
        {
            return await repository.ApproveListAsync(ids, updatedBy);
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

        // POST api/values/init/5
        [HttpPost("Override/{id}")]
        public async Task<ActionResult> Override(int id, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.OverrideAsync(id, updatedBy);

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
