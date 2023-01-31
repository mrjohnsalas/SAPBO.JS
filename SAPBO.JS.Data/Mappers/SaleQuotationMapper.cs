using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class SaleQuotationMapper : ISapB1AutoMapper<SaleQuotation>
    {
        public SaleQuotation Mapper(IRecordset rs)
        {
            return new SaleQuotation
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                BusinessPartnerId = rs.Fields.Item("U_CL_CODCLI").Value.ToString(),
                DeliveryDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECENT").Value).Value,
                CurrencyId = rs.Fields.Item("U_CL_CODMDA").Value.ToString(),
                ContactId = int.Parse(rs.Fields.Item("U_CL_CODCTO").Value.ToString()),
                PaymentId = int.Parse(rs.Fields.Item("U_CL_CODCPA").Value.ToString()),
                Rate = decimal.Parse(rs.Fields.Item("U_CL_DOCTPC").Value.ToString()),
                SaleEmployeeId = int.Parse(rs.Fields.Item("U_CL_CODVEN").Value.ToString()),
                DaysValidValue = int.Parse(rs.Fields.Item("U_CL_DIAVAL").Value.ToString()),
                Remarks = rs.Fields.Item("U_CL_REMARK").Value.ToString(),
                RejectReason = rs.Fields.Item("U_CL_MOTREC").Value.ToString(),
                Total = decimal.Parse(rs.Fields.Item("U_CL_TOTCOT").Value.ToString()),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STATUS").Value.ToString()),
                IsCustomer = rs.Fields.Item("U_CL_ISCUST").Value.Equals(1),
                AceptedTotal = decimal.Parse(rs.Fields.Item("U_CL_TOTACE").Value.ToString()),
                RejectedTotal = decimal.Parse(rs.Fields.Item("U_CL_TOTREC").Value.ToString()),
                EndDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECTER").Value)
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, SaleQuotation obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_CODCLI").Value = obj.BusinessPartnerId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_FECENT").Value = obj.DeliveryDate.ToString(AppFormats.Date);
            table.UserFields.Fields.Item("U_CL_CODMDA").Value = obj.CurrencyId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CODCTO").Value = obj.ContactId;
            table.UserFields.Fields.Item("U_CL_CODCPA").Value = obj.PaymentId;
            table.UserFields.Fields.Item("U_CL_DOCTPC").Value = (double)obj.Rate;
            table.UserFields.Fields.Item("U_CL_CODVEN").Value = obj.SaleEmployeeId;
            table.UserFields.Fields.Item("U_CL_DIAVAL").Value = obj.DaysValidValue;
            table.UserFields.Fields.Item("U_CL_REMARK").Value = obj.Remarks ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_MOTREC").Value = obj.RejectReason ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_TOTCOT").Value = (double)obj.Total;
            table.UserFields.Fields.Item("U_CL_STATUS").Value = obj.StatusId;
            table.UserFields.Fields.Item("U_CL_ISCUST").Value = obj.IsCustomer ? 1 : 0;
            table.UserFields.Fields.Item("U_CL_TOTACE").Value = (double)obj.AceptedTotal;
            table.UserFields.Fields.Item("U_CL_TOTREC").Value = (double)obj.RejectedTotal;

            if (obj.EndDate.HasValue)
                table.UserFields.Fields.Item("U_CL_FECTER").Value = obj.EndDate.Value.ToString(AppFormats.Date);

            return table;
        }
    }
}
