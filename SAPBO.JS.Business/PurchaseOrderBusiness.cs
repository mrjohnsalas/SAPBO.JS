using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class PurchaseOrderBusiness : SapB1GenericRepository<PurchaseOrder>, IPurchaseOrderBusiness
    {
        private readonly IBusinessPartnerBusiness _businessPartnerRepository;
        private readonly ICurrencyBusiness _currencyRepository;
        private readonly IBusinessPartnerContactBusiness _businessPartnerContactRepository;
        private readonly IBusinessPartnerPaymentBusiness _businessPartnerPaymentRepository;
        private readonly IEmployeeBusiness _employeeRepository;
        private readonly IBusinessPartnerAddressBusiness _businessPartnerAddressRepository;
        private readonly IPurchaseOrderDetailBusiness _purchaseOrderDetailRepository;

        public PurchaseOrderBusiness(SapB1Context context, ISapB1AutoMapper<PurchaseOrder> mapper,
            IBusinessPartnerBusiness businessPartnerRepository,
            ICurrencyBusiness currencyRepository,
            IBusinessPartnerContactBusiness businessPartnerContactRepository,
            IBusinessPartnerPaymentBusiness businessPartnerPaymentRepository,
            IEmployeeBusiness employeeRepository,
            IBusinessPartnerAddressBusiness businessPartnerAddressRepository,
            IPurchaseOrderDetailBusiness purchaseOrderDetailRepository) : base(context, mapper)
        {
            _businessPartnerRepository = businessPartnerRepository;
            _currencyRepository = currencyRepository;
            _businessPartnerContactRepository = businessPartnerContactRepository;
            _businessPartnerPaymentRepository = businessPartnerPaymentRepository;
            _employeeRepository = employeeRepository;
            _businessPartnerAddressRepository = businessPartnerAddressRepository;
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
        }

        public async Task<ICollection<PurchaseOrder>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_398", new List<dynamic> { year, month }), objectType);
        }

        public async Task<ICollection<PurchaseOrder>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_399", new List<dynamic> { businessPartnerId, year, month }), objectType);
        }

        public async Task<ICollection<PurchaseOrder>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_459", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<PurchaseOrder> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_400", new List<dynamic> { id }), objectType);
        }

        public async Task<PurchaseOrder> SetFullProperties(PurchaseOrder obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            obj.BusinessPartner = await _businessPartnerRepository.GetProviderAsync(obj.BusinessPartnerId);

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.Currency = await _currencyRepository.GetAsync(obj.CurrencyId);
                obj.Contact = await _businessPartnerContactRepository.GetAsync(obj.ContactId);
                obj.Payment = await _businessPartnerPaymentRepository.GetAsync(obj.PaymentId);
                obj.PurchaseEmployee = await _employeeRepository.GetByPurchaseEmployeeIdAsync(obj.PurchaseEmployeeId, Enums.ObjectType.Only);
                obj.BillAddress = await _businessPartnerAddressRepository.GetAsync(obj.BusinessPartnerId, obj.BillAddressId);

                if (objectType == Enums.ObjectType.Full)
                    obj.Details = await _purchaseOrderDetailRepository.GetAllAsync(obj.Id);
            }

            return obj;
        }

        public async Task<ICollection<PurchaseOrder>> SetFullProperties(ICollection<PurchaseOrder> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            //BusinessPartner
            var businessPartnerIds = objs.GroupBy(x => x.BusinessPartnerId).Select(g => g.Key);
            var businessPartners = await _businessPartnerRepository.GetProviderAllWithIdsAsync(businessPartnerIds);

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
                var employeeIds = objs.GroupBy(x => x.PurchaseEmployeeId).Select(g => g.Key);
                var employees = await _employeeRepository.GetAllWithPurchaseEmployeeIdsAsync(employeeIds, Enums.ObjectType.Only);

                foreach (var employee in employees)
                    objs.Where(x => x.PurchaseEmployeeId.Equals(employee.Id)).ToList().ForEach(x => x.PurchaseEmployee = employee);

                //BillAddress
                foreach (var id in businessPartnerIds)
                {
                    var billAddressIds = objs.Where(x => x.BusinessPartnerId.Equals(id)).GroupBy(x => x.BillAddressId).Select(g => g.Key);
                    var billAddresses = await _businessPartnerAddressRepository.GetAllWithIdsAsync(id, billAddressIds);

                    foreach (var billAddress in billAddresses)
                        objs.Where(x => x.BusinessPartnerId.Equals(id) && x.BillAddressId.Equals(billAddress.Id)).ToList().ForEach(x => x.BillAddress = billAddress);
                }

                if (objectType == Enums.ObjectType.Full)
                {
                    var purchaseOrderIds = objs.Select(x => x.Id);

                    var allDetails = await _purchaseOrderDetailRepository.GetAllWithIdsAsync(purchaseOrderIds);

                    foreach (var purchaseOrder in objs)
                        purchaseOrder.Details = allDetails.Where(x => x.Id.Equals(purchaseOrder.Id)).ToList();
                }
            }

            return objs;
        }
    }
}
