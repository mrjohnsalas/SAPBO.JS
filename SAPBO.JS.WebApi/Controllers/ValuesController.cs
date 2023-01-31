using Microsoft.AspNetCore.Mvc;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Dto;
using SAPBO.JS.WebApi.Utilities;
using System.Net.Mail;

namespace SAPBO.JS.WebApi.Controllers
{
    [Route(AppConfiguration.WebApiRoutePath)]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IFileStorage fileStorage;
        private readonly IEmailBusiness _emailBusinessRepository;

        public ValuesController(IFileStorage fileStorage, IEmailBusiness emailBusinessRepository)
        {
            this.fileStorage = fileStorage;
            _emailBusinessRepository = emailBusinessRepository;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // GET api/values/5
        [HttpGet("GetWebPath")]
        public ActionResult<string> GetWebPath()
        {
            return fileStorage.GetWebPath("Resources", "_htmlTemplate.html");
        }

        // GET api/values/5
        [HttpGet("GetCurrentDirectory")]
        public ActionResult<string> GetCurrentDirectory()
        {
            return fileStorage.GetCurrentDirectory();
        }

        // GET api/values/5
        [HttpGet("SendTestEmail")]
        public ActionResult<string> SendTestEmail()
        {
            var appEmail = new AppEmail();
            appEmail.To = new List<MailAddress> { new MailAddress("salas.john@hotmail.com") };
            appEmail.Subject = "Test new email";
            appEmail.Body = "It's ok.";
            _emailBusinessRepository.SendEmailAsync(appEmail);
            return "Ok.";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
