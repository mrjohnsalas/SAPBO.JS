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
    public class ProductFormatsController : Controller
    {
        private readonly IProductFormatBusiness repository;
        private readonly ILogger<ProductFormatsController> logger;

        public ProductFormatsController(IProductFormatBusiness repository, ILogger<ProductFormatsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetProductFormats")]
        public async Task<ICollection<ProductFormat>> Get(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(statusType, objectType);
        }

        // GET api/values
        [HttpGet("GetAllByProductFormulaIdAndProductMaterialTypeId", Name = "GetProductFormatsByProductFormulaIdAndProductMaterialTypeId")]
        public async Task<ICollection<ProductFormat>> GetByProductFormulaIdAndProductMaterialTypeId(int productFormulaId, int productMaterialTypeId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllByProductFormulaIdAndProductMaterialTypeIdAsync(productFormulaId, productMaterialTypeId, objectType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetProductFormat")]
        public async Task<ActionResult<ProductFormat>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var productFormat = await repository.GetAsync(id, objectType);

                if (productFormat == null)
                    return NotFound();

                return productFormat;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductFormat productFormat)
        {
            try
            {
                await repository.CreateAsync(productFormat);

                return new CreatedAtRouteResult("GetProductFormat", new { id = productFormat.Id }, productFormat);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = productFormat.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductFormat productFormat)
        {
            try
            {
                // if (!ModelState.IsValid) { throw new Exception(Helpers.Utilities.GetErrorsByModel(ModelState)); }

                if (!id.Equals(productFormat.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = productFormat.UpdatedBy
                    });

                await repository.UpdateAsync(productFormat);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = productFormat.UpdatedBy
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
