using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Helper;
using SAPBO.JS.WebApi.Utilities;

namespace SAPBO.JS.WebApi.Controllers
{
    [Route(AppConfiguration.WebApiRoutePath)]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =
        RoleNames.Admin + ", " +
        RoleNames.Manager + ", " +
        RoleNames.Customer + ", " +
        RoleNames.LogisticsEmployees + ", " +
        RoleNames.SalesEmployees + ", " +
        RoleNames.CreditEmployees)]
    public class SaleOrdersController : Controller
    {
        private readonly ISaleOrderBusiness repository;
        private readonly ILogger<SaleOrdersController> logger;
        private readonly IFileStorage fileStorage;
        private readonly IShoppingCartItemBusiness shoppingCartItemRepository;

        public SaleOrdersController(ISaleOrderBusiness repository, ILogger<SaleOrdersController> logger, IFileStorage fileStorage, IShoppingCartItemBusiness shoppingCartItemRepository)
        {
            this.repository = repository;
            this.logger = logger;
            this.fileStorage = fileStorage;
            this.shoppingCartItemRepository = shoppingCartItemRepository;
        }

        // GET api/values
        [HttpGet(Name = "GetSaleOrders")]
        public async Task<ICollection<SaleOrder>> Get(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(year, month, objectType);
        }

        // GET api/values
        [HttpGet("GetPendingSaleOrders", Name = "GetPendingSaleOrders")]
        public async Task<ICollection<SaleOrder>> GetPending()
        {
            return await repository.GetAllPendingAsync();
        }

        // GET api/values
        [HttpGet("GetBySaleEmployeeId/{saleEmployeeId}", Name = "GetSaleOrdersBySaleEmployeeId")]
        public async Task<ICollection<SaleOrder>> GetBySaleEmployeeId(int saleEmployeeId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllBySaleEmployeeIdAsync(saleEmployeeId, year, month, objectType);
        }

        // GET api/values
        [HttpGet("GetPendingBySaleEmployeeId/{saleEmployeeId}", Name = "GetSaleOrdersPendingBySaleEmployeeId")]
        public async Task<ICollection<SaleOrder>> GetPendingBySaleEmployeeId(int saleEmployeeId)
        {
            return await repository.GetAllPendingBySaleEmployeeIdAsync(saleEmployeeId);
        }

        // GET api/values
        [HttpGet("GetByBusinessPartnerId/{businessPartnerId}", Name = "GetSaleOrdersByBusinessPartnerId")]
        public async Task<ICollection<SaleOrder>> GetByBusinessPartnerId(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllByBusinessPartnerIdAsync(businessPartnerId, year, month, objectType);
        }

        // GET api/values
        [HttpGet("GetPendingByBusinessPartnerId/{businessPartnerId}", Name = "GetSaleOrdersPendingByBusinessPartnerId")]
        public async Task<ICollection<SaleOrder>> GetPendingByBusinessPartnerId(string businessPartnerId)
        {
            return await repository.GetAllPendingByBusinessPartnerIdAsync(businessPartnerId);
        }

        // GET api/values/5
        [HttpGet("GetCountBySaleEmployeeId/{saleEmployeeId}", Name = "GetSaleOrdersCountBySaleEmployeeId")]
        public Task<int> GetCountBySaleEmployeeId(int saleEmployeeId)
        {
            return repository.GetCountBySaleEmployeeIdAsync(saleEmployeeId);
        }

        // GET api/values/5
        [HttpGet("GetCountByBusinessPartnerId/{businessPartnerId}", Name = "GetSaleOrdersCountByBusinessPartnerId")]
        public Task<int> GetCountByBusinessPartnerId(string businessPartnerId)
        {
            return repository.GetCountByBusinessPartnerIdAsync(businessPartnerId);
        }

        // GET api/values
        [HttpGet("GetTopByBusinessPartnerId/{businessPartnerId}", Name = "GetTopSaleOrdersByBusinessPartnerId")]
        public async Task<ICollection<SaleOrder>> GetTopByBusinessPartnerId(string businessPartnerId, int count)
        {
            return await repository.GetTopByBusinessPartnerIdAsync(businessPartnerId, count);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetSaleOrder")]
        public async Task<ActionResult<SaleOrder>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var saleOrder = await repository.GetAsync(id, objectType);

                if (saleOrder == null)
                    return NotFound();

                return saleOrder;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] ShoppingCart shoppingCart)
        {
            try
            {
                await repository.CreateAsync(shoppingCart);

                await shoppingCartItemRepository.DeleteAllAsync(shoppingCart.CreatedBy);

                if (shoppingCart.BpReferenceFile != null)
                {
                    var fileNameWithoutExtension = $"WPD-{shoppingCart.SaleOrderId:00000}";

                    var fullPath = await fileStorage.SaveFile(AppDefaultValues.AttachmentPathServer, shoppingCart.BpReferenceFile, fileNameWithoutExtension);
                    await repository.AddFileAttachmentAsync(shoppingCart.SaleOrderId, AppDefaultValues.AttachmentPathServer, fileNameWithoutExtension, Path.GetExtension(fullPath).Replace(".", string.Empty));
                }

                return shoppingCart.SaleOrderId;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = shoppingCart.CreatedBy
                });
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await repository.DeleteAsync(id);

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
