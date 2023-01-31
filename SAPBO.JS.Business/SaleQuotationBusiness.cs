using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;
using System.Net.Mail;

namespace SAPBO.JS.Business
{
    public class SaleQuotationBusiness : SapB1GenericRepository<SaleQuotation>, ISaleQuotationBusiness
    {
        private const string _tableName = TableNames.SaleQuotation;
        private readonly IBusinessPartnerBusiness _businessPartnerRepository;
        private readonly ICurrencyBusiness _currencyRepository;
        private readonly IBusinessPartnerContactBusiness _businessPartnerContactRepository;
        private readonly IBusinessPartnerPaymentBusiness _businessPartnerPaymentRepository;
        private readonly IRateBusiness _rateRepository;
        private readonly IEmployeeBusiness _employeeRepository;
        private readonly ISaleQuotationDetailBusiness _saleQuotationDetailRepository;
        private readonly IEmailBusiness _emailBusinessRepository;

        public SaleQuotationBusiness(SapB1Context context, ISapB1AutoMapper<SaleQuotation> mapper,
            IBusinessPartnerBusiness businessPartnerRepository,
            ICurrencyBusiness currencyRepository,
            IBusinessPartnerContactBusiness businessPartnerContactRepository,
            IBusinessPartnerPaymentBusiness businessPartnerPaymentRepository,
            IRateBusiness rateRepository,
            IEmployeeBusiness employeeRepository,
            ISaleQuotationDetailBusiness saleQuotationDetailRepository,
            IEmailBusiness emailBusinessRepository) : base(context, mapper, true)
        {
            _businessPartnerRepository = businessPartnerRepository;
            _currencyRepository = currencyRepository;
            _businessPartnerContactRepository = businessPartnerContactRepository;
            _businessPartnerPaymentRepository = businessPartnerPaymentRepository;
            _rateRepository = rateRepository;
            _employeeRepository = employeeRepository;
            _saleQuotationDetailRepository = saleQuotationDetailRepository;
            _emailBusinessRepository = emailBusinessRepository;
        }

        public async Task<ICollection<SaleQuotation>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_221", new List<dynamic> { year, month }), objectType);
        }

        public async Task<ICollection<SaleQuotation>> GetAllBySaleEmployeeIdAsync(int saleEmployeeId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_216", new List<dynamic> { saleEmployeeId, year, month }), objectType);
        }

