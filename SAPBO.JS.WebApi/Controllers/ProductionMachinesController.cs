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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin + ", " + RoleNames.MaintenanceEmployees)]
    public class ProductionMachinesController : ControllerBase
    {
        private readonly IProductionMachineBusiness repository;
        private readonly IProductionMachineZoneBusiness _productionMachineZoneRepository;
        private readonly ILogger<ProductionMachinesController> logger;

        public ProductionMachinesController(IProductionMachineBusiness repository, ILogger<ProductionMachinesController> logger, IProductionMachineZoneBusiness productionMachineZoneRepository)
        {
            this.repository = repository;
            _productionMachineZoneRepository = productionMachineZoneRepository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetProductionMachines")]
        public async Task<ICollection<ProductionMachine>> Get(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(statusType, objectType);
        }

        // GET api/values
        [HttpGet("GetProductionMachineZones/{productionMachineId}", Name = "GetProductionMachineZones")]
        public async Task<ICollection<ProductionMachineZone>> GetZones(int productionMachineId, Enums.StatusType statusType = Enums.StatusType.Activo)
        {
            return await _productionMachineZoneRepository.GetAllByProductionMachineIdAsync(productionMachineId, statusType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetProductionMachine")]
        public async Task<ActionResult<ProductionMachine>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var productionMachine = await repository.GetAsync(id, objectType);

                if (productionMachine == null)
                    return NotFound();

                return productionMachine;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductionMachine productionMachine)
        {
            try
            {
                await repository.CreateAsync(productionMachine);

                return new CreatedAtRouteResult("GetProductionMachine", new { id = productionMachine.Id }, productionMachine);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = productionMachine.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductionMachine productionMachine)
        {
            try
            {
                if (!id.Equals(productionMachine.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = productionMachine.UpdatedBy
                    });

                await repository.UpdateAsync(productionMachine);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = productionMachine.UpdatedBy
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
