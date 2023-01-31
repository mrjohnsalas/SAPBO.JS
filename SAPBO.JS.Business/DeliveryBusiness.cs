using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;
using System.Net.Mail;

namespace SAPBO.JS.Business
{
    public class DeliveryBusiness : SapB1GenericRepository<Delivery>, IDeliveryBusiness
    {
        private readonly IBusinessPartnerBusiness _businessPartnerRepository;
        private readonly ICurrencyBusiness _currencyRepository;
        private readonly IBusinessPartnerContactBusiness _businessPartnerContactRepository;
        private readonly IBusinessPartnerPaymentBusiness _businessPartnerPaymentRepository;
        private readonly IEmployeeBusiness _employeeRepository;
        private readonly IBusinessPartnerAddressBusiness _businessPartnerAddressRepository;
        private readonly IDeliveryDetailBusiness _deliveryDetailRepository;
        private readonly IEmailBusiness _emailRepository;

        public DeliveryBusiness(SapB1Context context, ISapB1AutoMapper<Delivery> mapper,
            IBusinessPartnerBusiness businessPartnerRepository,
            ICurrencyBusiness currencyRepository,
            IBusinessPartnerContactBusiness businessPartnerContactRepository,
            IBusinessPartnerPaymentBusiness businessPartnerPaymentRepository,
            IEmployeeBusiness employeeRepository,
            IBusinessPartnerAddressBusiness businessPartnerAddressRepository,
            IDeliveryDetailBusiness deliveryDetailRepository,
            IEmailBusiness emailRepository) : base(context, mapper)
        {
            _businessPartnerRepository = businessPartnerRepository;
            _currencyRepository = currencyRepository;
            _businessPartnerContactRepository = businessPartnerContactRepository;
            _businessPartnerPaymentRepository = businessPartnerPaymentRepository;
            _employeeRepository = employeeRepository;
            _businessPartnerAddressRepository = businessPartnerAddressRepository;
            _deliveryDetailRepository = deliveryDetailRepository;
            _emailRepository = emailRepository;
        }

        public async Task<ICollection<Delivery>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_463", new List<dynamic> { year, month }), objectType);
        }

        public async Task<ICollection<Delivery>> GetAllByCarrierIdAsync(string carrierId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_464", new List<dynamic> { carrierId, year, month }), objectType);
        }

        public async Task<ICollection<Delivery>> GetAllByBusinessPartnerIdAsync(string businessPartnerId, int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_465", new List<dynamic> { businessPartnerId, year, month }), objectType);
        }

