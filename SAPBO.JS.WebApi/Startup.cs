using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Model.App;
using SAPBO.JS.Model.Auth;
using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Dto;
using SAPBO.JS.WebApi.Utilities;
using System.Text;

namespace SAPBO.JS.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddCors(options => {
                options.AddDefaultPolicy(builder => {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            services.AddTransient<IFileStorage, LocalFileStorage>();
            services.AddHttpContextAccessor();

            services.AddDbContext<SecurityDbContext>(options =>
                options.UseSqlServer(Configuration[AppConfiguration.SecurityConnectionName]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SecurityDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration[AppConfiguration.JwtKeyName])),
                    ClockSkew = TimeSpan.Zero
                });

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SAPBO.JS.WebApi", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { }
                    }
                });
            });

            services.Configure<SmtpClientSetting>(smtp =>
            {
                smtp.Host = Configuration[AppConfiguration.SmtpClientHost];
                smtp.Port = int.Parse(Configuration[AppConfiguration.SmtpClientPort]);
                smtp.EmailAddress = Configuration[AppConfiguration.EmailUserId];
                smtp.Password = Configuration[AppConfiguration.EmailPassword];
                smtp.EmailFrom = Configuration[AppConfiguration.EmailFrom];
                smtp.DisplayName = Configuration[AppConfiguration.EmailName];
            });

            services.AddScoped<SapB1Context>();

            //Add Mappers
            services.AddScoped<ISapB1AutoMapper<BusinessPartnerAddress>, BusinessPartnerAddressMapper>();
            services.AddScoped<ISapB1AutoMapper<BusinessPartnerContact>, BusinessPartnerContactMapper>();
            services.AddScoped<ISapB1AutoMapper<BusinessPartnerPayment>, BusinessPartnerPaymentMapper>();
            services.AddScoped<ISapB1AutoMapper<BusinessPartnerVehicle>, BusinessPartnerVehicleMapper>();
            services.AddScoped<ISapB1AutoMapper<BusinessPartnerDriver>, BusinessPartnerDriverMapper>();
            services.AddScoped<ISapB1AutoMapper<BusinessPartner>, BusinessPartnerMapper>();
            services.AddScoped<ISapB1AutoMapper<BusinessUnit>, BusinessUnitMapper>();
            services.AddScoped<ISapB1AutoMapper<Employee>, EmployeeMapper>();
            services.AddScoped<ISapB1AutoMapper<FailureCause>, FailureCauseMapper>();
            services.AddScoped<ISapB1AutoMapper<FailureImpact>, FailureImpactMapper>();
            services.AddScoped<ISapB1AutoMapper<FailureMechanism>, FailureMechanismMapper>();
            services.AddScoped<ISapB1AutoMapper<FailureSeverity>, FailureSeverityMapper>();
            services.AddScoped<ISapB1AutoMapper<FailureType>, FailureTypeMapper>();
            services.AddScoped<ISapB1AutoMapper<Job>, JobMapper>();
            services.AddScoped<ISapB1AutoMapper<MachineFailure>, MachineFailureMapper>();
            services.AddScoped<ISapB1AutoMapper<MaintenancePriority>, MaintenancePriorityMapper>();
            services.AddScoped<ISapB1AutoMapper<MaintenanceProgram>, MaintenanceProgramMapper>();
            services.AddScoped<ISapB1AutoMapper<MaintenanceProgramJob>, MaintenanceProgramJobMapper>();
            services.AddScoped<ISapB1AutoMapper<MaintenanceProgramReplacement>, MaintenanceProgramReplacementMapper>();
            services.AddScoped<ISapB1AutoMapper<MaintenanceProgramTool>, MaintenanceProgramToolMapper>();
            services.AddScoped<ISapB1AutoMapper<MaintenanceTool>, MaintenanceToolMapper>();
            services.AddScoped<ISapB1AutoMapper<MaintenanceType>, MaintenanceTypeMapper>();
            services.AddScoped<ISapB1AutoMapper<MaintenanceWorkOrder>, MaintenanceWorkOrderMapper>();
            services.AddScoped<ISapB1AutoMapper<MaintenanceWorkOrderEmployee>, MaintenanceWorkOrderEmployeeMapper>();
            services.AddScoped<ISapB1AutoMapper<MaintenanceWorkOrderReplacement>, MaintenanceWorkOrderReplacementMapper>();
            services.AddScoped<ISapB1AutoMapper<MaintenanceWorkOrderTool>, MaintenanceWorkOrderToolMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductionMachine>, ProductionMachineMapper>();
            services.AddScoped<ISapB1AutoMapper<Replacement>, ReplacementMapper>();
            services.AddScoped<ISapB1AutoMapper<TimeFrequency>, TimeFrequencyMapper>();
            services.AddScoped<ISapB1AutoMapper<Country>, CountryMapper>();
            services.AddScoped<ISapB1AutoMapper<State>, StateMapper>();
            services.AddScoped<ISapB1AutoMapper<CRMActivity>, CRMActivityMapper>();
            services.AddScoped<ISapB1AutoMapper<OpportunityStage>, OpportunityStageMapper>();
            services.AddScoped<ISapB1AutoMapper<OpportunityLossReason>, OpportunityLossReasonMapper>();
            services.AddScoped<ISapB1AutoMapper<SaleOpportunity>, SaleOpportunityMapper>();
            services.AddScoped<ISapB1AutoMapper<SaleOpportunityLossReason>, SaleOpportunityLossReasonMapper>();
            services.AddScoped<ISapB1AutoMapper<SaleOpportunityStage>, SaleOpportunityStageMapper>();
            services.AddScoped<ISapB1AutoMapper<PurchaseOrderAuthorization>, PurchaseOrderAuthorizationMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductClass>, ProductClassMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductSuperGroup>, ProductSuperGroupMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductGroup>, ProductGroupMapper>();
            services.AddScoped<ISapB1AutoMapper<Warehouse>, WarehouseMapper>();
            services.AddScoped<ISapB1AutoMapper<ShoppingCartItem>, ShoppingCartItemMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductPrice>, ProductPriceMapper>();
            services.AddScoped<ISapB1AutoMapper<Product>, ProductMapper>();
            services.AddScoped<ISapB1AutoMapper<Currency>, CurrencyMapper>();
            services.AddScoped<ISapB1AutoMapper<SaleOrder>, SaleOrderMapper>();
            services.AddScoped<ISapB1AutoMapper<SaleOrderDetail>, SaleOrderDetailMapper>();
            services.AddScoped<ISapB1AutoMapper<PurchaseOrder>, PurchaseOrderMapper>();
            services.AddScoped<ISapB1AutoMapper<PurchaseOrderDetail>, PurchaseOrderDetailMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductQuantityDiscount>, ProductQuantityDiscountMapper>();
            services.AddScoped<ISapB1AutoMapper<Rate>, RateMapper>();
            services.AddScoped<ISapB1AutoMapper<QuotationAccessory>, QuotationAccessoryMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductFormat>, ProductFormatMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductInkLevel>, ProductInkLevelMapper>();
            services.AddScoped<ISapB1AutoMapper<QuotationIndirectSpending>, QuotationIndirectSpendingMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductionProcessTypeCost>, ProductionProcessTypeCostMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductGrammage>, ProductGrammageMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductMaterialType>, ProductMaterialTypeMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductMaterial>, ProductMaterialMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductionProcess>, ProductionProcessMapper>();
            services.AddScoped<ISapB1AutoMapper<UnitOfMeasurement>, UnitOfMeasurementMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductFormula>, ProductFormulaMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductFormulaConsumptionFactor>, ProductFormulaConsumptionFactorMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductFormulaProductionProcess>, ProductFormulaProductionProcessMapper>();
            services.AddScoped<ISapB1AutoMapper<SaleQuotation>, SaleQuotationMapper>();
            services.AddScoped<ISapB1AutoMapper<SaleQuotationDetail>, SaleQuotationDetailMapper>();
            services.AddScoped<ISapB1AutoMapper<ProductionMachineZone>, ProductionMachineZoneMapper>();
            services.AddScoped<ISapB1AutoMapper<CostCenter>, CostCenterMapper>();
            services.AddScoped<ISapB1AutoMapper<Delivery>, DeliveryMapper>();
            services.AddScoped<ISapB1AutoMapper<DeliveryDetail>, DeliveryDetailMapper>();
            services.AddScoped<ISapB1AutoMapper<AppEmailGroupItem>, AppEmailGroupItemMapper>();
            services.AddScoped<ISapB1AutoMapper<SaleOrderAuthorization>, SaleOrderAuthorizationMapper>();
            services.AddScoped<ISapB1AutoMapper<BilledAmountData>, BilledAmountDataMapper>();
            services.AddScoped<ISapB1AutoMapper<TopBilledBusinessPartner>, TopBilledBusinessPartnerMapper>();
            services.AddScoped<ISapB1AutoMapper<TopBilledProduct>, TopBilledProductMapper>();
            services.AddScoped<ISapB1AutoMapper<SaleGoalDataBySaleEmployee>, SaleGoalDataBySaleEmployeeMapper>();
            services.AddScoped<ISapB1AutoMapper<Bill>, BillMapper>();
            services.AddScoped<ISapB1AutoMapper<BillDetail>, BillDetailMapper>();
            services.AddScoped<ISapB1AutoMapper<BillFile>, BillFileMapper>();
            services.AddScoped<ISapB1AutoMapper<ContactMessage>, ContactMessageMapper>();

            //Add Services
            services.AddScoped<IUserBusiness, UserBusiness>();
            services.AddScoped<IBusinessPartnerAddressBusiness, BusinessPartnerAddressBusiness>();
            services.AddScoped<IBusinessPartnerContactBusiness, BusinessPartnerContactBusiness>();
            services.AddScoped<IBusinessPartnerPaymentBusiness, BusinessPartnerPaymentBusiness>();
            services.AddScoped<IBusinessPartnerVehicleBusiness, BusinessPartnerVehicleBusiness>();
            services.AddScoped<IBusinessPartnerDriverBusiness, BusinessPartnerDriverBusiness>();
            services.AddScoped<IBusinessPartnerBusiness, BusinessPartnerBusiness>();
            services.AddScoped<IBusinessUnitBusiness, BusinessUnitBusiness>();
            services.AddScoped<ICountryBusiness, CountryBusiness>();
            services.AddScoped<ICostCenterBusiness, CostCenterBusiness>();
            services.AddScoped<ICurrencyBusiness, CurrencyBusiness>();
            services.AddScoped<IFailureCauseBusiness, FailureCauseBusiness>();
            services.AddScoped<IFailureImpactBusiness, FailureImpactBusiness>();
            services.AddScoped<IFailureMechanismBusiness, FailureMechanismBusiness>();
            services.AddScoped<IFailureSeverityBusiness, FailureSeverityBusiness>();
            services.AddScoped<IFailureTypeBusiness, FailureTypeBusiness>();
            services.AddScoped<IJobBusiness, JobBusiness>();
            services.AddScoped<IMaintenancePriorityBusiness, MaintenancePriorityBusiness>();
            services.AddScoped<IMaintenanceToolBusiness, MaintenanceToolBusiness>();
            services.AddScoped<IMaintenanceTypeBusiness, MaintenanceTypeBusiness>();
            services.AddScoped<IProductClassBusiness, ProductClassBusiness>();
            services.AddScoped<IProductSuperGroupBusiness, ProductSuperGroupBusiness>();
            services.AddScoped<IProductGroupBusiness, ProductGroupBusiness>();
            services.AddScoped<IProductFormatBusiness, ProductFormatBusiness>();
            services.AddScoped<IProductGrammageBusiness, ProductGrammageBusiness>();
            services.AddScoped<IProductInkLevelBusiness, ProductInkLevelBusiness>();
            services.AddScoped<IStateBusiness, StateBusiness>();
            services.AddScoped<ITimeFrequencyBusiness, TimeFrequencyBusiness>();
            services.AddScoped<IUnitOfMeasurementBusiness, UnitOfMeasurementBusiness>();
            services.AddScoped<IWarehouseBusiness, WarehouseBusiness>();
            services.AddScoped<IEmployeeBusiness, EmployeeBusiness>();
            services.AddScoped<IMachineFailureBusiness, MachineFailureBusiness>();
            services.AddScoped<IMaintenanceProgramBusiness, MaintenanceProgramBusiness>();
            services.AddScoped<IMaintenanceProgramJobBusiness, MaintenanceProgramJobBusiness>();
            services.AddScoped<IMaintenanceProgramReplacementBusiness, MaintenanceProgramReplacementBusiness>();
            services.AddScoped<IMaintenanceProgramToolBusiness, MaintenanceProgramToolBusiness>();
            services.AddScoped<IMaintenanceWorkOrderBusiness, MaintenanceWorkOrderBusiness>();
            services.AddScoped<IMaintenanceWorkOrderEmployeeBusiness, MaintenanceWorkOrderEmployeeBusiness>();
            services.AddScoped<IMaintenanceWorkOrderReplacementBusiness, MaintenanceWorkOrderReplacementBusiness>();
            services.AddScoped<IMaintenanceWorkOrderToolBusiness, MaintenanceWorkOrderToolBusiness>();
            services.AddScoped<IProductionMachineBusiness, ProductionMachineBusiness>();
            services.AddScoped<IReplacementBusiness, ReplacementBusiness>();
            services.AddScoped<ICRMActivityBusiness, CRMActivityBusiness>();
            services.AddScoped<IOpportunityStageBusiness, OpportunityStageBusiness>();
            services.AddScoped<IOpportunityLossReasonBusiness, OpportunityLossReasonBusiness>();
            services.AddScoped<ISaleOpportunityBusiness, SaleOpportunityBusiness>();
            services.AddScoped<ISaleOpportunityLossReasonBusiness, SaleOpportunityLossReasonBusiness>();
            services.AddScoped<ISaleOpportunityStageBusiness, SaleOpportunityStageBusiness>();
            services.AddScoped<IPurchaseOrderAuthorizationBusiness, PurchaseOrderAuthorizationBusiness>();
            services.AddScoped<IShoppingCartItemBusiness, ShoppingCartItemBusiness>();
            services.AddScoped<IProductPriceBusiness, ProductPriceBusiness>();
            services.AddScoped<IProductBusiness, ProductBusiness>();
            services.AddScoped<ISaleOrderBusiness, SaleOrderBusiness>();
            services.AddScoped<ISaleOrderDetailBusiness, SaleOrderDetailBusiness>();
            services.AddScoped<IPurchaseOrderBusiness, PurchaseOrderBusiness>();
            services.AddScoped<IPurchaseOrderDetailBusiness, PurchaseOrderDetailBusiness>();
            services.AddScoped<IProductQuantityDiscountBusiness, ProductQuantityDiscountBusiness>();
            services.AddScoped<IRateBusiness, RateBusiness>();
            services.AddScoped<IQuotationAccessoryBusiness, QuotationAccessoryBusiness>();
            services.AddScoped<IQuotationIndirectSpendingBusiness, QuotationIndirectSpendingBusiness>();
            services.AddScoped<IProductionProcessTypeCostBusiness, ProductionProcessTypeCostBusiness>();
            services.AddScoped<IProductMaterialTypeBusiness, ProductMaterialTypeBusiness>();
            services.AddScoped<IProductMaterialBusiness, ProductMaterialBusiness>();
            services.AddScoped<IProductionProcessBusiness, ProductionProcessBusiness>();
            services.AddScoped<IProductFormulaBusiness, ProductFormulaBusiness>();
            services.AddScoped<IProductFormulaConsumptionFactorBusiness, ProductFormulaConsumptionFactorBusiness>();
            services.AddScoped<IProductFormulaProductionProcessBusiness, ProductFormulaProductionProcessBusiness>();
            services.AddScoped<ISaleQuotationBusiness, SaleQuotationBusiness>();
            services.AddScoped<ISaleQuotationDetailBusiness, SaleQuotationDetailBusiness>();
            services.AddScoped<IProductionMachineZoneBusiness, ProductionMachineZoneBusiness>();
            services.AddScoped<IDeliveryBusiness, DeliveryBusiness>();
            services.AddScoped<IDeliveryDetailBusiness, DeliveryDetailBusiness>();
            services.AddScoped<IEmailBusiness, EmailBusiness>();
            services.AddScoped<ISaleOrderAuthorizationBusiness, SaleOrderAuthorizationBusiness>();
            services.AddScoped<IBilledAmountDataBusiness, BilledAmountDataBusiness>();
            services.AddScoped<ITopBilledBusinessPartnerBusiness, TopBilledBusinessPartnerBusiness>();
            services.AddScoped<ITopBilledProductBusiness, TopBilledProductBusiness>();
            services.AddScoped<ISaleGoalDataBySaleEmployeeBusiness, SaleGoalDataBySaleEmployeeBusiness>();
            services.AddScoped<IBillBusiness, BillBusiness>();
            services.AddScoped<IBillDetailBusiness, BillDetailBusiness>();
            services.AddScoped<IBillFileBusiness, BillFileBusiness>();
            services.AddScoped<IContactMessageBusiness, ContactMessageBusiness>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
