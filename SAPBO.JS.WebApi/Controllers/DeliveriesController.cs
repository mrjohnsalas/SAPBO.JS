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
        RoleNames.Carrier + ", " +
        RoleNames.LogisticsEmployees + ", " +
        RoleNames.SalesEmployees + ", " +
        RoleNames.CreditEmployees)]
    public class DeliveriesController : Controller
    {
        private readonly IDeliveryBusiness repository;
        private readonly ILogger<DeliveriesController> logger;

        public DeliveriesController(IDeliveryBusiness repository, ILogger<DeliveriesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetDeliveries")]
        public async Task<ICollection<Delivery>> Get(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(year, month, objectType);
        }

        // GET api/values
        [HttpGet("GetByCarrierId/{carrierId}", Name = "GetDeliveriesByCarrierId")]
        public async Task<ICollection<Delivery>> GetByCarrierId(string carrierId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllByCarrierIdAsync(carrierId, year, month, objectType);
        }

        // GET api/valuesl
        [HttpGet("GetByBusinessPartnerId/{businessPartnerId}", Name = "GetDeliveriesByBusinessPartnerId")]
        public async Task<ICollection<Delivery>> GetByBusinessPartnerId(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllByBusinessPartnerIdAsync(businessPartnerId, year, month, objectType);
        }

        // GET api/values
        [HttpGet("GetBySaleOrderIdAndLineNum/{saleOrderId}", Name = "GetDeliveriesBySaleOrderIdAndLineNum")]
        public async Task<ICollection<Delivery>> GetBySaleOrderIdAndLineNum(int saleOrderId, int lineNum)
        {
            return await repository.GetAllBySaleOrderIdAndLineNumAsync(saleOrderId, lineNum);
        }

        // GET api/values/5
        [HttpGet("{id:int}", Name = "GetDelivery")]
        public async Task<ActionResult<Delivery>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var delivery = await repository.GetAsync(id, objectType);

                if (delivery == null)
                    return NotFound();

                return delivery;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values/init/5
        [HttpPost("Dispatched/{id:int}")]
        public async Task<ActionResult> Dispatched(int id, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.DispatchedAsync(id, updatedBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = ""
                });
            }
        }

        // POST api/values/end/5
        [HttpPost("Delivered/{id:int}")]
        public async Task<ActionResult> Delivered(int id, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.DeliveredAsync(id, updatedBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = ""
                });
            }
        }

        // POST api/values/end/5
        [HttpPost("SetCarrier")]
        public async Task<ICollection<ApprovalListResult>> SetCarrier([FromBody] DeliveryData deliveryData)
        {
            return await repository.SetCarrierAsync(deliveryData.Ids, deliveryData.CarrierId, deliveryData.AddressId);
        }
    }
}
