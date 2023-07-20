using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;
using System.Net.Mail;

namespace SAPBO.JS.Business
{
    public class BusinessPartnerBusiness : SapB1GenericRepository<BusinessPartner>, IBusinessPartnerBusiness
    {
        private readonly IEmployeeBusiness _employeeRepository;
        private readonly IBusinessPartnerContactBusiness _businessPartnerContactRepository;
        private readonly IBusinessPartnerPaymentBusiness _businessPartnerPaymentRepository;
        private readonly IBusinessPartnerAddressBusiness _businessPartnerAddressRepository;
        private readonly IBusinessPartnerDriverBusiness _businessPartnerDriverRepository;
        private readonly IBusinessPartnerVehicleBusiness _businessPartnerVehicleRepository;
        private readonly ICRMActivityBusiness _cRMActivityRepository;
        private readonly ISaleOpportunityBusiness _saleOpportunityRepository;
        private readonly IEmailBusiness _emailBusinessRepository;
        private const string _tableName = TableNames.BusinessPartnerTemp;

        public BusinessPartnerBusiness(SapB1Context context, ISapB1AutoMapper<BusinessPartner> mapper,
            IEmployeeBusiness employeeRepository,
            IBusinessPartnerContactBusiness businessPartnerContactRepository,
            IBusinessPartnerPaymentBusiness businessPartnerPaymentRepository,
            IBusinessPartnerAddressBusiness businessPartnerAddressRepository,
            IBusinessPartnerDriverBusiness businessPartnerDriverRepository,
            IBusinessPartnerVehicleBusiness businessPartnerVehicleRepository,
            ICRMActivityBusiness cRMActivityRepository,
            ISaleOpportunityBusiness saleOpportunityRepository,
            IEmailBusiness emailBusinessRepository
            ) : base(context, mapper)
        {
            _employeeRepository = employeeRepository;
            _businessPartnerContactRepository = businessPartnerContactRepository;
            _businessPartnerPaymentRepository = businessPartnerPaymentRepository;
            _businessPartnerAddressRepository = businessPartnerAddressRepository;
            _businessPartnerDriverRepository = businessPartnerDriverRepository;
            _businessPartnerVehicleRepository = businessPartnerVehicleRepository;
            _cRMActivityRepository = cRMActivityRepository;
            _saleOpportunityRepository = saleOpportunityRepository;
            _emailBusinessRepository = emailBusinessRepository;
        }

        #region Customers

        public async Task<ICollection<BusinessPartner>> GetAllAsync(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var searchText = "%";
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_051", new List<dynamic> { searchText }), objectType);
        }

        public async Task<ICollection<BusinessPartner>> GetAllWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_411", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<ICollection<BusinessPartner>> GetAllBySaleEmployeeIdAsync(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only, bool includeLeads = false)
        {
            var searchText = "%";
            if (includeLeads)
            {
                return await SetFullProperties(await GetAllAsync("GP_WEB_APP_472", new List<dynamic> { saleEmployeeId, searchText }), objectType);
            }
            else
            {
                return await SetFullProperties(await GetAllAsync("GP_WEB_APP_006", new List<dynamic> { saleEmployeeId, searchText }), objectType);
            }
        }

        public async Task<BusinessPartner> GetAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            //return await SetFullProperties(await GetAsync("GP_WEB_APP_007", new List<dynamic> { businessPartnerId }), objectType);
            return await SetFullProperties(await GetAsync("GP_WEB_APP_473", new List<dynamic> { businessPartnerId }), objectType);
        }

        public async Task<BusinessPartner> GetByRUCAsync(string ruc, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_291", new List<dynamic> { ruc }), objectType);
        }

        //INCLUDE TEMP
        public async Task<ICollection<BusinessPartner>> GetAllWithTempAsync(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_383"), objectType);
        }

