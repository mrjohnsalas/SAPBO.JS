using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class SaleOrderAuthorizationMapper : ISapB1AutoMapper<SaleOrderAuthorization>
    {
        public SaleOrderAuthorization Mapper(IRecordset rs)
        {
            return new SaleOrderAuthorization
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                SaleOrderId = int.Parse(rs.Fields.Item("U_CL_NROPED").Value.ToString()),

                RequestingUserId = rs.Fields.Item("U_CL_USRSOL").Value.ToString(),
                RequestDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECSOL").Value, Utilities.ClockToValue(rs.Fields.Item("U_CL_HORSOL").Value)).Value,
                RejectReason = rs.Fields.Item("U_CL_MOTREC").Value.ToString(),

                StatusId = Utilities.GetStatusTypeIdFromSaleOrderAuthorizationStatus(rs.Fields.Item("U_CL_STSSOL").Value),

                FirstUserId = rs.Fields.Item("U_CL_USRCRE").Value.ToString(),
                FirstDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECCRE").Value, Utilities.ClockToValue(rs.Fields.Item("U_CL_HORCRE").Value)),
                FirstCheck = rs.Fields.Item("U_CL_CHKCRE").Value.ToString().Equals("Y"),
                FirstUserName = rs.Fields.Item("CRE_NAME").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, SaleOrderAuthorization obj)
        {
            table.Name = obj.Id.ToString();

            table.UserFields.Fields.Item("U_CL_NROPED").Value = obj.SaleOrderId.ToString();

            table.UserFields.Fields.Item("U_CL_USRSOL").Value = obj.RequestingUserId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_FECSOL").Value = obj.RequestDate.ToString(AppFormats.Date);
            table.UserFields.Fields.Item("U_CL_HORSOL").Value = obj.RequestDate.ToString(AppFormats.Time);
            table.UserFields.Fields.Item("U_CL_MOTREC").Value = obj.RejectReason ?? string.Empty;

            table.UserFields.Fields.Item("U_CL_STSSOL").Value = Utilities.GetSaleOrderAuthorizationStatusFromStatusType(obj.StatusType);

            table.UserFields.Fields.Item("U_CL_USRCRE").Value = obj.FirstUserId ?? string.Empty;
            if (obj.FirstDate.HasValue)
            {
                table.UserFields.Fields.Item("U_CL_FECCRE").Value = obj.FirstDate.Value.ToString(AppFormats.Date);
                table.UserFields.Fields.Item("U_CL_HORCRE").Value = obj.FirstDate.Value.ToString(AppFormats.Time);
            }
            else
            {
                table.UserFields.Fields.Item("U_CL_FECCRE").SetNullValue();
                table.UserFields.Fields.Item("U_CL_HORCRE").SetNullValue();
            }
            table.UserFields.Fields.Item("U_CL_CHKCRE").Value = obj.FirstCheck ? "Y" : "N";
          

            return table;
        }
    }
}
