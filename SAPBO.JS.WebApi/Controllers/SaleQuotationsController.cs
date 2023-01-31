using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Helper;
using SAPBO.JS.WebApi.Utilities;
using System.Data;
using System.Text;

namespace SAPBO.JS.WebApi.Controllers
{
    [Route(AppConfiguration.WebApiRoutePath)]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =
        RoleNames.Admin + ", " +
        RoleNames.Manager + ", " +
        RoleNames.Customer + ", " +
        RoleNames.ProductionEmployees + ", " +
        RoleNames.SalesEmployees)]
    public class SaleQuotationsController : Controller
    {
        private readonly IFileStorage _fileStorage;
        private readonly ISaleQuotationBusiness repository;
        private readonly ILogger<SaleQuotationsController> logger;

        public SaleQuotationsController(ISaleQuotationBusiness repository, IFileStorage fileStorage, ILogger<SaleQuotationsController> logger)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _fileStorage = fileStorage;
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetSaleQuotations")]
        public async Task<ICollection<SaleQuotation>> Get(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(year, month, objectType);
        }

        // GET api/values
        [HttpGet("GetBySaleEmployeeId/{saleEmployeeId}", Name = "GetSaleQuotationsBySaleEmployeeId")]
        public async Task<ICollection<SaleQuotation>> GetBySaleEmployeeId(int saleEmployeeId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllBySaleEmployeeIdAsync(saleEmployeeId, year, month, objectType);
        }

        // GET api/values
        [HttpGet("GetByBusinessPartnerId/{businessPartnerId}", Name = "GetSaleQuotationsByBusinessPartnerId")]
        public async Task<ICollection<SaleQuotation>> GetByBusinessPartnerId(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllByBusinessPartnerIdAsync(businessPartnerId, year, month, objectType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetSaleQuotation")]
        public async Task<ActionResult<SaleQuotation>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var saleQuotation = await repository.GetAsync(id, objectType);

                if (saleQuotation == null)
                    return NotFound();

                return saleQuotation;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SaleQuotation saleQuotation)
        {
            try
            {
                await repository.CreateAsync(saleQuotation);

                return new CreatedAtRouteResult("GetSaleQuotation", new { id = saleQuotation.Id }, saleQuotation);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = saleQuotation.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] SaleQuotation saleQuotation)
        {
            try
            {
                if (!id.Equals(saleQuotation.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = saleQuotation.UpdatedBy
                    });

                await repository.UpdateAsync(saleQuotation);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = saleQuotation.UpdatedBy
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

        //GET api/values/5
        [HttpGet("Print/{id}")]
        public async Task<ActionResult> Print(int id)
        {
            try
            {
                var saleQuotation = await repository.GetAsync(id, Enums.ObjectType.Full);

                if (saleQuotation == null)
                    return NotFound();

                var fullPath = _fileStorage.GetWebPath("Reports", "SaleQuotationReport.rdlc");

                Stream reportDefinition;
                using var fileStream = new FileStream(fullPath, FileMode.Open);
                reportDefinition = fileStream;

                var localReport = new LocalReport();
                localReport.LoadReportDefinition(reportDefinition);

                //Set Parameters
                localReport.SetParameters(new[] { new ReportParameter("rp1", "Hello") });
                
                //Set Datasources
                var sqDT = new ReportDataSources.SaleQuotationReportDS.SaleQuotationDTDataTable();
                var sqRow = sqDT.NewRow();
                sqRow["Id"] = saleQuotation.Id;
                sqRow["IdFull"] = saleQuotation.IdFull;
                
                sqRow["BusinessPartnerId"] = saleQuotation.BusinessPartner.Id;
                sqRow["BusinessPartnerRUC"] = saleQuotation.BusinessPartner.Ruc;
                sqRow["BusinessPartnerName"] = saleQuotation.BusinessPartner.Name;

                sqRow["ContactId"] = saleQuotation.Contact.Id;
                sqRow["ContactName"] = saleQuotation.Contact.FullName;

                sqRow["SaleEmployeeId"] = saleQuotation.SaleEmployee.Id;
                sqRow["SaleEmployeeName"] = saleQuotation.SaleEmployee.FullName;

                sqRow["PaymentId"] = saleQuotation.Payment.Id;
                sqRow["PaymentName"] = saleQuotation.Payment.Name;

                sqRow["Rate"] = saleQuotation.Rate;
                sqRow["DaysValidValue"] = saleQuotation.DaysValidValue;
                sqRow["Remarks"] = saleQuotation.Remarks;

                sqRow["CreatedAt"] = saleQuotation.CreatedAt?.ToString(AppFormats.Date);
                sqRow["DeliveryDate"] = saleQuotation.DeliveryDate.ToString(AppFormats.Date);

                sqRow["CurrencyId"] = saleQuotation.Currency.Id;
                sqRow["CurrencySymbol"] = saleQuotation.Currency.Symbol;
                sqRow["CurrencyName"] = saleQuotation.Currency.Name;

                var igv = decimal.Round(saleQuotation.Total * 0.18M, 2);
                sqRow["SubTotal"] = saleQuotation.Total;
                sqRow["IGV"] = igv;
                sqRow["Total"] = saleQuotation.Total + igv;
                sqRow["Status"] = saleQuotation.StatusType;

                sqDT.Rows.Add(sqRow);
                localReport.DataSources.Add(new ReportDataSource("SaleQuotationDS", (DataTable)sqDT));

                var detailDT = new ReportDataSources.SaleQuotationReportDS.SaleQuotationDetailDTDataTable();
                var i = 0;
                if (saleQuotation.Products != null && saleQuotation.Products.Count > 0)
                    foreach (var detail in saleQuotation.Products)
                    {
                        var detailRow = detailDT.NewRow();
                        i++;
                        detailRow["Id"] = i;
                        detailRow["SaleQuotationId"] = detail.SaleQuotationId;
                        detailRow["ProductDescription"] = $"{detail.ProductFormula.Name}, {detail.ProductMaterialType.Name}, {detail.ProductGrammage.Name}, {detail.ProductFormat.Name}, {detail.ProductInkLevel.Name}, {detail.NroCopias} COPIAS, {detail.Remark}.";
                        detailRow["ProductFormatId"] = detail.ProductFormat.Id;
                        detailRow["ProductFormatName"] = detail.ProductFormat.Name;
                        detailRow["Quantity"] = detail.Quantity;
                        detailRow["NroCopias"] = detail.NroCopias;
                        detailRow["UnitPrice"] = detail.UnitPrice;
                        detailRow["TotalPrice"] = decimal.Round(detail.TotalPrice, 2);
                        detailDT.Rows.Add(detailRow);
                    }
                localReport.DataSources.Add(new ReportDataSource("SaleQuotationDetailDS", (DataTable)detailDT));

                var pdfReport = localReport.Render("PDF");

                fileStream.Dispose();

                return File(pdfReport, AppDefaultValues.PDFApplication, $"{saleQuotation.IdFull}.PDF");
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values/init/5
        [HttpPost("Approve/{id}")]
        public async Task<ActionResult> Approve(int id, [FromBody] List<int> acceptedProducts, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.ApproveAsync(id, acceptedProducts, updatedBy);

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

        // POST api/values/end/5
        [HttpPost("Reject/{id}")]
        public async Task<ActionResult> Reject(int id, [FromBody] RejectReason rejectReason, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.RejectAsync(id, rejectReason.Reason, updatedBy);

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
    }
}
