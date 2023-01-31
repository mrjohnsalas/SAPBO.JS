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
    public class ProductMaterialsController : Controller
    {
        private readonly IProductMaterialBusiness repository;
        private readonly ILogger<ProductMaterialsController> logger;

        public ProductMaterialsController(IProductMaterialBusiness repository, ILogger<ProductMaterialsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetProductMaterials")]
        public async Task<ICollection<ProductMaterial>> Get(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(statusType, objectType);
        }

        // GET api/values
        [HttpGet("GetAllByProductMaterialTypeIdAndGramajeId", Name = "GetProductMaterialsByProductMaterialTypeIdAndGramajeId")]
        public async Task<ICollection<ProductMaterial>> GetByProductMaterialTypeIdAndGramajeId(int productFormulaId, int gramajeId, int copies, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllByProductMaterialTypeIdAndGramajeIdAsync(productFormulaId, gramajeId, copies, objectType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetProductMaterial")]
        public async Task<ActionResult<ProductMaterial>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var productMaterial = await repository.GetAsync(id, objectType);

                if (productMaterial == null)
                    return NotFound();

                return productMaterial;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductMaterial productMaterial)
        {
            try
            {
                await repository.CreateAsync(productMaterial);

                return new CreatedAtRouteResult("GetProductMaterial", new { id = productMaterial.Id }, productMaterial);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = productMaterial.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductMaterial productMaterial)
        {
            try
            {
                // if (!ModelState.IsValid) { throw new Exception(Helpers.Utilities.GetErrorsByModel(ModelState)); }

                if (!id.Equals(productMaterial.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = productMaterial.UpdatedBy
                    });

                await repository.UpdateAsync(productMaterial);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = productMaterial.UpdatedBy
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
