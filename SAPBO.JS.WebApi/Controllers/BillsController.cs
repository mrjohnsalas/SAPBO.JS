using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Dto;
using SAPBO.JS.Model.Helper;
using SAPBO.JS.WebApi.Utilities;
using System.Text;
using System.Net.Mail;

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
        RoleNames.CreditEmployees)]
    public class BillsController : Controller
    {
        private readonly IBillBusiness repository;
        private readonly IBillFileBusiness billFileRepository;
        private readonly IEmailBusiness emailRepository;
        private readonly ILogger<BillsController> logger;
        private readonly IFileStorage fileStorage;

        public BillsController(IBillBusiness repository, ILogger<BillsController> logger, IFileStorage fileStorage, IBillFileBusiness billFileRepository, IEmailBusiness emailRepository)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            this.repository = repository;
            this.logger = logger;
            this.fileStorage = fileStorage;
            this.billFileRepository = billFileRepository;
            this.emailRepository = emailRepository;
        }

        // GET api/values
        [HttpGet(Name = "GetBills")]
        public async Task<ICollection<Bill>> Get(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(year, month, objectType);
        }

        // GET api/values
        [HttpGet("GetBySaleEmployeeId/{saleEmployeeId}", Name = "GetBillsBySaleEmployeeId")]
        public async Task<ICollection<Bill>> GetBySaleEmployeeId(int saleEmployeeId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllBySaleEmployeeIdAsync(saleEmployeeId, year, month, objectType);
        }

        // GET api/values
        [HttpGet("GetByBusinessPartnerId/{businessPartnerId}", Name = "GetBillsByBusinessPartnerId")]
        public async Task<ICollection<Bill>> GetByBusinessPartnerId(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllByBusinessPartnerIdAsync(businessPartnerId, year, month, objectType);
        }

        // GET api/values
        [HttpGet("GetPending", Name = "GetPendingBills")]
        public async Task<ICollection<Bill>> GetPending(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllPendingAsync(objectType);
        }

        // GET api/values
        [HttpGet("GetPendingBySaleEmployeeId/{saleEmployeeId}", Name = "GetPendingBillsBySaleEmployeeId")]
        public async Task<ICollection<Bill>> GetPendingBySaleEmployeeId(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllPendingBySaleEmployeeIdAsync(saleEmployeeId, objectType);
        }

        // GET api/values
        [HttpGet("GetPendingByBusinessPartnerId/{businessPartnerId}", Name = "GetPendingBillsByBusinessPartnerId")]
        public async Task<ICollection<Bill>> GetPendingByBusinessPartnerId(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllPendingByBusinessPartnerIdAsync(businessPartnerId, objectType);
        }


        // GET api/values/5
        [HttpGet("{id}", Name = "GetBill")]
        public async Task<ActionResult<Bill>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var bill = await repository.GetAsync(id, objectType);

                if (bill == null)
                    return NotFound();

                return bill;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        //GET api/values/5
        [HttpGet("GetFile/{fileId}")]
        public async Task<ActionResult> GetFile(int fileId, Enums.BillFileType billFileType = Enums.BillFileType.PDF)
        {
            try
            {
                var files = await billFileRepository.GetAllAsync(fileId);

                if (files == null)
                    return NotFound();

                var selectedFile = files.FirstOrDefault(x => x.BillFileType == billFileType);
                var selectedFileType = Common.Utilities.BillFileTypeToContentType(billFileType);

                var ms = new MemoryStream(System.IO.File.ReadAllBytes(selectedFile.FullFilePath));

                return File(ms, selectedFileType, selectedFile.FileName);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values/end/5
        [HttpPost("SendEmail/{fileId}")]
        public async Task<ActionResult> SendEmail(int fileId, [FromBody] AppEmailDto appEmailDto, [FromQuery] bool includeAllFiles = false)
        {
            try
            {
                var files = await billFileRepository.GetAllAsync(fileId);
                var selectedFile = files.FirstOrDefault(x => x.BillFileType == Enums.BillFileType.PDF);

                if (files == null)
                    return NotFound();

                var appEmail = new AppEmail();
                appEmail.Subject = appEmailDto.Subject;
                appEmail.Body = appEmailDto.Body;
                
                appEmail.To = new List<MailAddress>();
                foreach (var mailAddress in appEmailDto.To)
                    appEmail.To.Add(new MailAddress(mailAddress.Address, mailAddress.DisplayName));

                if (appEmailDto.Cc != null && appEmailDto.Cc.Any())
                {
                    appEmail.Cc = new List<MailAddress>();
                    foreach (var mailAddress in appEmailDto.Cc)
                        appEmail.Cc.Add(new MailAddress(mailAddress.Address, mailAddress.DisplayName));
                }
                

                if (appEmailDto.Co != null && appEmailDto.Co.Any())
                {
                    appEmail.Co = new List<MailAddress>();
                    foreach (var mailAddress in appEmailDto.Co)
                        appEmail.Co.Add(new MailAddress(mailAddress.Address, mailAddress.DisplayName));
                }

                appEmail.Attachments = new List<Attachment>();
                if (includeAllFiles)
                {
                    foreach (var file in files)
                    {
                        appEmail.Attachments.Add(new Attachment(new MemoryStream(
                            System.IO.File.ReadAllBytes(file.FullFilePath)), 
                            file.FileName, 
                            Common.Utilities.BillFileTypeToContentType(file.BillFileType)));
                    }
                } 
                else
                {
                    appEmail.Attachments.Add(new Attachment(new MemoryStream(
                            System.IO.File.ReadAllBytes(selectedFile.FullFilePath)),
                            selectedFile.FileName,
                            Common.Utilities.BillFileTypeToContentType(selectedFile.BillFileType)));
                }

                emailRepository.SendEmailAsync(appEmail);

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
