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
    public class ProductMaterialTypesController : Controller
    {
        private readonly IProductMaterialTypeBusiness repository;
        private readonly ILogger<ProductMaterialTypesController> logger;

        public ProductMaterialTypesController(IProductMaterialTypeBusiness repository, ILogger<ProductMaterialTypesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetProductMaterialTypes")]
        public async Task<ICollection<ProductMaterialType>> Get(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return await repository.GetAllAsync(statusType);
        }

        // GET api/values
        [HttpGet("GetAllByProductFormulaId/{productFormulaId}", Name = "GetProductMaterialTypesByProductFormulaId")]
        public async Task<ICollection<ProductMaterialType>> GetByProductFormulaId(int productFormulaId)
        {
            return await repository.GetAllByProductFormulaIdAsync(productFormulaId);
        }

        // GET api/values
        [HttpGet("GetAllOnlyPaper", Name = "GetProductMaterialTypesOnlyPaper")]
        public async Task<ICollection<ProductMaterialType>> GetOnlyPaper()
        {
            return await repository.GetOnlyPaperAsync();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetProductMaterialType")]
        public async Task<ActionResult<ProductMaterialType>> Get(int id)
        {
            try
            {
                var productMaterialType = await repository.GetAsync(id);

                if (productMaterialType == null)
                    return NotFound();

                return productMaterialType;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductMaterialType productMaterialType)
        {
            try
            {
                await repository.CreateAsync(productMaterialType);

                return new CreatedAtRouteResult("GetProductMaterialType", new { id = productMaterialType.Id }, productMaterialType);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = productMaterialType.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductMaterialType productMaterialType)
        {
            try
            {
                // if (!ModelState.IsValid) { throw new Exception(Helpers.Utilities.GetErrorsByModel(ModelState)); }

                if (!id.Equals(productMaterialType.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = productMaterialType.UpdatedBy
                    });

                await repository.UpdateAsync(productMaterialType);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = productMaterialType.UpdatedBy
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