        //INCLUDE TEMP
        public async Task<BusinessPartner> GetWithTempAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_446", new List<dynamic> { businessPartnerId }), objectType);
        }

        //INCLUDE TEMP
        public async Task<ICollection<BusinessPartner>> GetAllWithTempWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_447", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        //INCLUDE TEMP
        public async Task<ICollection<BusinessPartner>> GetAllWithTempBySaleEmployeeIdAsync(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_384", new List<dynamic> { saleEmployeeId }), objectType);
        }

        public Task<int> GetCountBySaleEmployeeIdAsync(int saleEmployeeId)
        {
            return Task.FromResult(GetValue("GP_WEB_APP_488", "NRO_CU", new List<dynamic> { saleEmployeeId }));
        }

        #endregion

        #region Credits

        public async Task<decimal> GetCreditLineAvailableAsync(string businessPartnerId)
        {
            decimal creditLineAvailable = await GetValueAsync("GP_WEB_APP_038", "CreditLineAvailable", new List<dynamic> { businessPartnerId });
            return creditLineAvailable;
        }

        public async Task<string> GetCreditStatusAsync(string businessPartnerId)
        {
            string creditStatus = await GetValueAsync("GP_WEB_APP_052", "BlockStatus", new List<dynamic> { businessPartnerId });
            return creditStatus;
        }

        #endregion

        #region Providers

        public async Task<ICollection<BusinessPartner>> GetProviderAllAsync(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_406"), objectType, Enums.BusinessPartnerType.Supplier);
        }

        public async Task<ICollection<BusinessPartner>> GetProviderAllWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_401", new List<dynamic> { string.Join(",", ids) }), objectType, Enums.BusinessPartnerType.Supplier);
        }

        public async Task<BusinessPartner> GetProviderAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_407", new List<dynamic> { businessPartnerId }), objectType, Enums.BusinessPartnerType.Supplier);
        }

        #endregion

        #region Carriers

        public async Task<ICollection<BusinessPartner>> GetCarrierAllAsync(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var searchText = "%";
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_149", new List<dynamic> { searchText }), objectType, Enums.BusinessPartnerType.Supplier);
        }

        public async Task<ICollection<BusinessPartner>> GetCarrierAllWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_412", new List<dynamic> { string.Join(",", ids) }), objectType, Enums.BusinessPartnerType.Supplier);
        }

        public async Task<BusinessPartner> GetCarrierAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_154", new List<dynamic> { businessPartnerId }), objectType, Enums.BusinessPartnerType.Supplier);
        }

        #endregion

        #region Temp

        public async Task<ICollection<BusinessPartner>> GetTempAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(statusType == Enums.StatusType.Todos
                ? await GetAllAsync("GP_WEB_APP_296")
                : await GetAllAsync("GP_WEB_APP_295", new List<dynamic> { (int)statusType }), objectType);
        }

        public async Task<ICollection<BusinessPartner>> GetTempAllBySaleEmployeeIdAsync(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_297", new List<dynamic> { saleEmployeeId }), objectType);
        }

        public async Task<BusinessPartner> GetTempAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_294", new List<dynamic> { businessPartnerId }), objectType);
        }

        public async Task<BusinessPartner> GetTempByRUCAsync(string ruc, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_292", new List<dynamic> { ruc }), objectType);
        }

        public async Task CreateAsync(BusinessPartner obj)
        {
            await CheckRulesAsync(obj, Enums.ObjectAction.Insert);

            obj.Id = $"CL{obj.Ruc}";
            obj.CreatedAt = DateTime.Now;

            await CreateAsync(_tableName, obj, obj.Id.ToString());

            var email = await _emailBusinessRepository.GetByGroupIdAsync("BPB001");
            email.Subject = string.Format(AppMessages.BusinessPartnerTempCreatedSubject, obj.Id);
            email.Body = string.Format(AppMessages.BusinessPartnerTempCreatedBody, obj.CreatedBy, obj.Id, obj.Ruc, obj.Name, obj.CreatedAt.Value);
            email.Cc.Add(new MailAddress(obj.CreatedBy, obj.CreatedBy));
            _emailBusinessRepository.SendEmailAsync(email);
        }

        public async Task UpdateAsync(BusinessPartner obj)
        {
            //Get obj
            var currentObj = await GetTempAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            await CheckRulesAsync(obj, Enums.ObjectAction.Update);

            //Set obj
            currentObj.UpdatedBy = obj.UpdatedBy;

            currentObj.Name = obj.Name;
            currentObj.SaleEmployeeId = obj.SaleEmployeeId;
            currentObj.Email = obj.Email;
            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        private async Task CheckRulesAsync(BusinessPartner obj, Enums.ObjectAction objectAction)
        {
            switch (objectAction)
            {
                case Enums.ObjectAction.Insert:
                    //Check User - Create
                    if (string.IsNullOrEmpty(obj.CreatedBy))
                        throw new Exception(AppMessages.UserError);

                    break;
                case Enums.ObjectAction.Update:
                    //Check User - Update
                    if (string.IsNullOrEmpty(obj.UpdatedBy))
                        throw new Exception(AppMessages.UserError);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectAction), objectAction, null);
            }

            var bp = await GetByRUCAsync(obj.Ruc, Enums.ObjectType.FullHeader);
            if (bp != null)
                throw new Exception(string.Format(AppMessages.BusinessPartnerTempExist, bp.Employee.FullName));

            bp = await GetTempByRUCAsync(obj.Ruc, Enums.ObjectType.FullHeader);
            if (bp != null)
                throw new Exception(string.Format(AppMessages.BusinessPartnerTempExist, bp.Employee.FullName));
        }

        #endregion

        #region TransportAgencies

        public async Task<ICollection<BusinessPartner>> GetTransportAgencyAllAsync(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var searchText = "%";
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_040", new List<dynamic> { searchText }), objectType, Enums.BusinessPartnerType.Supplier);
        }

        public async Task<ICollection<BusinessPartner>> GetTransportAgencyAllWithIdsAsync(IEnumerable<string> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_469", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<BusinessPartner> GetTransportAgencyAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_041", new List<dynamic> { businessPartnerId }), objectType, Enums.BusinessPartnerType.Supplier);
        }

        #endregion

        public async Task<BusinessPartner> SetFullProperties(BusinessPartner obj, Enums.ObjectType objectType, Enums.BusinessPartnerType businessPartnerType = Enums.BusinessPartnerType.Customer)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader || objectType == Enums.ObjectType.Custom)
            {
                if (businessPartnerType == Enums.BusinessPartnerType.Customer)
                {
                    obj.Employee = await _employeeRepository.GetBySaleEmployeeIdAsync(obj.SaleEmployeeId);
                    obj.EmployeeId = obj.Employee.Id;
                }

                if (objectType == Enums.ObjectType.Custom || objectType == Enums.ObjectType.Full)
                {
                    obj.Contacts = await _businessPartnerContactRepository.GetAllAsync(obj.Id);
                    obj.Payments = await _businessPartnerPaymentRepository.GetAllAsync(obj.Id);
                    obj.Addresses = await _businessPartnerAddressRepository.GetAllAsync(obj.Id);

                    if (businessPartnerType == Enums.BusinessPartnerType.Customer && objectType == Enums.ObjectType.Full)
                    {
                        obj.Activities = await _cRMActivityRepository.GetAllAsync(obj.Id);
                        obj.Opportunities = await _saleOpportunityRepository.GetAllAsync(obj.Id);
                    }

                    if (businessPartnerType == Enums.BusinessPartnerType.Supplier && objectType == Enums.ObjectType.Full)
                    {
                        obj.Vehicles = await _businessPartnerVehicleRepository.GetAllAsync(obj.Id);
                        obj.Drivers = await _businessPartnerDriverRepository.GetAllAsync(obj.Id);
                    }
                }
            }

            return obj;
        }

        public async Task<ICollection<BusinessPartner>> SetFullProperties(ICollection<BusinessPartner> objs, Enums.ObjectType objectType, Enums.BusinessPartnerType businessPartnerType = Enums.BusinessPartnerType.Customer)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader || objectType == Enums.ObjectType.Custom)
            {
                if (businessPartnerType == Enums.BusinessPartnerType.Customer)
                {
                    var saleEmployeeIds = objs.GroupBy(x => x.SaleEmployeeId).Select(g => g.Key);
                    var employees = await _employeeRepository.GetAllWithSaleEmployeeIdsAsync(saleEmployeeIds);

                    foreach (var employee in employees)
                        objs.Where(x => x.SaleEmployeeId.Equals(employee.SaleEmployeeId)).ToList().ForEach(x => {
                            x.EmployeeId = employee.Id;
                            x.Employee = employee;
                        });
                }

                if (objectType == Enums.ObjectType.Custom || objectType == Enums.ObjectType.Full)
                {
                    var businessPartnerIds = objs.GroupBy(x => x.Id).Select(g => g.Key);

                    foreach (var obj in objs)
                    {
                        obj.Contacts = await _businessPartnerContactRepository.GetAllAsync(obj.Id);
                        obj.Payments = await _businessPartnerPaymentRepository.GetAllAsync(obj.Id);
                        obj.Addresses = await _businessPartnerAddressRepository.GetAllAsync(obj.Id);

                        if (businessPartnerType == Enums.BusinessPartnerType.Customer && objectType == Enums.ObjectType.Full)
                        {
                            obj.Activities = await _cRMActivityRepository.GetAllAsync(obj.Id);
                            obj.Opportunities = await _saleOpportunityRepository.GetAllAsync(obj.Id);
                        }

                        if (businessPartnerType == Enums.BusinessPartnerType.Supplier && objectType == Enums.ObjectType.Full)
                        {
                            obj.Drivers = await _businessPartnerDriverRepository.GetAllAsync(obj.Id);
                            obj.Vehicles = await _businessPartnerVehicleRepository.GetAllAsync(obj.Id);
                        }
                    }
                }
            }

            return objs;
        }
    }
}
