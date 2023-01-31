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
        RoleNames.PurchaseEmployees)]
    public class PurchaseOrdersController : Controller
    {
        private readonly IPurchaseOrderBusiness repository;
        private readonly ILogger<PurchaseOrdersController> logger;

        public PurchaseOrdersController(IPurchaseOrderBusiness repository, ILogger<PurchaseOrdersController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetPurchaseOrders")]
        public async Task<ICollection<PurchaseOrder>> Get(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(year, month, objectType);
        }

        // GET api/values
        [HttpGet("GetByBusinessPartnerId/{businessPartnerId}", Name = "GetPurchaseOrdersByBusinessPartnerId")]
        public async Task<ICollection<PurchaseOrder>> GetByBusinessPartnerId(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllByBusinessPartnerIdAsync(businessPartnerId, year, month, objectType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetPurchaseOrder")]
        public async Task<ActionResult<PurchaseOrder>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var purchaseOrder = await repository.GetAsync(id, objectType);

                if (purchaseOrder == null)
                    return NotFound();

                return purchaseOrder;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }
    }
}
