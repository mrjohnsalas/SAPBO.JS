using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Dto;
using SAPBO.JS.Model.Helper;

namespace SAPBO.JS.WebApi.Controllers
{
    [Route(AppConfiguration.WebApiRoutePath)]
    [ApiController]
    public class ContactMessagesController : Controller
    {
        private readonly IContactMessageBusiness repository;
        private readonly ILogger<DeliveriesController> logger;

        public ContactMessagesController(IContactMessageBusiness repository, ILogger<DeliveriesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // POST api/values
        //[HttpPost]
        [HttpPost("SendContactMessage")]
        public async Task<ActionResult> SendContactMessage([FromBody] ContactMessage message)
        {
            try
            {
                await repository.SendContactMessageAsync(message);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = ""
                });
            }
        }
    }
}
