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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin + ", " + RoleNames.SalesEmployees)]
    public class SaleOpportunitiesController : ControllerBase
    {
        private readonly ISaleOpportunityBusiness repository;
        private readonly ILogger<SaleOpportunitiesController> logger;

        public SaleOpportunitiesController(ISaleOpportunityBusiness repository, ILogger<SaleOpportunitiesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetSaleOpportunities")]
        public async Task<ICollection<SaleOpportunity>> Get(string businessPartnerId)
        {
            return await repository.GetAllAsync(businessPartnerId);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetSaleOpportunity")]
        public async Task<ActionResult<SaleOpportunity>> Get(int id)
        {
            try
            {
                var opportunity = await repository.GetAsync(id);

                if (opportunity == null)
                    return NotFound();

                return opportunity;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SaleOpportunity opportunity)
        {
            try
            {
                await repository.CreateAsync(opportunity);

                return new CreatedAtRouteResult("GetSaleOpportunity", new { id = opportunity.Id }, opportunity);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}"
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] SaleOpportunity opportunity)
        {
            try
            {
                if (!id.Equals(opportunity.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}"
                    });

                await repository.UpdateAsync(opportunity);

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

        // POST api/values/won/5
        [HttpPost("Won/{id}")]
        public async Task<ActionResult> Won(int id, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.WonAsync(id, updatedBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = updatedBy
                });
            }
        }

        [HttpPost("Lost/{id}")]
        public async Task<ActionResult> Lost(int id, [FromBody] List<SaleOpportunityLossReason> lostReasons, [FromQuery] string updatedBy)
        {
            try
            {
                if (lostReasons == null && !lostReasons.Any())
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.NotFoundContentFromOperation}"
                    });

                foreach (var reason in lostReasons)
                {
                    if (!id.Equals(reason.SaleOpportunityId))
                        return BadRequest(new ServiceException
                        {
                            Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}"
                        });
                }

                await repository.LostAsync(id, lostReasons, updatedBy);

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
