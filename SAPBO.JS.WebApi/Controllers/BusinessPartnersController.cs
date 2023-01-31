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
        RoleNames.Supplier + ", " +
        RoleNames.CreditEmployees + ", " +
        RoleNames.ProductionEmployees + ", " +
        RoleNames.LogisticsEmployees + ", " +
        RoleNames.PurchaseEmployees + ", " +
        RoleNames.SalesEmployees)]
    public class BusinessPartnersController : ControllerBase
    {
        private readonly IBusinessPartnerBusiness repository;
        private readonly ILogger<BusinessPartnersController> logger;

        public BusinessPartnersController(IBusinessPartnerBusiness repository, ILogger<BusinessPartnersController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetBusinessPartners")]
        public async Task<ICollection<BusinessPartner>> Get(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(objectType);
        }

        // GET api/values
        [HttpGet("GetAllWithTemp", Name = "GetAllWithTemp")]
        public async Task<ICollection<BusinessPartner>> GetAllWithTemp(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllWithTempAsync(objectType);
        }

        // GET api/values
        [HttpGet("GetTempAll", Name = "GetTempAll")]
        public async Task<ICollection<BusinessPartner>> GetTempAll(Enums.ObjectType objectType = Enums.ObjectType.Only, Enums.StatusType statusType = Enums.StatusType.Todos)
        {
            return await repository.GetTempAllAsync(statusType, objectType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetBusinessPartner")]
        public async Task<ActionResult<BusinessPartner>> Get(string id, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            try
            {
                var businessPartner = await repository.GetAsync(id, objectType);

                if (businessPartner == null)
                    return NotFound();

                return businessPartner;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values/5
        [HttpGet("GetTemp/{id}", Name = "GetTemp")]
        public async Task<ActionResult<BusinessPartner>> GetTemp(string id, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            try
            {
                var businessPartner = await repository.GetTempAsync(id, objectType);

                if (businessPartner == null)
                    return NotFound();

                return businessPartner;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values/5
        [HttpGet("GetWithTemp/{id}", Name = "GetWithTemp")]
        public async Task<ActionResult<BusinessPartner>> GetWithTemp(string id, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            try
            {
                var businessPartner = await repository.GetWithTempAsync(id, objectType);

                if (businessPartner == null)
                    return NotFound();

                return businessPartner;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values/5
        [HttpGet("GetByRUC/{id}", Name = "GetByRUC")]
        public async Task<ActionResult<BusinessPartner>> GetByRUC(string ruc, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            try
            {
                var businessPartner = await repository.GetByRUCAsync(ruc, objectType);

                if (businessPartner == null)
                    return NotFound();

                return businessPartner;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values/5
        [HttpGet("GetTempByRUC/{id}", Name = "GetTempByRUC")]
        public async Task<ActionResult<BusinessPartner>> GetTempByRUC(string ruc, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            try
            {
                var businessPartner = await repository.GetTempByRUCAsync(ruc, objectType);

                if (businessPartner == null)
                    return NotFound();

                return businessPartner;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values
        [HttpGet("GetAllBySaleEmployeeId/{saleEmployeeId}", Name = "GetAllBySaleEmployeeId")]
        public async Task<ICollection<BusinessPartner>> GetAllBySaleEmployeeId(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only, bool includeLeads = false)
        {
            return await repository.GetAllBySaleEmployeeIdAsync(saleEmployeeId, objectType, includeLeads);
        }

        // GET api/values
        [HttpGet("GetAllWithTempBySaleEmployeeId/{saleEmployeeId}", Name = "GetAllWithTempBySaleEmployeeId")]
        public async Task<ICollection<BusinessPartner>> GetAllWithTempBySaleEmployeeId(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllWithTempBySaleEmployeeIdAsync(saleEmployeeId, objectType);
        }

        // GET api/values
        [HttpGet("GetTempAllBySaleEmployeeId/{saleEmployeeId}", Name = "GetTempAllBySaleEmployeeId")]
        public async Task<ICollection<BusinessPartner>> GetTempAllBySaleEmployeeId(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetTempAllBySaleEmployeeIdAsync(saleEmployeeId, objectType);
        }

        // GET api/values
        [HttpGet("GetProviders", Name = "GetProviders")]
        public async Task<ICollection<BusinessPartner>> GetProviders(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetProviderAllAsync(objectType);
        }

        // GET api/values/5
        [HttpGet("GetProvider/{id}", Name = "GetProvider")]
        public async Task<ActionResult<BusinessPartner>> GetProvider(string id, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            try
            {
                var businessPartner = await repository.GetProviderAsync(id, objectType);

                if (businessPartner == null)
                    return NotFound();

                return businessPartner;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values
        [HttpGet("GetCarriers", Name = "GetCarriers")]
        public async Task<ICollection<BusinessPartner>> GetCarriers(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetCarrierAllAsync(objectType);
        }

        // GET api/values/5
        [HttpGet("GetCarrier/{id}", Name = "GetCarrier")]
        public async Task<ActionResult<BusinessPartner>> GetCarrier(string id, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            try
            {
                var businessPartner = await repository.GetCarrierAsync(id, objectType);

                if (businessPartner == null)
                    return NotFound();

                return businessPartner;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // GET api/values
        [HttpGet("GetTransportAgencies", Name = "GetTransportAgencies")]
        public async Task<ICollection<BusinessPartner>> GetTransportAgencies(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetTransportAgencyAllAsync(objectType);
        }

        // GET api/values/5
        [HttpGet("GetTransportAgency/{id}", Name = "GetTransportAgency")]
        public async Task<ActionResult<BusinessPartner>> GetTransportAgency(string id, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            try
            {
                var businessPartner = await repository.GetTransportAgencyAsync(id, objectType);

                if (businessPartner == null)
                    return NotFound();

                return businessPartner;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BusinessPartner businessPartner)
        {
            try
            {
                await repository.CreateAsync(businessPartner);

                return new CreatedAtRouteResult("GetTemp", new { id = businessPartner.Id }, businessPartner);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = businessPartner.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] BusinessPartner businessPartner)
        {
            try
            {
                if (!id.Equals(businessPartner.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = businessPartner.UpdatedBy
                    });

                await repository.UpdateAsync(businessPartner);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = businessPartner.UpdatedBy
                });
            }
        }
    }
}