        public async Task<Delivery> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_466", new List<dynamic> { id }), objectType);
        }

        public async Task<ICollection<Delivery>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_471", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<ICollection<Delivery>> GetAllBySaleOrderIdAndWithIdsAsync(int saleOrderId, IEnumerable<int> lineNums)
        {
            var details = await _deliveryDetailRepository.GetAllBySaleOrderIdAndWithIdsAsync(saleOrderId, lineNums);
            if (details == null || !details.Any())
                return null;
            return await SetHeaderProperties(details);
        }

        public async Task<ICollection<Delivery>> GetAllBySaleOrderIdAndLineNumAsync(int saleOrderId, int lineNum)
        {
            var details = await _deliveryDetailRepository.GetAllBySaleOrderIdAndLineNumAsync(saleOrderId, lineNum);
            if (details == null || !details.Any())
                return null;
            return await SetHeaderProperties(details);
        }

        private EmailAlertTemplateModel0 GetBody(Delivery delivery, Enums.DeliveryStep step)
        {
            var body = new EmailAlertTemplateModel0();
            body.HeadTitle = "ENTREGA";
            body.HeadSecondLine = delivery.ReferenceNumber;
            switch (step)
            {
                case Enums.DeliveryStep.Ready:
                    body.AlertImageLink = AppMessages.Step2_DeliveryReadyImageLink;
                    body.AlertTitle = string.Format(AppMessages.Step2_DeliveryReadyTitle, delivery.ReferenceNumber);
                    body.AlertText = AppMessages.Step2_DeliveryReadyText;
                    break;
                case Enums.DeliveryStep.Dispatched:
                    body.AlertImageLink = AppMessages.Step3_DeliveryDispatchedImageLink;
                    body.AlertTitle = string.Format(AppMessages.Step3_DeliveryDispatchedTitle, delivery.ReferenceNumber);
                    body.AlertText = AppMessages.Step3_DeliveryDispatchedText;
                    break;
                case Enums.DeliveryStep.Delivered:
                    body.AlertImageLink = AppMessages.Step4_DeliveryDeliveredImageLink;
                    body.AlertTitle = string.Format(AppMessages.Step4_DeliveryDeliveredTitle, delivery.ReferenceNumber);
                    body.AlertText = AppMessages.Step4_DeliveryDeliveredText;
                    break;
            }

            body.Heads = new List<EmailAlertTemplateModel0Data>
            {
                new EmailAlertTemplateModel0Data { Name = "ORDEN DE VENTA:", Value = delivery.Details.FirstOrDefault().SaleOrderId.ToString() },
                new EmailAlertTemplateModel0Data { Name = "FECHA:", Value = delivery.DocumentDate.ToString(AppFormats.Date) },
                new EmailAlertTemplateModel0Data { Name = "RUC:", Value = delivery.BusinessPartner.Ruc },
                new EmailAlertTemplateModel0Data { Name = "CLIENTE:", Value = delivery.BusinessPartner.Name },
                new EmailAlertTemplateModel0Data { Name = "CONTACTO:", Value = delivery.Contact == null ? string.Empty : delivery.Contact.FullName },
                new EmailAlertTemplateModel0Data { Name = "VENDEDOR:", Value = delivery.SaleEmployee.FullName },
                new EmailAlertTemplateModel0Data { Name = "COND. PAGO:", Value = delivery.Payment.Name },
                new EmailAlertTemplateModel0Data { Name = "MONEDA:", Value = delivery.Currency.Name }
            };
            if (delivery.CurrencyId == "USD")
                body.Heads.Add(new EmailAlertTemplateModel0Data { Name = "T. CAMBIO:", Value = $"{AppDefaultValues.CurrencySymbolSol} {delivery.Rate.ToString(AppFormats.StringQuantity)}" });

            body.DetailTitle = "DETALLE DE LA ENTREGA";

            body.Details = new List<EmailAlertTemplateModel0Detail>();
            foreach (var item in delivery.Details)
            {
                body.Details.Add(new EmailAlertTemplateModel0Detail
                {
                    DetailName = item.Product.Name,
                    DetailSecondName = "U.Medida:",
                    DetailSecondValue = item.MedidaId,
                    DetailText = item.ProductDetail,
                    DetailRightName0 = "Cantidad",
                    DetailRightValue0 = item.Quantity.ToString(AppFormats.StringQuantity),
                    DetailRightName1 = "Total",
                    DetailRightValue1 = item.BaseTotal < item.CustomerTotal
                        ? $"{delivery.Currency.Symbol} {item.CustomerTotal.ToString(AppFormats.StringTotal)}"
                        : $"{delivery.Currency.Symbol} {item.BaseTotal.ToString(AppFormats.StringTotal)}"
                });
            }

            body.Footers = new List<EmailAlertTemplateModel0Data>
            {
                new EmailAlertTemplateModel0Data { Name = "TOTAL S. DESC.:", Value = delivery.Descuento < 0
                    ? $"{delivery.Currency.Symbol} {delivery.SubTotal.ToString(AppFormats.StringTotal)}"
                    : $"{delivery.Currency.Symbol} {delivery.TotalSinDescuento.ToString(AppFormats.StringTotal)}" },
                new EmailAlertTemplateModel0Data { Name = "DESCUENTO:", Value = delivery.Descuento < 0
                    ? $"{delivery.Currency.Symbol} {"0.00"}"
                    : $"{delivery.Currency.Symbol} {delivery.Descuento.ToString(AppFormats.StringTotal)}" },
                new EmailAlertTemplateModel0Data { Name = "SUB TOTAL:", Value = $"{delivery.Currency.Symbol} {delivery.SubTotal.ToString(AppFormats.StringTotal)}" },
                new EmailAlertTemplateModel0Data { Name = "IGV:", Value = $"{delivery.Currency.Symbol} {delivery.Impuesto.ToString(AppFormats.StringTotal)}" },
                new EmailAlertTemplateModel0Data { Name = "TOTAL:", Value = $"{delivery.Currency.Symbol} {delivery.Total.ToString(AppFormats.StringTotal)}" }
            };

            body.FooterTitle = "Comentarios:";
            body.FooterText = delivery.Remarks;

            body.FooterBlocks = new List<EmailAlertTemplateModel0Block>();
            body.FooterBlocks.Add(
                new EmailAlertTemplateModel0Block
                {
                    LeftBlock = new EmailAlertTemplateModel0Data
                    {
                        Name = "Dirección de entrega:",
                        Value = delivery.ShipAddress.FullDescription
                    },
                    RightBlock = new EmailAlertTemplateModel0Data
                    {
                        Name = "Transportista:",
                        Value = $"{delivery.Carrier.Name}<br>{delivery.Carrier.Ruc}"
                    }
                });

            var footer2Line = new EmailAlertTemplateModel0Block();
            if (delivery.Agent != null)
            {
                footer2Line.LeftBlock = new EmailAlertTemplateModel0Data
                {
                    Name = "Agencia:",
                    Value = $"{delivery.Agent.Name}<br>{delivery.AgentAddress.FullDescription}"
                };
            }
            if (!string.IsNullOrEmpty(delivery.FirstNameConsignatario))
            {
                footer2Line.RightBlock = new EmailAlertTemplateModel0Data
                {
                    Name = "Consignatario:",
                    Value = $"{delivery.FullNameConsignatario}<br>{delivery.DNIConsignatario}<br>{delivery.MobilePhoneConsignatario}<br>{delivery.EmailConsignatario}"
                };
            }
            if (footer2Line.LeftBlock != null || footer2Line.RightBlock != null)
                body.FooterBlocks.Add(footer2Line);

            return body;
        }

        public async Task DispatchedAsync(int id, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Check
            if (string.IsNullOrEmpty(updatedBy))
                throw new Exception(AppMessages.UserError);

            if (currentObj.StatusType != Enums.StatusType.PorDespachar)
                throw new Exception(AppMessages.StatusError);

            if (currentObj.Carrier == null)
                throw new Exception(AppMessages.CarrierNotFoundFromOperation);

            var systemDate = DateTime.Now;
            await UpdateAllAsync("GP_WEB_APP_467", new List<dynamic> { id, systemDate.ToString(AppFormats.FullDate), updatedBy });

            var email = await _emailRepository.GetByGroupIdAsync("DB001");

            if (!string.IsNullOrEmpty(currentObj.BusinessPartner.Email))
                email.To.Add(new MailAddress(currentObj.BusinessPartner.Email));
            if (currentObj.Contact != null && !string.IsNullOrEmpty(currentObj.Contact.Email))
                email.To.Add(new MailAddress(currentObj.Contact.Email));

            if (!string.IsNullOrEmpty(currentObj.SaleEmployee.Email))
                if (email.To != null && email.To.Any())
                    email.Cc.Add(new MailAddress(currentObj.SaleEmployee.Email));
                else
                    email.To.Add(new MailAddress(currentObj.SaleEmployee.Email));

            if (!string.IsNullOrEmpty(currentObj.Carrier.Email))
                if (email.To != null && email.To.Any())
                    email.Cc.Add(new MailAddress(currentObj.Carrier.Email));
                else
                    email.To.Add(new MailAddress(currentObj.Carrier.Email));

            email.Subject = string.Format(AppMessages.Step3_DeliveryDispatchedSubject, currentObj.ReferenceNumber);
            email.Body = EmailAlertTemplateUtilities.EmailAlertTemplateModel0Builder(GetBody(currentObj, Enums.DeliveryStep.Dispatched));
            _emailRepository.SendEmailAsync(email);
        }

        public async Task DeliveredAsync(int id, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Check
            if (string.IsNullOrEmpty(updatedBy))
                throw new Exception(AppMessages.UserError);

            if (currentObj.StatusType != Enums.StatusType.PorEntregar)
                throw new Exception(AppMessages.StatusError);

            if (currentObj.Carrier == null)
                throw new Exception(AppMessages.CarrierNotFoundFromOperation);

            var systemDate = DateTime.Now;
            await UpdateAllAsync("GP_WEB_APP_468", new List<dynamic> { id, systemDate.ToString(AppFormats.FullDate), updatedBy });

            var email = await _emailRepository.GetByGroupIdAsync("DB001");

            if (!string.IsNullOrEmpty(currentObj.BusinessPartner.Email))
                email.To.Add(new MailAddress(currentObj.BusinessPartner.Email));
            if (currentObj.Contact != null && !string.IsNullOrEmpty(currentObj.Contact.Email))
                email.To.Add(new MailAddress(currentObj.Contact.Email));

            if (!string.IsNullOrEmpty(currentObj.SaleEmployee.Email))
                if (email.To != null && email.To.Any())
                    email.Cc.Add(new MailAddress(currentObj.SaleEmployee.Email));
                else
                    email.To.Add(new MailAddress(currentObj.SaleEmployee.Email));

            if (!string.IsNullOrEmpty(currentObj.Carrier.Email))
                if (email.To != null && email.To.Any())
                    email.Cc.Add(new MailAddress(currentObj.Carrier.Email));
                else
                    email.To.Add(new MailAddress(currentObj.Carrier.Email));

            email.Subject = string.Format(AppMessages.Step4_DeliveryDeliveredSubject, currentObj.ReferenceNumber);
            email.Body = EmailAlertTemplateUtilities.EmailAlertTemplateModel0Builder(GetBody(currentObj, Enums.DeliveryStep.Delivered));
            _emailRepository.SendEmailAsync(email);
        }

        public async Task<ICollection<ApprovalListResult>> SetCarrierAsync(List<int> ids, string carrierId, string addressId)
        {
            //Get obj
            var deliveries = await GetAllWithIdsAsync(ids);
            if (deliveries == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Get Carrier
            var carrier = await _businessPartnerRepository.GetCarrierAsync(carrierId);
            if (carrier == null)
                throw new Exception(AppMessages.CarrierNotFoundFromOperation);

            //Get Address
            var address = await _businessPartnerAddressRepository.GetAsync(carrierId, addressId);
            if (address == null)
                throw new Exception(AppMessages.AddressNotFoundFromOperation);

            var addressDescription = $"{address.Street} {address.County}-{address.City}";

            var results = new List<ApprovalListResult>();
            foreach (var id in ids)
            {
                var delivery = deliveries.FirstOrDefault(x => x.Id.Equals(id));
                if (delivery == null)
                    results.Add(new ApprovalListResult { Id = id, Result = false, Message = AppMessages.NotFoundFromOperation });
                else
                {
                    if (delivery.StatusType != Enums.StatusType.PorDespachar)
                        results.Add(new ApprovalListResult { Id = id, Result = false, Message = AppMessages.StatusError });
                    else
                        results.Add(new ApprovalListResult { Id = id, Result = true, Message = "OK" });
                }
            }

            await UpdateAllAsync("GP_WEB_APP_470", new List<dynamic> { string.Join(",", ids), carrier.Id, carrier.Ruc, carrier.Name.Length > 50 ? carrier.Name.Substring(0, 50) : carrier.Name, addressDescription.Length > 100 ? addressDescription.Substring(0, 100) : addressDescription });

            return results;
        }

        public async Task<ICollection<Delivery>> SetHeaderProperties(ICollection<DeliveryDetail> objs)
        {
            var deliveryIds = objs.GroupBy(x => x.DeliveryId).Select(g => g.Key);
            var deliveries = await GetAllWithIdsAsync(deliveryIds, Enums.ObjectType.Only);
            foreach (var delivery in deliveries)
                delivery.Details = objs.Where(x => x.DeliveryId.Equals(delivery.Id)).ToList();
            return deliveries;
        }

        public async Task<Delivery> SetFullProperties(Delivery obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            obj.Carrier = await _businessPartnerRepository.GetCarrierAsync(obj.CarrierId);
            obj.BusinessPartner = await _businessPartnerRepository.GetAsync(obj.BusinessPartnerId);
            obj.ShipAddress = await _businessPartnerAddressRepository.GetAsync(obj.BusinessPartnerId, obj.ShipAddressId);

            if (!string.IsNullOrEmpty(obj.AgentId))
                obj.Agent = await _businessPartnerRepository.GetTransportAgencyAsync(obj.AgentId);

            if (!string.IsNullOrEmpty(obj.AgentAddressId))
                obj.AgentAddress = await _businessPartnerAddressRepository.GetAsync(obj.AgentId, obj.AgentAddressId);

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.Currency = await _currencyRepository.GetAsync(obj.CurrencyId);
                obj.Contact = await _businessPartnerContactRepository.GetAsync(obj.ContactId);
                obj.Payment = await _businessPartnerPaymentRepository.GetAsync(obj.PaymentId);
                obj.SaleEmployee = await _employeeRepository.GetBySaleEmployeeIdAsync(obj.SaleEmployeeId, Enums.ObjectType.Only);
                //obj.ShipAddress = await _businessPartnerAddressRepository.GetAsync(obj.BusinessPartnerId, obj.ShipAddressId);
                obj.BillAddress = await _businessPartnerAddressRepository.GetAsync(obj.BusinessPartnerId, obj.BillAddressId);

                if (objectType == Enums.ObjectType.Full)
                    obj.Details = await _deliveryDetailRepository.GetAllAsync(obj.Id);
            }

            return obj;
        }

        public async Task<ICollection<Delivery>> SetFullProperties(ICollection<Delivery> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            //Carrier
            var carrierIds = objs.GroupBy(x => x.CarrierId).Select(g => g.Key);
            var carriers = await _businessPartnerRepository.GetCarrierAllWithIdsAsync(carrierIds);

            foreach (var carrier in carriers)
                objs.Where(x => x.CarrierId.Equals(carrier.Id)).ToList().ForEach(x => x.Carrier = carrier);

            //BusinessPartner
            var businessPartnerIds = objs.GroupBy(x => x.BusinessPartnerId).Select(g => g.Key);
            var businessPartners = await _businessPartnerRepository.GetAllWithIdsAsync(businessPartnerIds);

            foreach (var businessPartner in businessPartners)
            {
                objs.Where(x => x.BusinessPartnerId.Equals(businessPartner.Id)).ToList().ForEach(x => x.BusinessPartner = businessPartner);

                //ShipAddress
                var shipAddressIds = objs.Where(x => x.BusinessPartnerId.Equals(businessPartner.Id)).GroupBy(x => x.ShipAddressId).Select(g => g.Key);
                var shipAddresses = await _businessPartnerAddressRepository.GetAllWithIdsAsync(businessPartner.Id, shipAddressIds);

                foreach (var shipAddress in shipAddresses)
                    objs.Where(x => x.BusinessPartnerId.Equals(businessPartner.Id) && x.ShipAddressId.Equals(shipAddress.Id)).ToList().ForEach(x => x.ShipAddress = shipAddress);
            }

            //Agent
            var agentIds = objs.GroupBy(x => x.AgentId).Select(g => g.Key);
            var agents = await _businessPartnerRepository.GetTransportAgencyAllWithIdsAsync(agentIds);

            foreach (var agent in agents)
            {
                objs.Where(x => x.AgentId.Equals(agent.Id)).ToList().ForEach(x => x.Agent = agent);

                //Agent Address
                var agentAddressIds = objs.Where(x => x.AgentId.Equals(agent.Id)).GroupBy(x => x.AgentAddressId).Select(g => g.Key);
                var agentAddresses = await _businessPartnerAddressRepository.GetAllWithIdsAsync(agent.Id, agentAddressIds);

                foreach (var agentAddress in agentAddresses)
                    objs.Where(x => x.AgentId.Equals(agent.Id) && x.AgentAddressId.Equals(agentAddress.Id)).ToList().ForEach(x => x.AgentAddress = agentAddress);
            }

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
                    objs.Where(x => x.SaleEmployeeId.Equals(employee.Id)).ToList().ForEach(x => x.SaleEmployee = employee);

                ////ShipAddress
                //foreach (var id in businessPartnerIds)
                //{
                //    var shipAddressIds = objs.Where(x => x.BusinessPartnerId.Equals(id)).GroupBy(x => x.ShipAddressId).Select(g => g.Key);
                //    var shipAddresses = await _businessPartnerAddressRepository.GetAllWithIdsAsync(id, shipAddressIds);

                //    foreach (var shipAddress in shipAddresses)
                //        objs.Where(x => x.BusinessPartnerId.Equals(id) && x.ShipAddressId.Equals(shipAddress.Id)).ToList().ForEach(x => x.ShipAddress = shipAddress);
                //}

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
                    var deliveryIds = objs.Select(x => x.Id);

                    var allDetails = await _deliveryDetailRepository.GetAllWithIdsAsync(deliveryIds);

                    foreach (var delivery in objs)
                        delivery.Details = allDetails.Where(x => x.Id.Equals(delivery.Id)).ToList();
                }
            }

            return objs;
        }
    }
}
