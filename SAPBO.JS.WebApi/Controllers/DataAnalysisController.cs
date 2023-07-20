using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Dto;

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
    public class DataAnalysisController : Controller
    {
        private readonly IBilledAmountDataBusiness repository;
        private readonly ISaleGoalDataBySaleEmployeeBusiness repository2;
        private readonly ITopBilledBusinessPartnerBusiness repository3;
        private readonly ITopBilledProductBusiness repository5;
        private readonly ILogger<DataAnalysisController> logger;

        public DataAnalysisController(
            IBilledAmountDataBusiness repository, 
            ISaleGoalDataBySaleEmployeeBusiness repository2,
            ITopBilledBusinessPartnerBusiness repository3,
            ITopBilledProductBusiness repository5,
            ILogger<DataAnalysisController> logger)
        {
            this.repository = repository;
            this.repository2 = repository2;
            this.repository3 = repository3;
            this.repository5 = repository5;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet("GetBilledAmountDataByBusinessPartnerId/{businessPartnerId}", Name = "GetBilledAmountDataByBusinessPartnerId")]
        public async Task<ActionResult<DataAnalysis<BilledAmountData>>> GetBilledAmountDataByBusinessPartnerId(string businessPartnerId)
        {
            return new DataAnalysis<BilledAmountData>
            {
                BusinessPartnerId = businessPartnerId,
                UpdatedTo = DateTime.Now,
                Data = await repository.GetBilledAmountDataByBusinessPartnerIdAsync(businessPartnerId)
            };
        }

        // GET api/values
        [HttpGet("GetBilledAmountDataBySaleEmployeeId/{saleEmployeeId}", Name = "GetBilledAmountDataBySaleEmployeeId")]
        public async Task<ActionResult<DataAnalysis<BilledAmountData>>> GetBilledAmountDataBySaleEmployeeId(int saleEmployeeId)
        {
            return new DataAnalysis<BilledAmountData> { 
                SaleEmployeeId = saleEmployeeId, 
                UpdatedTo = DateTime.Now, 
                Data = await repository.GetBilledAmountDataBySaleEmployeeIdAsync(saleEmployeeId)
            };
        }

        // GET api/values
        [HttpGet("GetBilledAmountDataByProductId/{productId}", Name = "GetBilledAmountDataByProductId")]
        public async Task<ActionResult<DataAnalysis<BilledAmountData>>> GetBilledAmountDataByProductId(string productId)
        {
            return new DataAnalysis<BilledAmountData>
            {
                UpdatedTo = DateTime.Now,
                Data = await repository.GetBilledAmountDataByProductIdAsync(productId)
            };
        }

        // GET api/values
        [HttpGet("GetBilledAmountDataByProductIdAndSaleEmployeeId/{productId}", Name = "GetBilledAmountDataByProductIdAndSaleEmployeeId")]
        public async Task<ActionResult<DataAnalysis<BilledAmountData>>> GetBilledAmountDataByProductIdAndSaleEmployeeId(string productId, int saleEmployeeId)
        {
            return new DataAnalysis<BilledAmountData>
            {
                UpdatedTo = DateTime.Now,
                Data = await repository.GetBilledAmountDataByProductIdAndSaleEmployeeIdAsync(productId, saleEmployeeId)
            };
        }

        // GET api/values
        [HttpGet("GetTopBilledBusinessPartnerBySaleEmployeeId/{saleEmployeeId}", Name = "GetTopBilledBusinessPartnerBySaleEmployeeId")]
        public async Task<ActionResult<DataAnalysis<TopBilledBusinessPartner>>> GetTopBilledBusinessPartnerBySaleEmployeeId(int saleEmployeeId, int count)
        {
            return new DataAnalysis<TopBilledBusinessPartner>
            {
                SaleEmployeeId = saleEmployeeId,
                UpdatedTo = DateTime.Now,
                Data = await repository3.GetTopBilledBusinessPartnerBySaleEmployeeIdAsync(saleEmployeeId, count)
            };
        }

        // GET api/values
        [HttpGet("GetTopBilledBusinessPartnerByProductId/{productId}", Name = "GetTopBilledBusinessPartnerByProductId")]
        public async Task<ActionResult<DataAnalysis<TopBilledBusinessPartner>>> GetTopBilledBusinessPartnerByProductId(string productId, int count)
        {
            return new DataAnalysis<TopBilledBusinessPartner>
            {
                UpdatedTo = DateTime.Now,
                Data = await repository3.GetTopBilledBusinessPartnerByProductIdAsync(productId, count)
            };
        }

        // GET api/values
        [HttpGet("GetTopBilledBusinessPartnerByProductIdAndSaleEmployeeId/{productId}", Name = "GetTopBilledBusinessPartnerByProductIdAndSaleEmployeeId")]
        public async Task<ActionResult<DataAnalysis<TopBilledBusinessPartner>>> GetTopBilledBusinessPartnerByProductIdAndSaleEmployeeId(string productId, int saleEmployeeId, int count)
        {
            return new DataAnalysis<TopBilledBusinessPartner>
            {
                UpdatedTo = DateTime.Now,
                Data = await repository3.GetTopBilledBusinessPartnerByProductIdAndSaleEmployeeIdAsync(productId, saleEmployeeId, count)
            };
        }

        // GET api/values
        [HttpGet("GetSaleGoalDataBySaleEmployeeId/{saleEmployeeId}", Name = "GetSaleGoalDataBySaleEmployeeId")]
        public async Task<ActionResult<DataAnalysis<SaleGoalDataBySaleEmployee>>> GetSaleGoalDataBySaleEmployeeId(int saleEmployeeId)
        {
            return new DataAnalysis<SaleGoalDataBySaleEmployee>
            {
                SaleEmployeeId = saleEmployeeId,
                UpdatedTo = DateTime.Now,
                Data = await repository2.GetSaleGoalDataBySaleEmployeeIdAsync(saleEmployeeId)
            };
        }

        // GET api/values
        [HttpGet("GetTopBilledProduct", Name = "GetTopBilledProduct")]
        public async Task<ActionResult<DataAnalysis<TopBilledProduct>>> GetTopBilledProduct(int count)
        {
            return new DataAnalysis<TopBilledProduct>
            {
                UpdatedTo = DateTime.Now,
                Data = await repository5.GetTopBilledProductAsync(count)
            };
        }

        // GET api/values
        [HttpGet("GetTopBilledProductBySaleEmployeeId/{saleEmployeeId}", Name = "GetTopBilledProductBySaleEmployeeId")]
        public async Task<ActionResult<DataAnalysis<TopBilledProduct>>> GetTopBilledProductBySaleEmployeeId(int saleEmployeeId, int count)
        {
            return new DataAnalysis<TopBilledProduct>
            {
                SaleEmployeeId = saleEmployeeId,
                UpdatedTo = DateTime.Now,
                Data = await repository5.GetTopBilledProductBySaleEmployeeIdAsync(saleEmployeeId, count)
            };
        }

        // GET api/values
        [HttpGet("GetTopBilledProductByBusinessPartnerId/{businessPartnerId}", Name = "GetTopBilledProductByBusinessPartnerId")]
        public async Task<ActionResult<DataAnalysis<TopBilledProduct>>> GetTopBilledProductByBusinessPartnerId(string businessPartnerId, int count)
        {
            return new DataAnalysis<TopBilledProduct>
            {
                BusinessPartnerId = businessPartnerId,
                UpdatedTo = DateTime.Now,
                Data = await repository5.GetTopBilledProductByBusinessPartnerIdAsync(businessPartnerId, count)
            };
        }
    }
}
