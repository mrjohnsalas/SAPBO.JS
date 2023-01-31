using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;
using System.Net.Mail;

namespace SAPBO.JS.Business
{
    public class PurchaseOrderAuthorizationBusiness : SapB1GenericRepository<PurchaseOrderAuthorization>, IPurchaseOrderAuthorizationBusiness
    {
        private readonly IEmailBusiness _emailBusinessRepository;
        private readonly ICurrencyBusiness _currencyRepository;
        private readonly IPurchaseOrderBusiness _purchaseOrderRepository;
        private const string _tableName = TableNames.PurchaseOrderAuthorization;

        public PurchaseOrderAuthorizationBusiness(
            SapB1Context context,
            ISapB1AutoMapper<PurchaseOrderAuthorization> mapper,
            IPurchaseOrderBusiness purchaseOrderRepository,
            IEmailBusiness emailBusinessRepository,
            ICurrencyBusiness currencyRepository) : base(context, mapper)
        {
            _emailBusinessRepository = emailBusinessRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<ICollection<PurchaseOrderAuthorization>> GetAllAsync(int year, int month)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_390", new List<dynamic> { year, month }), Enums.ObjectType.Full);
        }

        public async Task<PurchaseOrderAuthorization> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_391", new List<dynamic> { id }), objectType);
        }

        private dynamic GetInHtml(int id, string message)
        {
            var html = GetValue("GP_WEB_APP_392", "HTML", new List<dynamic> { id, message });
            if (html == null)
                throw new Exception(AppMessages.GenNewId);

            return html;
        }

        public async Task<ICollection<ApprovalListResult>> ApproveListAsync(List<int> ids, string updatedBy)
        {
            var results = new List<ApprovalListResult>();
            foreach (var id in ids)
            {
                try
                {
                    await ApproveAsync(id, updatedBy);
                    results.Add(new ApprovalListResult { Id = id, Result = true, Message = "OK" });
                }
                catch (Exception ex)
                {
                    results.Add(new ApprovalListResult { Id = id, Result = false, Message = ex.Message });
                }
            }

            return results;
        }

        public async Task ApproveAsync(int id, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);
            currentObj.FirstUserId = updatedBy;

            CheckRules(currentObj, Enums.ObjectAction.Approve);

            //Set obj
            currentObj.Status = Enums.PurchaseOrderAuthorizationStatus.Autorizado;
            currentObj.FirstCheck = true;
            currentObj.FirstDate = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());

            var email = await _emailBusinessRepository.GetByGroupIdAsync("POB001");
            email.Subject = string.Format(AppMessages.PurchaseOrderAuthorization_Response_Subject, currentObj.PurchaseOrderId);
            email.Body = GetInHtml(currentObj.PurchaseOrderId, string.Format(AppMessages.PurchaseOrderAuthorization_Approve_Message, currentObj.PurchaseOrderId, currentObj.FirstUserId, currentObj.FirstDate.Value));
            email.To.Add(new MailAddress(currentObj.UserIdSolicitante, currentObj.UserIdSolicitante));
            _emailBusinessRepository.SendEmailAsync(email);
        }

        public async Task OverrideAsync(int id, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(currentObj, Enums.ObjectAction.Override);

            //Set obj
            currentObj.Status = Enums.PurchaseOrderAuthorizationStatus.Pendiente;
            currentObj.FirstUserId = null;
            currentObj.FirstDate = null;
            currentObj.FirstCheck = false;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());

            var email = await _emailBusinessRepository.GetByGroupIdAsync("POB001");
            email.Subject = string.Format(AppMessages.PurchaseOrderAuthorization_Override_Subject, currentObj.PurchaseOrderId);
            email.Body = GetInHtml(currentObj.PurchaseOrderId, string.Format(AppMessages.PurchaseOrderAuthorization_Override_Message, currentObj.PurchaseOrderId, updatedBy, DateTime.Now));
            email.To.Add(new MailAddress(currentObj.UserIdSolicitante, currentObj.UserIdSolicitante));
            _emailBusinessRepository.SendEmailAsync(email);
        }

        public async Task RejectAsync(int id, string reason, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);
            currentObj.FirstUserId = updatedBy;
            currentObj.RejectReason = reason;

            CheckRules(currentObj, Enums.ObjectAction.Reject);

            //Set obj
            currentObj.Status = Enums.PurchaseOrderAuthorizationStatus.Rechazado;
            currentObj.FirstCheck = true;
            currentObj.FirstDate = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());

            var email = await _emailBusinessRepository.GetByGroupIdAsync("POB001");
            email.Subject = string.Format(AppMessages.PurchaseOrderAuthorization_Response_Subject, currentObj.PurchaseOrderId);
            email.Body = GetInHtml(currentObj.PurchaseOrderId, string.Format(AppMessages.PurchaseOrderAuthorization_Reject_Message, currentObj.PurchaseOrderId, currentObj.FirstUserId, currentObj.FirstDate.Value, currentObj.RejectReason));
            email.To.Add(new MailAddress(currentObj.UserIdSolicitante, currentObj.UserIdSolicitante));
            _emailBusinessRepository.SendEmailAsync(email);
        }

        private void CheckRules(PurchaseOrderAuthorization obj, Enums.ObjectAction objectAction)
        {
            //Check Status
            if (objectAction == Enums.ObjectAction.Approve && obj.Status != Enums.PurchaseOrderAuthorizationStatus.Pendiente)
                throw new Exception(AppMessages.StatusError);

            //Check MotivoRechazo
            if (objectAction == Enums.ObjectAction.Reject && string.IsNullOrEmpty(obj.RejectReason))
                throw new Exception(AppMessages.PurchaseOrderAuthorizationRejectReason);

            //Check Override Status
            if (objectAction == Enums.ObjectAction.Override && obj.Status != Enums.PurchaseOrderAuthorizationStatus.Autorizado)
                throw new Exception(AppMessages.StatusError);
        }

        public async Task<PurchaseOrderAuthorization> SetFullProperties(PurchaseOrderAuthorization obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.PurchaseOrderCurrency = await _currencyRepository.GetAsync(obj.PurchaseOrderCurrencyId);

                if (objectType == Enums.ObjectType.Full)
                {
                    obj.PurchaseOrder = await _purchaseOrderRepository.GetAsync(obj.PurchaseOrderId);
                }
            }

            return obj;
        }

        public async Task<ICollection<PurchaseOrderAuthorization>> SetFullProperties(ICollection<PurchaseOrderAuthorization> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                //Currency
                var currencyIds = objs.GroupBy(x => x.PurchaseOrderCurrencyId).Select(g => g.Key);
                var currencies = await _currencyRepository.GetAllAsync();

                foreach (var currency in currencies)
                    objs.Where(x => x.PurchaseOrderCurrencyId.Equals(currency.Id)).ToList().ForEach(x => x.PurchaseOrderCurrency = currency);

                if (objectType == Enums.ObjectType.Full)
                {
                    var purchaseOrderIds = objs.GroupBy(x => x.PurchaseOrderId).Select(g => g.Key);
                    var purchaseOrders = await _purchaseOrderRepository.GetAllWithIdsAsync(purchaseOrderIds);

                    foreach (var purchaseOrder in purchaseOrders)
                        objs.Where(x => x.PurchaseOrderId.Equals(purchaseOrder.Id)).ToList().ForEach(x => x.PurchaseOrder = purchaseOrder);
                }
            }

            return objs;
        }
    }
}