        public async Task<ICollection<SaleQuotation>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_442", new List<dynamic> { businessPartnerId, year, month }), objectType);
        }

        public async Task<SaleQuotation> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_215", new List<dynamic> { id }), objectType);
        }

        private dynamic GetInHtml(int id, string message)
        {
            var html = GetValue("GP_WEB_APP_199", "CT", new List<dynamic> { id, message });
            if (html == null)
                throw new Exception(AppMessages.GenNewId);

            return html;
        }

        public async Task CreateAsync(SaleQuotation obj)
        {
            var systemDate = DateTime.Now;

            //Check User - Create
            if (string.IsNullOrEmpty(obj.CreatedBy))
                throw new Exception(AppMessages.UserError);

            ////Check DeliveryDate
            //if (obj.DeliveryDate < systemDate.Date)
            //    throw new Exception(AppMessages.SaleQuotation_DeliveryDate);

            //Check Details
            if (!obj.Products.Any())
                throw new Exception(AppMessages.SaleQuotation_Products);

            //Check CurrencyId
            if (string.IsNullOrEmpty(obj.CurrencyId))
                throw new Exception(AppMessages.SaleQuotation_CurrencyId);

            if (!obj.CurrencyId.Equals(AppDefaultValues.CurrencyIdDolar) && !obj.CurrencyId.Equals(AppDefaultValues.CurrencyIdSol))
                throw new Exception(AppMessages.CurrencyIdNotValid);

            //Check Rate
            var rate = await _rateRepository.GetByDateAndCurrencyIdAsync(systemDate, AppDefaultValues.CurrencyIdDolar);
            if (rate == null)
                throw new Exception(AppMessages.RateNotFound);

            obj.Rate = obj.CurrencyId.Equals(AppDefaultValues.CurrencyIdDolar) ? rate.Value : AppDefaultValues.RateForSolCurrency;

            //Check BusinessPartner
            var bp = await _businessPartnerRepository.GetWithTempAsync(obj.BusinessPartnerId, Enums.ObjectType.Custom);
            if (bp == null)
                throw new Exception(AppMessages.BusinessPartnerNotFound);

            //Check ContactId
            if (!bp.IsTemp && obj.ContactId.Equals(0))
                throw new Exception(AppMessages.SaleQuotation_ContactId);

            if (!bp.IsTemp && !obj.ContactId.Equals(0) && !bp.Contacts.Any(x => x.Id.Equals(obj.ContactId)))
                throw new Exception(AppMessages.BusinessPartnerContactNotFound);

            //Check PaymentId
            if (obj.PaymentId.Equals(0))
                throw new Exception(AppMessages.SaleQuotation_PaymentId);

            if (!bp.Payments.Any(x => x.Id.Equals(obj.PaymentId)))
                throw new Exception(AppMessages.BusinessPartnerPaymentNotFound);

            //obj.BusinessPartner = bp;
            obj.SaleEmployeeId = bp.SaleEmployeeId;
            obj.DaysValidValue = AppDefaultValues.DefaultDaysForSaleQuotation;
            obj.StatusId = (int)Enums.StatusType.Solicitado;
            obj.CreatedAt = systemDate;
            obj.DeliveryDate = obj.CreatedAt.Value;

            obj.Id = GetNewId();
            await CreateAsync(_tableName, obj, obj.Id.ToString());

            await _saleQuotationDetailRepository.CreateAsync(obj.Products, obj.Id);

            //Send email alert
            var fullId = $"CT-{obj.CreatedAt.Value.Year:0000}-{obj.CreatedAt.Value.Month:00}-{obj.Id:0000}";
            var email = await _emailBusinessRepository.GetByGroupIdAsync("SQB001");
            email.Subject = string.Format(AppMessages.Cotizacion_Create_Subject, fullId);
            email.Body = GetInHtml(obj.Id, string.Format(AppMessages.Cotizacion_Create_Message, obj.CreatedBy, fullId, obj.CreatedAt.Value.ToString(AppFormats.Date)));
            email.Cc.Add(new MailAddress(obj.CreatedBy, obj.CreatedBy));
            _emailBusinessRepository.SendEmailAsync(email);
        }

        public async Task UpdateAsync(SaleQuotation obj)
        {
            var systemDate = DateTime.Now;

            //Get doc
            var currentObj = await GetAsync(obj.Id, Enums.ObjectType.Only);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Check User - Update
            if (string.IsNullOrEmpty(obj.UpdatedBy))
                throw new Exception(AppMessages.UserError);

            if (!currentObj.CreatedBy.Equals(obj.UpdatedBy))
                throw new Exception(AppMessages.UserError);

            //Check Status
            if (currentObj.StatusType != Enums.StatusType.Solicitado)
                throw new Exception(AppMessages.StatusError);

            //Check Details
            if (!obj.Products.Any())
                throw new Exception(AppMessages.SaleQuotation_Products);

            //Check CurrencyId
            if (string.IsNullOrEmpty(obj.CurrencyId))
                throw new Exception(AppMessages.SaleQuotation_CurrencyId);

            if (!obj.CurrencyId.Equals(AppDefaultValues.CurrencyIdDolar) && !obj.CurrencyId.Equals(AppDefaultValues.CurrencyIdSol))
                throw new Exception(AppMessages.CurrencyIdNotValid);

            //Check Rate
            var rate = await _rateRepository.GetByDateAndCurrencyIdAsync(systemDate, AppDefaultValues.CurrencyIdDolar);
            if (rate == null)
                throw new Exception(AppMessages.RateNotFound);

            currentObj.Rate = obj.CurrencyId.Equals(AppDefaultValues.CurrencyIdDolar) ? rate.Value : AppDefaultValues.RateForSolCurrency;

            //Check BusinessPartner
            var bp = await _businessPartnerRepository.GetWithTempAsync(obj.BusinessPartnerId, Enums.ObjectType.Custom);
            if (bp == null)
                throw new Exception(AppMessages.BusinessPartnerNotFound);

            //Check ContactId
            if (!bp.IsTemp && obj.ContactId.Equals(0))
                throw new Exception(AppMessages.SaleQuotation_ContactId);

            if (!bp.IsTemp && !obj.ContactId.Equals(0) && !bp.Contacts.Any(x => x.Id.Equals(obj.ContactId)))
                throw new Exception(AppMessages.BusinessPartnerContactNotFound);

            //Check PaymentId
            if (obj.PaymentId.Equals(0))
                throw new Exception(AppMessages.SaleQuotation_PaymentId);

            if (!bp.Payments.Any(x => x.Id.Equals(obj.PaymentId)))
                throw new Exception(AppMessages.BusinessPartnerPaymentNotFound);

            //Set obj
            currentObj.CurrencyId = obj.CurrencyId;
            currentObj.ContactId = obj.ContactId;
            currentObj.PaymentId = obj.PaymentId;
            currentObj.Remarks = obj.Remarks;
            currentObj.Total = obj.Total;
            currentObj.UpdatedBy = obj.UpdatedBy;
            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());

            await _saleQuotationDetailRepository.UpdateAsync(obj.Products, obj.Id);
        }

        public async Task DeleteAsync(int id, string deleteBy)
        {
            var currentObj = await GetAsync(id, Enums.ObjectType.Only);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Check User - Delete
            if (string.IsNullOrEmpty(deleteBy))
                throw new Exception(AppMessages.UserError);

            if (!currentObj.CreatedBy.Equals(deleteBy))
                throw new Exception(AppMessages.UserError);

            //Check Status
            if (currentObj.StatusType != Enums.StatusType.Solicitado)
                throw new Exception(AppMessages.StatusError);

            currentObj.DeletedBy = deleteBy;
            currentObj.StatusId = (int)Enums.StatusType.Anulado;
            currentObj.DeletedAt = DateTime.Now;

            await SoftDeleteByIdAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        public async Task EndAsync(int id, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id, Enums.ObjectType.OnlyFull);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Check validations
            //Check User - Delete
            if (string.IsNullOrEmpty(updatedBy))
                throw new Exception(AppMessages.UserError);

            //Check Status
            if (currentObj.StatusType != Enums.StatusType.Pendiente)
                throw new Exception(AppMessages.StatusError);

            //Check Details
            currentObj.Total = 0;
            foreach (var product in currentObj.Products)
            {
                currentObj.Total += decimal.Round(product.TotalPrice, 2);
                if (product.UnitPrice.Equals(0))
                    throw new Exception(AppMessages.SaleQuotation_End_Products);
            }

            //Set obj
            currentObj.StatusId = (int)Enums.StatusType.Terminado;
            currentObj.UpdatedAt = DateTime.Now;
            currentObj.UpdatedBy = updatedBy;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        public async Task ApproveAsync(int id, List<int> acceptedProducts, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Check AcceptedProducts
            if (acceptedProducts == null || acceptedProducts.Count.Equals(0))
                throw new Exception(AppMessages.SaleQuotation_Accept_Products);

            //Check Details
            currentObj.Total = 0;
            foreach (var product in currentObj.Products)
            {
                currentObj.Total += product.TotalPrice;

                if (acceptedProducts.Contains(product.Id))
                {
                    product.StatusId = (int)Enums.StatusType.Aceptado;
                    currentObj.AceptedTotal += product.TotalPrice;
                }
                else
                {
                    product.StatusId = (int)Enums.StatusType.Rechazado;
                    currentObj.RejectedTotal += decimal.Round(product.TotalPrice, 2);
                }
            }

            //Previous Set obj
            currentObj.UpdatedBy = updatedBy;

            //Check validations
            CheckRules(currentObj, Enums.ObjectAction.Approve);

            //Set obj
            currentObj.StatusId = (int)Enums.StatusType.Aceptado;
            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());

            await _saleQuotationDetailRepository.UpdateAsync(currentObj.Products);

            //Send email alert
            var email = await _emailBusinessRepository.GetByGroupIdAsync("SQB001");
            email.Subject = string.Format(AppMessages.Cotizacion_Accept_Subject, currentObj.IdFull);
            email.Body = GetInHtml(currentObj.Id, string.Format(AppMessages.Cotizacion_Accept_Message, currentObj.BusinessPartner.Name, currentObj.IdFull, currentObj.UpdatedAt.Value.ToString(AppFormats.Date)));
            email.Cc.Add(new MailAddress(currentObj.UpdatedBy, currentObj.UpdatedBy));
            if (currentObj.UpdatedBy != currentObj.CreatedBy)
                email.Cc.Add(new MailAddress(currentObj.CreatedBy, currentObj.CreatedBy));
            _emailBusinessRepository.SendEmailAsync(email);
        }

        public async Task RejectAsync(int id, string rejectReason, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Previous Set obj
            currentObj.UpdatedBy = updatedBy;
            currentObj.RejectReason = rejectReason;

            //Check validations
            CheckRules(currentObj, Enums.ObjectAction.Reject);

            //Set obj
            currentObj.UpdatedAt = DateTime.Now;
            currentObj.StatusId = (int)Enums.StatusType.Rechazado;
            currentObj.AceptedTotal = 0;
            currentObj.RejectedTotal = currentObj.Products.Sum(x => x.TotalPrice);

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
            await _saleQuotationDetailRepository.UpdateStatusBySaleQuotationIdAsync(currentObj.Id, Enums.StatusType.Rechazado);

            //Send email alert
            var email = await _emailBusinessRepository.GetByGroupIdAsync("SQB001");
            email.Subject = string.Format(AppMessages.Cotizacion_Deny_Subject, currentObj.IdFull);
            email.Body = GetInHtml(currentObj.Id, string.Format(AppMessages.Cotizacion_Deny_Message, currentObj.BusinessPartner.Name, currentObj.IdFull, currentObj.UpdatedAt.Value.ToString(AppFormats.Date), currentObj.RejectReason));
            email.Cc.Add(new MailAddress(currentObj.UpdatedBy, currentObj.UpdatedBy));
            if (currentObj.UpdatedBy != currentObj.CreatedBy)
                email.Cc.Add(new MailAddress(currentObj.CreatedBy, currentObj.CreatedBy));
            _emailBusinessRepository.SendEmailAsync(email);
        }

        public async Task PendingAsync(int id, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Check validations
            //...


            //Set obj
            currentObj.StatusId = (int)Enums.StatusType.Terminado;
            currentObj.UpdatedAt = DateTime.Now;
            currentObj.UpdatedBy = updatedBy;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        private void CheckRules(SaleQuotation obj, Enums.ObjectAction objectAction)
        {
            //Check - Approve
            if (objectAction == Enums.ObjectAction.Approve)
            {
                //Check Status
                if (obj.StatusType != Enums.StatusType.Terminado)
                    throw new Exception(AppMessages.StatusError);

                //Check User - Update
                if (string.IsNullOrEmpty(obj.UpdatedBy))
                    throw new Exception(AppMessages.UserError);
            }

            //Check - Reject
            if (objectAction == Enums.ObjectAction.Reject)
            {
                //Check Status
                if (obj.StatusType != Enums.StatusType.Terminado)
                    throw new Exception(AppMessages.StatusError);

                //Check User - Update
                if (string.IsNullOrEmpty(obj.UpdatedBy))
                    throw new Exception(AppMessages.UserError);

                //Check RejectReason
                if (string.IsNullOrEmpty(obj.RejectReason))
                    throw new Exception(AppMessages.SaleOrderAuthorizationRejectReason);
            }
        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_214", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<SaleQuotation> SetFullProperties(SaleQuotation obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            //obj.BusinessPartner = await _businessPartnerRepository.GetAsync(obj.BusinessPartnerId);
            obj.BusinessPartner = await _businessPartnerRepository.GetWithTempAsync(obj.BusinessPartnerId);

            if (objectType == Enums.ObjectType.OnlyFull)
                obj.Products = await _saleQuotationDetailRepository.GetAllAsync(obj.Id, Enums.ObjectType.Only);

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.Currency = await _currencyRepository.GetAsync(obj.CurrencyId);

                if (!obj.ContactId.Equals(0))
                    obj.Contact = await _businessPartnerContactRepository.GetAsync(obj.ContactId);

                obj.Payment = await _businessPartnerPaymentRepository.GetAsync(obj.PaymentId);
                obj.SaleEmployee = await _employeeRepository.GetBySaleEmployeeIdAsync(obj.SaleEmployeeId, Enums.ObjectType.Only);

                if (objectType == Enums.ObjectType.Full)
                    obj.Products = await _saleQuotationDetailRepository.GetAllAsync(obj.Id);
            }

            return obj;
        }

        public async Task<ICollection<SaleQuotation>> SetFullProperties(ICollection<SaleQuotation> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            //BusinessPartner
            var businessPartnerIds = objs.GroupBy(x => x.BusinessPartnerId).Select(g => g.Key);
            //var businessPartners = await _businessPartnerRepository.GetAllWithIdsAsync(businessPartnerIds);
            var businessPartners = await _businessPartnerRepository.GetAllWithTempWithIdsAsync(businessPartnerIds);

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
                var contactIds = objs.Where(y => !y.ContactId.Equals(0)).GroupBy(x => x.ContactId).Select(g => g.Key);
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
                    objs.Where(x => x.SaleEmployeeId.Equals(employee.Id)).ToList().ForEach(x => x.SaleEmployee = employee);

                if (objectType == Enums.ObjectType.Full)
                {
                    var ids = objs.Select(x => x.Id);

                    var products = await _saleQuotationDetailRepository.GetAllWithIdsAsync(ids);

                    foreach (var order in objs)
                        order.Products = products.Where(x => x.Id.Equals(order.Id)).ToList();
                }
            }

            return objs;
        }
    }
}
