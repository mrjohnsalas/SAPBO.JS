using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;
using System.Net.Mail;

namespace SAPBO.JS.Business
{
    public class SaleOrderAuthorizationBusiness : SapB1GenericRepository<SaleOrderAuthorization>, ISaleOrderAuthorizationBusiness
    {
        private readonly IEmailBusiness _emailBusinessRepository;
        private readonly ISaleOrderBusiness _saleOrderRepository;
        private const string _tableName = TableNames.SaleOrderAuthorization;

        public SaleOrderAuthorizationBusiness(
            SapB1Context context,
            ISapB1AutoMapper<SaleOrderAuthorization> mapper,
            ISaleOrderBusiness saleOrderRepository,
            IEmailBusiness emailBusinessRepository) : base(context, mapper)
        {
            _emailBusinessRepository = emailBusinessRepository;
            _saleOrderRepository = saleOrderRepository;
        }

        public async Task<ICollection<SaleOrderAuthorization>> GetAllAsync(int year, int month)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_049", new List<dynamic> { year, month }), Enums.ObjectType.Full);
        }

        public async Task<ICollection<SaleOrderAuthorization>> GetAllBySaleEmployeeIdAsync(int year, int month, int saleEmployeeId)
        {
            var systemDate = DateTime.Now;
            year = year <= 0 ? systemDate.Year : year;
            month = month <= 0 ? systemDate.Month : month;
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_482", new List<dynamic> { year, month, saleEmployeeId }), Enums.ObjectType.Full);
        }

        public async Task<SaleOrderAuthorization> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_047", new List<dynamic> { id }), objectType);
        }

        private dynamic GetInHtml(int id, string message)
        {
            var html = GetValue("GP_WEB_APP_483", "HTML", new List<dynamic> { id, message });
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
            currentObj.StatusId = (int)Enums.StatusType.Autorizado;
            currentObj.FirstCheck = true;
            currentObj.FirstDate = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());

            var email = await _emailBusinessRepository.GetByGroupIdAsync("SOA001");
            email.Subject = string.Format(AppMessages.SaleOrderAuthorization_Response_Subject, currentObj.SaleOrderId);
            email.Body = GetInHtml(currentObj.SaleOrderId, string.Format(AppMessages.SaleOrderAuthorization_Approve_Message, currentObj.SaleOrderId, currentObj.FirstUserId, currentObj.FirstDate.Value));
            email.To.Add(new MailAddress(currentObj.RequestingUserId, currentObj.RequestingUserId));
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
            currentObj.StatusId = (int)Enums.StatusType.Rechazado;
            currentObj.FirstCheck = true;
            currentObj.FirstDate = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());

            var email = await _emailBusinessRepository.GetByGroupIdAsync("SOA001");
            email.Subject = string.Format(AppMessages.SaleOrderAuthorization_Response_Subject, currentObj.SaleOrderId);
            email.Body = GetInHtml(currentObj.SaleOrderId, string.Format(AppMessages.SaleOrderAuthorization_Reject_Message, currentObj.SaleOrderId, currentObj.FirstUserId, currentObj.FirstDate.Value, currentObj.RejectReason));
            email.To.Add(new MailAddress(currentObj.RequestingUserId, currentObj.RequestingUserId));
            _emailBusinessRepository.SendEmailAsync(email);
        }

        private void CheckRules(SaleOrderAuthorization obj, Enums.ObjectAction objectAction)
        {
            //Check Status - Approve
            if (objectAction == Enums.ObjectAction.Approve && obj.StatusType != Enums.StatusType.Pendiente)
                throw new Exception(AppMessages.StatusError);

            //Check - Reject
            if (objectAction == Enums.ObjectAction.Reject)
            {
                //Check Status - Reject
                if (obj.StatusType != Enums.StatusType.Pendiente)
                    throw new Exception(AppMessages.StatusError);

                //Check MotivoRechazo
                if (string.IsNullOrEmpty(obj.RejectReason))
                    throw new Exception(AppMessages.SaleOrderAuthorizationRejectReason);
            }
        }

        public async Task<SaleOrderAuthorization> SetFullProperties(SaleOrderAuthorization obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                if (objectType == Enums.ObjectType.Full)
                {
                    obj.SaleOrder = await _saleOrderRepository.GetAsync(obj.SaleOrderId);
                }
            }

            return obj;
        }

        public async Task<ICollection<SaleOrderAuthorization>> SetFullProperties(ICollection<SaleOrderAuthorization> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {

                if (objectType == Enums.ObjectType.Full)
                {
                    var saleOrderIds = objs.GroupBy(x => x.SaleOrderId).Select(g => g.Key);
                    var saleOrders = await _saleOrderRepository.GetAllWithIdsAsync(saleOrderIds, Enums.ObjectType.FullHeader);

                    foreach (var saleOrder in saleOrders)
                        objs.Where(x => x.SaleOrderId.Equals(saleOrder.Id)).ToList().ForEach(x => x.SaleOrder = saleOrder);
                }
            }

            return objs;
        }
    }
}
