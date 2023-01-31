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
    public class ProductGrammagesController : Controller
    {
        private readonly IProductGrammageBusiness repository;
        private readonly ILogger<ProductGrammagesController> logger;

        public ProductGrammagesController(IProductGrammageBusiness repository, ILogger<ProductGrammagesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetProductGrammages")]
        public async Task<ICollection<ProductGrammage>> Get(Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return await repository.GetAllAsync(statusType);
        }

        // GET api/values
        // Verificar si se usa
        [HttpGet("GetAllByProductMaterialTypeId/{productMaterialTypeId}", Name = "GetProductGrammagesByProductMaterialTypeId")]
        public async Task<ICollection<ProductGrammage>> GetByProductMaterialTypeId(int productMaterialTypeId)
        {
            return await repository.GetAllByProductMaterialTypeIdAsync(productMaterialTypeId);
        }

        // GET api/values
        [HttpGet("GetAllByProductFormulaIdAndProductMaterialTypeId", Name = "GetProductGrammagesByProductFormulaIdAndProductMaterialTypeId")]
        public async Task<ICollection<ProductGrammage>> GetByProductFormulaIdAndProductMaterialTypeId(int productFormulaId, int productMaterialTypeId)
        {
            return await repository.GetAllByProductFormulaIdAndProductMaterialTypeIdAsync(productFormulaId, productMaterialTypeId);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetProductGrammage")]
        public async Task<ActionResult<ProductGrammage>> Get(int id)
        {
            try
            {
                var productGrammage = await repository.GetAsync(id);

                if (productGrammage == null)
                    return NotFound();

                return productGrammage;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductGrammage productGrammage)
        {
            try
            {
                await repository.CreateAsync(productGrammage);

                return new CreatedAtRouteResult("GetProductGrammage", new { id = productGrammage.Id }, productGrammage);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = productGrammage.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductGrammage productGrammage)
        {
            try
            {
                // if (!ModelState.IsValid) { throw new Exception(Helpers.Utilities.GetErrorsByModel(ModelState)); }

                if (!id.Equals(productGrammage.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = productGrammage.UpdatedBy
                    });

                await repository.UpdateAsync(productGrammage);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = productGrammage.UpdatedBy
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
