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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Customer + ", " + RoleNames.SalesEmployees)]
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartItemBusiness repository;
        private readonly ILogger<ShoppingCartsController> logger;

        public ShoppingCartsController(IShoppingCartItemBusiness repository, ILogger<ShoppingCartsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet("{userId}", Name = "GetShoppingCart")]
        public async Task<ICollection<ShoppingCartItem>> Get(string userId, DateTime? rateDate, string businessPartnerId = "", string currencyId = AppDefaultValues.CurrencyIdDolar)
        {
            return await repository.GetAllAsync(userId, rateDate, businessPartnerId, currencyId);
        }

        // GET api/values/5
        [HttpGet("GetShoppingCartItem/{id}", Name = "GetShoppingCartItem")]
        public async Task<ActionResult<ShoppingCartItem>> Get(int id, DateTime? rateDate, string businessPartnerId = "", string currencyId = AppDefaultValues.CurrencyIdDolar)
        {
            try
            {
                var failureType = await repository.GetAsync(id, rateDate, businessPartnerId, currencyId);

                if (failureType == null)
                    return NotFound();

                return failureType;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values/5
        [HttpGet("GetCountByUserId/{userId}", Name = "GetShoppingCartItemsCountByUserId")]
        public Task<int> GetCountByUserId(string userId)
        {
            return repository.GetCountByUserIdAsync(userId);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ShoppingCartItem shoppingCart)
        {
            try
            {
                await repository.CreateAsync(shoppingCart);

                return new CreatedAtRouteResult("GetShoppingCartItem", new { id = shoppingCart.Id }, shoppingCart);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = shoppingCart.UserId
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ShoppingCartItem shoppingCart)
        {
            try
            {
                if (!id.Equals(shoppingCart.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = shoppingCart.UserId
                    });

                await repository.UpdateAsync(shoppingCart);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = shoppingCart.UserId
                });
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromQuery] string deleteBy)
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
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = deleteBy
                });
            }
        }

        // DELETE api/values/5
        [HttpDelete("DeleteAllShoppingCartItems/{userId}", Name = "DeleteAllShoppingCartItems")]
        public async Task<ActionResult> DeleteAll(string userId)
        {
            try
            {
                await repository.DeleteAllAsync(userId);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = userId
                });
            }
        }
    }
}
