using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;
using static SAPBO.JS.Common.Enums;
using System;

namespace SAPBO.JS.Business
{
    public class SaleOrderBusiness : SaleOrderRepository, ISaleOrderBusiness
    {
        private readonly IBusinessPartnerBusiness _businessPartnerRepository;
        private readonly ICurrencyBusiness _currencyRepository;
        private readonly IBusinessPartnerContactBusiness _businessPartnerContactRepository;
        private readonly IBusinessPartnerPaymentBusiness _businessPartnerPaymentRepository;
        private readonly IEmployeeBusiness _employeeRepository;
        private readonly IBusinessPartnerAddressBusiness _businessPartnerAddressRepository;
        private readonly ISaleOrderDetailBusiness _saleOrderDetailRepository;
        private readonly IRateBusiness _rateRepository;
        private readonly IShoppingCartItemBusiness _shoppingCartItemRepository;
        private readonly IDeliveryBusiness _deliveryRepository;

        public SaleOrderBusiness(SapB1Context context, ISapB1AutoMapper<SaleOrder> mapper,
            IBusinessPartnerBusiness businessPartnerRepository,
            ICurrencyBusiness currencyRepository,
            IBusinessPartnerContactBusiness businessPartnerContactRepository,
            IBusinessPartnerPaymentBusiness businessPartnerPaymentRepository,
            IEmployeeBusiness employeeRepository,
            IBusinessPartnerAddressBusiness businessPartnerAddressRepository,
            ISaleOrderDetailBusiness saleOrderDetailRepository,
            IRateBusiness rateRepository,
            IShoppingCartItemBusiness shoppingCartItemRepository,
            IDeliveryBusiness deliveryRepository) : base(context, mapper)
        {
            _businessPartnerRepository = businessPartnerRepository;
            _currencyRepository = currencyRepository;
            _businessPartnerContactRepository = businessPartnerContactRepository;
            _businessPartnerPaymentRepository = businessPartnerPaymentRepository;
            _employeeRepository = employeeRepository;
            _businessPartnerAddressRepository = businessPartnerAddressRepository;
            _saleOrderDetailRepository = saleOrderDetailRepository;
            _rateRepository = rateRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _deliveryRepository = deliveryRepository;
        }

        public async Task<ICollection<SaleOrder>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_413", new List<dynamic> { year, month }), objectType);
        }

        public async Task<ICollection<SaleOrder>> GetAllPendingAsync()
        {
            var details = await _saleOrderDetailRepository.GetAllPendingAsync();
            if (details == null || !details.Any())
                return null;
            return await SetHeaderProperties(details);
        }

        public async Task<ICollection<SaleOrder>> GetAllBySaleEmployeeIdAsync(int saleEmployeeId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_414", new List<dynamic> { saleEmployeeId, year, month }), objectType);
        }

        public async Task<ICollection<SaleOrder>> GetAllPendingBySaleEmployeeIdAsync(int saleEmployeeId)
        {
            var details = await _saleOrderDetailRepository.GetAllPendingBySaleEmployeeIdAsync(saleEmployeeId);
            if (details == null || !details.Any())
                return null;
            return await SetHeaderProperties(details);
        }

        public Task<int> GetCountBySaleEmployeeIdAsync(int saleEmployeeId)
        {
            return Task.FromResult(GetValue("GP_WEB_APP_486", "NRO_PD", new List<dynamic> { saleEmployeeId }));
        }

        public Task<int> GetCountByBusinessPartnerIdAsync(string businessPartnerId)
        {
            return Task.FromResult(GetValue("GP_WEB_APP_515", "NRO_PD", new List<dynamic> { businessPartnerId }));
        }

