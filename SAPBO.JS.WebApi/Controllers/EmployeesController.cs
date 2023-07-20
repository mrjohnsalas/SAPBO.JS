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
        RoleNames.LogisticsEmployees + ", " +
        RoleNames.SalesEmployees + ", " +
        RoleNames.MaintenanceEmployees + ", " +
        RoleNames.CreditEmployees)]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeBusiness repository;
        private readonly ILogger<EmployeesController> logger;

        public EmployeesController(IEmployeeBusiness repository, ILogger<EmployeesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetEmployees")]
        public async Task<ICollection<Employee>> Get(string businessUnitId = "", Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(businessUnitId, statusType, objectType);
        }

        // GET api/values
        [HttpGet("GetSuper", Name = "GetSuperEmployees")]
        public async Task<ICollection<Employee>> GetSuper(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllSupersAsync(statusType, objectType);
        }

        // GET api/values
        [HttpGet("GetSaleEmployees", Name = "GetSaleEmployees")]
        public async Task<ICollection<Employee>> GetSaleEmployees(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllSaleEmployeesAsync(statusType, objectType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<ActionResult<Employee>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var employee = await repository.GetAsync(id, objectType);

                if (employee == null)
                    return NotFound();

                return employee;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values/5
        [HttpGet("GetByEmail/{email}", Name = "GetEmployeeByEmail")]
        public async Task<ActionResult<Employee>> GetByEmail(string email, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var employee = await repository.GetByEmailAsync(email, objectType);

                if (employee == null)
                    return NotFound();

                return employee;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values/5
        [HttpGet("GetBySaleEmployeeId/{saleEmployeeId}", Name = "GetEmployeeBySaleEmployeeId")]
        public async Task<ActionResult<Employee>> GetBySaleEmployeeId(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var employee = await repository.GetBySaleEmployeeIdAsync(saleEmployeeId, objectType);

                if (employee == null)
                    return NotFound();

                return employee;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values/5
        [HttpGet("GetByPurchaseEmployeeId/{purchaseEmployeeId}", Name = "GetEmployeeByPurchaseEmployeeId")]
        public async Task<ActionResult<Employee>> GetByPurchaseEmployeeId(int purchaseEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var employee = await repository.GetByPurchaseEmployeeIdAsync(purchaseEmployeeId, objectType);

                if (employee == null)
                    return NotFound();

                return employee;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Employee employee)
        {
            try
            {
                await repository.CreateAsync(employee);

                return new CreatedAtRouteResult("GetEmployee", new { id = employee.Id }, employee);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = employee.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Employee employee)
        {
            try
            {
                if (!id.Equals(employee.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = employee.UpdatedBy
                    });

                await repository.UpdateAsync(employee);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = employee.UpdatedBy
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
