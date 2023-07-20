using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class BillBusiness : SapB1GenericRepository<Bill>, IBillBusiness
    {
        private readonly IBusinessPartnerBusiness _businessPartnerRepository;
        private readonly ICurrencyBusiness _currencyRepository;
        private readonly IBusinessPartnerContactBusiness _businessPartnerContactRepository;
        private readonly IBusinessPartnerPaymentBusiness _businessPartnerPaymentRepository;
        private readonly IEmployeeBusiness _employeeRepository;
        private readonly IBusinessPartnerAddressBusiness _businessPartnerAddressRepository;
        private readonly IBillDetailBusiness _billDetailRepository;

        public BillBusiness(SapB1Context context, ISapB1AutoMapper<Bill> mapper,
            IBusinessPartnerBusiness businessPartnerRepository,
            ICurrencyBusiness currencyRepository,
            IBusinessPartnerContactBusiness businessPartnerContactRepository,
            IBusinessPartnerPaymentBusiness businessPartnerPaymentRepository,
            IEmployeeBusiness employeeRepository,
            IBusinessPartnerAddressBusiness businessPartnerAddressRepository,
            IBillDetailBusiness billDetailRepository) : base(context, mapper)
        {
            _businessPartnerRepository = businessPartnerRepository;
            _currencyRepository = currencyRepository;
            _businessPartnerContactRepository = businessPartnerContactRepository;
            _businessPartnerPaymentRepository = businessPartnerPaymentRepository;
            _employeeRepository = employeeRepository;
            _businessPartnerAddressRepository = businessPartnerAddressRepository;
            _billDetailRepository = billDetailRepository;
        }

        public async Task<ICollection<Bill>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_497", new List<dynamic> { year, month }), objectType);
        }

        public async Task<ICollection<Bill>> GetAllBySaleEmployeeIdAsync(int saleEmployeeId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_498", new List<dynamic> { saleEmployeeId, year, month }), objectType);
        }

        public async Task<ICollection<Bill>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_499", new List<dynamic> { businessPartnerId, year, month }), objectType);
        }

        public async Task<ICollection<Bill>> GetAllPendingAsync(Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_503"), objectType);
        }

        public async Task<ICollection<Bill>> GetAllPendingBySaleEmployeeIdAsync(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_504", new List<dynamic> { saleEmployeeId }), objectType);
        }

        public async Task<ICollection<Bill>> GetAllPendingByBusinessPartnerIdAsync(string businessPartnerId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_505", new List<dynamic> { businessPartnerId }), objectType);
        }

        public async Task<ICollection<Bill>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_501", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<Bill> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_500", new List<dynamic> { id }), objectType);
        }

        public async Task<Bill> SetFullProperties(Bill obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            obj.BusinessPartner = await _businessPartnerRepository.GetAsync(obj.BusinessPartnerId);

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.Currency = await _currencyRepository.GetAsync(obj.CurrencyId);
                obj.Contact = await _businessPartnerContactRepository.GetAsync(obj.ContactId);
                obj.Payment = await _businessPartnerPaymentRepository.GetAsync(obj.PaymentId);
                obj.SaleEmployee = await _employeeRepository.GetBySaleEmployeeIdAsync(obj.SaleEmployeeId, Enums.ObjectType.Only);
                obj.ShipAddress = await _businessPartnerAddressRepository.GetAsync(obj.BusinessPartnerId, obj.ShipAddressId);
                obj.BillAddress = await _businessPartnerAddressRepository.GetAsync(obj.BusinessPartnerId, obj.BillAddressId);

                if (!string.IsNullOrEmpty(obj.AgentId))
                    obj.Agent = await _businessPartnerRepository.GetTransportAgencyAsync(obj.AgentId);

                if (!string.IsNullOrEmpty(obj.AgentAddressId))
                    obj.AgentAddress = await _businessPartnerAddressRepository.GetAsync(obj.AgentId, obj.AgentAddressId);

                if (objectType == Enums.ObjectType.Full)
                {
                    obj.Details = await _billDetailRepository.GetAllAsync(obj.Id);
                    ////Files
                    //obj.Files = await _billFileRepository.GetAllAsync(obj.Id);
                }
            }

            return obj;
        }

        public async Task<ICollection<Bill>> SetFullProperties(ICollection<Bill> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            var billIds = objs.Select(x => x.Id);

            //BusinessPartner
            var businessPartnerIds = objs.GroupBy(x => x.BusinessPartnerId).Select(g => g.Key);
            var businessPartners = await _businessPartnerRepository.GetAllWithIdsAsync(businessPartnerIds);

            foreach (var businessPartner in businessPartners)
                objs.Where(x => x.BusinessPartnerId.Equals(businessPartner.Id)).ToList().ForEach(x => x.BusinessPartner = businessPartner);

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                //Currency
                var currencyIds = objs.GroupBy(x => x.CurrencyId).Select(g => g.Key);
                var Currencies = await _currencyRepository.GetAllAsync();

                foreach (var currency in Currencies)
                    objs.Where(x => x.CurrencyId.Equals(currency.Id)).ToList().ForEach(x => x.Currency = currency);

                //Contact
                var contactIds = objs.GroupBy(x => x.ContactId).Select(g => g.Key);
                var contacts = await _businessPartnerContactRepository.GetAllWithIdsAsync(contactIds);

                foreach (var contact in contacts)
                    objs.Where(x => x.ContactId.Equals(contact.Id)).ToList().ForEach(x => x.Contact = contact);

                //Payment
                var paymentIds = objs.GroupBy(x => x.PaymentId).Select(g => g.Key);
                var payments = await _businessPartnerPaymentRepository.GetAllWithIdsAsync(paymentIds);

                foreach (var payment in payments)
                    objs.Where(x => x.PaymentId.Equals(payment.Id)).ToList().ForEach(x => x.Payment = payment);

                //Employee
                var employeeIds = objs.GroupBy(x => x.SaleEmployeeId).Select(g => g.Key);
                var employees = await _employeeRepository.GetAllWithSaleEmployeeIdsAsync(employeeIds, Enums.ObjectType.Only);

                foreach (var employee in employees)
                    objs.Where(x => x.SaleEmployeeId.Equals(employee.SaleEmployeeId)).ToList().ForEach(x => x.SaleEmployee = employee);

                //ShipAddress
                foreach (var id in businessPartnerIds)
                {
                    var shipAddressIds = objs.Where(x => x.BusinessPartnerId.Equals(id)).GroupBy(x => x.ShipAddressId).Select(g => g.Key);
                    var shipAddresses = await _businessPartnerAddressRepository.GetAllWithIdsAsync(id, shipAddressIds);

                    foreach (var shipAddress in shipAddresses)
                        objs.Where(x => x.BusinessPartnerId.Equals(id) && x.ShipAddressId.Equals(shipAddress.Id)).ToList().ForEach(x => x.ShipAddress = shipAddress);
                }

                //BillAddress
                foreach (var id in businessPartnerIds)
                {
                    var billAddressIds = objs.Where(x => x.BusinessPartnerId.Equals(id)).GroupBy(x => x.BillAddressId).Select(g => g.Key);
                    var billAddresses = await _businessPartnerAddressRepository.GetAllWithIdsAsync(id, billAddressIds);

                    foreach (var billAddress in billAddresses)
                        objs.Where(x => x.BusinessPartnerId.Equals(id) && x.BillAddressId.Equals(billAddress.Id)).ToList().ForEach(x => x.BillAddress = billAddress);
                }

                //Agent
                var agentIds = objs.GroupBy(x => x.AgentId).Select(g => g.Key);
                var agents = await _businessPartnerRepository.GetTransportAgencyAllWithIdsAsync(agentIds);

                foreach (var agent in agents)
                    objs.Where(x => x.AgentId.Equals(agent.Id)).ToList().ForEach(x => x.Agent = agent);

                //Agent Address
                foreach (var id in agentIds)
                {
                    var agentAddressIds = objs.Where(x => x.AgentId.Equals(id)).GroupBy(x => x.AgentAddressId).Select(g => g.Key);
                    var agentAddresses = await _businessPartnerAddressRepository.GetAllWithIdsAsync(id, agentAddressIds);

                    foreach (var agentAddress in agentAddresses)
                        objs.Where(x => x.AgentId.Equals(id) && x.AgentAddressId.Equals(agentAddress.Id)).ToList().ForEach(x => x.AgentAddress = agentAddress);
                }

                if (objectType == Enums.ObjectType.Full)
                {
                    //Details
                    var details = await _billDetailRepository.GetAllWithIdsAsync(billIds);
                    foreach (var bill in objs)
                        bill.Details = details.Where(x => x.Id.Equals(bill.Id)).ToList();

                    ////Files
                    //var files = await _billFileRepository.GetAllWithIdsAsync(billIds);
                    //foreach (var bill in objs)
                    //    bill.Files = files.Where(x => x.Id.Equals(bill.Id)).ToList();
                }
            }

            return objs;
        }
    }
}