        public async Task<ICollection<SaleOrder>> GetTopByBusinessPartnerIdAsync(string businessPartnerId, int count)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_516", new List<dynamic> { businessPartnerId, count }), ObjectType.Only);
        }

        public async Task<ICollection<SaleOrder>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_415", new List<dynamic> { businessPartnerId, year, month }), objectType);
        }

        public async Task<ICollection<SaleOrder>> GetAllPendingByBusinessPartnerIdAsync(string businessPartnerId)
        {
            var details = await _saleOrderDetailRepository.GetAllPendingByBusinessPartnerIdAsync(businessPartnerId);
            if (details == null || !details.Any())
                return null;
            return await SetHeaderProperties(details);
        }

        public async Task<ICollection<SaleOrder>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_477", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<SaleOrder> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_416", new List<dynamic> { id }), objectType);
        }

        private dynamic IsUniqueBpReferenceNumber(string businessPartnerId, string bpReferenceNumber)
        {
            return GetValue("GP_WEB_APP_057", "Unique", new List<dynamic> { businessPartnerId, bpReferenceNumber });
        }

        public async Task CreateAsync(ShoppingCart obj)
        {
            //Validations:

            var systemDate = DateTime.Now;

            //Check DeliveryDate
            if (obj.DeliveryDate < systemDate)
                throw new Exception(AppMessages.SaleOrder_DeliveryDate);

            //Check CurrencyId
            if (string.IsNullOrEmpty(obj.CurrencyId))
                throw new Exception(AppMessages.SaleOrder_CurrencyId);

            if (!obj.CurrencyId.Equals(AppDefaultValues.CurrencyIdDolar) && !obj.CurrencyId.Equals(AppDefaultValues.CurrencyIdSol))
                throw new Exception(AppMessages.CurrencyIdNotValid);

            //Check Rate
            var rate = await _rateRepository.GetByDateAndCurrencyIdAsync(obj.RateDate, AppDefaultValues.CurrencyIdDolar);
            if (rate == null)
                throw new Exception(AppMessages.RateNotFound);

            obj.Rate = obj.CurrencyId.Equals(AppDefaultValues.CurrencyIdDolar) ? rate.Value : AppDefaultValues.RateForSolCurrency;

            //Check BpReferenceNumber Is Unique
            if (!string.IsNullOrEmpty(obj.BpReferenceNumber))
            {
                var isUnique = IsUniqueBpReferenceNumber(obj.BusinessPartnerId, obj.BpReferenceNumber);
                if (Convert.ToString(isUnique) == "N")
                    throw new Exception(AppMessages.SaleOrder_BpReferenceNumberIsNotUnique);
            }

            //Check BusinessPartner
            var bp = await _businessPartnerRepository.GetAsync(obj.BusinessPartnerId, Enums.ObjectType.Custom);

            if (bp == null)
                throw new Exception(AppMessages.BusinessPartnerNotFound);

            //Check ContactId
            if (obj.ContactId.Equals(0))
                throw new Exception(AppMessages.SaleOrder_ContactId);

            if (!bp.Contacts.Any(x => x.Id.Equals(obj.ContactId)))
                throw new Exception(AppMessages.BusinessPartnerContactNotFound);

            //Check PaymentId
            if (obj.PaymentId.Equals(0))
                throw new Exception(AppMessages.SaleOrder_PaymentId);

            if (!bp.Payments.Any(x => x.Id.Equals(obj.PaymentId)))
                throw new Exception(AppMessages.BusinessPartnerPaymentNotFound);

            obj.BusinessPartner = bp;

            //Check ShipAddress
            obj.ShipAddress = await _businessPartnerAddressRepository.GetAsync(obj.BusinessPartnerId, obj.ShipAddressId);
            if (obj.ShipAddress == null)
                throw new Exception(AppMessages.BusinessPartnerAddressNotFound);

            //Check Details
            obj.ShoppingCartItems = await _shoppingCartItemRepository.GetAllAsync(obj.CreatedBy, obj.RateDate, obj.BusinessPartnerId, obj.CurrencyId);
            if (!obj.ShoppingCartItems.Any())
                throw new Exception(AppMessages.SaleOrder_Products);

            //Check CreditLine
            if (!obj.PaymentId.Equals(AppDefaultValues.FacturaContadoId) && !obj.PaymentId.Equals(AppDefaultValues.BoletaContadoId))
            {
                var totalSoles = obj.CurrencyId.Equals(AppDefaultValues.CurrencyIdDolar) ? decimal.Round(obj.SapTotal * rate.Value, AppFormats.Total) : obj.SapTotal;
                if (totalSoles > bp.CreditLineAvailable)
                    throw new Exception(AppMessages.SaleOrder_BusinessPartnerCreditLineError);
            }

            await Task.Run(() => Create(obj));
        }

        public async Task DeleteAsync(int id)
        {
            var obj = await GetAsync(id, Enums.ObjectType.Only);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            await Task.Run(() => Delete(id));
        }

        public Task AddFileAttachmentAsync(int saleOrderId, string path, string fileNameWithoutExtension, string fileExtensionWithoutDot)
        {
            return Task.Run(() => AddFileAttachment(saleOrderId, path, fileNameWithoutExtension, fileExtensionWithoutDot));
        }

        public async Task<ICollection<SaleOrder>> SetHeaderProperties(ICollection<SaleOrderDetail> objs)
        {
            var saleOrderIds = objs.GroupBy(x => x.SaleOrderId).Select(g => g.Key);
            var saleOrders = await GetAllWithIdsAsync(saleOrderIds, Enums.ObjectType.FullHeader);
            foreach (var saleOrder in saleOrders)
                saleOrder.Details = objs.Where(x => x.SaleOrderId.Equals(saleOrder.Id)).ToList();
            return saleOrders;
        }

        public async Task<SaleOrder> SetFullProperties(SaleOrder obj, Enums.ObjectType objectType)
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
                    obj.Details = await _saleOrderDetailRepository.GetAllAsync(obj.Id);
            }

            return obj;
        }

        public async Task<ICollection<SaleOrder>> SetFullProperties(ICollection<SaleOrder> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

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
                    var saleOrderIds = objs.Select(x => x.Id);

                    var allDetails = await _saleOrderDetailRepository.GetAllWithIdsAsync(saleOrderIds);

                    foreach (var saleOrder in objs)
                        saleOrder.Details = allDetails.Where(x => x.Id.Equals(saleOrder.Id)).ToList();
                }
            }

            return objs;
        }
    }
}
