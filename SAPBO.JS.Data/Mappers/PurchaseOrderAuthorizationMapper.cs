using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class PurchaseOrderAuthorizationMapper : ISapB1AutoMapper<PurchaseOrderAuthorization>
    {
        public PurchaseOrderAuthorization Mapper(IRecordset rs)
        {
            return new PurchaseOrderAuthorization
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                PurchaseOrderId = int.Parse(rs.Fields.Item("U_CL_NROPED").Value.ToString()),
                PurchaseOrderCurrencyId = rs.Fields.Item("MONEDA_ID_OC").Value.ToString(),
                PurchaseOrderTotal = decimal.Parse(rs.Fields.Item("TOTAL_OC").Value.ToString()),
                UserIdSolicitante = rs.Fields.Item("U_CL_USRSOL").Value.ToString(),
                RequestDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECSOL").Value, rs.Fields.Item("U_CL_HORSOL").Value).Value,
                Status = Utilities.StringToPurchaseOrderAuthorizationStatus(rs.Fields.Item("U_CL_STSSOL").Value),

                FirstUserId = rs.Fields.Item("U_CL_1USEMA").Value.ToString(),
                FirstDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_1USFEC").Value, rs.Fields.Item("U_CL_1USHOR").Value),
                FirstCheck = int.Parse(rs.Fields.Item("U_CL_1USCHK").Value.ToString()).Equals(1),
                RejectReason = rs.Fields.Item("U_CL_1USMOT").Value.ToString(),

                PurchaseOrderUserName = rs.Fields.Item("COM_NAME").Value.ToString(),
                PurchaseOrderEmail = rs.Fields.Item("COM_EMAIL").Value.ToString(),

                PurchaseRequestId = Utilities.IntValueToIntOrNull(rs.Fields.Item("SOL_ID").Value),
                PurchaseRequestUserId = rs.Fields.Item("SOL_USER_ID").Value.ToString(),
                PurchaseRequestUserName = rs.Fields.Item("SOL_NAME").Value.ToString(),
                PurchaseRequestEmail = rs.Fields.Item("SOL_EMAIL").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, PurchaseOrderAuthorization obj)
        {
            table.Name = obj.Id.ToString();

            table.UserFields.Fields.Item("U_CL_NROPED").Value = obj.PurchaseOrderId;
            table.UserFields.Fields.Item("U_CL_USRSOL").Value = obj.UserIdSolicitante ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_FECSOL").Value = obj.RequestDate.ToString(AppFormats.Date);
            table.UserFields.Fields.Item("U_CL_HORSOL").Value = obj.RequestDate.ToString(AppFormats.Time);
            table.UserFields.Fields.Item("U_CL_STSSOL").Value = Utilities.PurchaseOrderAuthorizationStatusToString(obj.Status);

            table.UserFields.Fields.Item("U_CL_1USEMA").Value = obj.FirstUserId ?? string.Empty;
            if (obj.FirstDate.HasValue)
            {
                table.UserFields.Fields.Item("U_CL_1USFEC").Value = obj.FirstDate.Value.ToString(AppFormats.Date);
                table.UserFields.Fields.Item("U_CL_1USHOR").Value = obj.FirstDate.Value.ToString(AppFormats.Time);
            }
            else 
            {
                table.UserFields.Fields.Item("U_CL_1USFEC").SetNullValue();
                table.UserFields.Fields.Item("U_CL_1USHOR").SetNullValue();
            }
            table.UserFields.Fields.Item("U_CL_1USCHK").Value = obj.FirstCheck ? 1 : 0;
            table.UserFields.Fields.Item("U_CL_1USMOT").Value = obj.RejectReason ?? string.Empty;

            return table;
        }
    }
}
