using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class SaleOrderMapper : ISapB1AutoMapper<SaleOrder>
    {
        public SaleOrder Mapper(IRecordset rs)
        {
            return new SaleOrder
            {
                Id = int.Parse(rs.Fields.Item("DocEntry").Value.ToString()),
                SaleOrderTypeId = (int)Utilities.StringToSaleOrderType(rs.Fields.Item("TYPE").Value.ToString()),

                BusinessPartnerId = rs.Fields.Item("CardCode").Value.ToString(),

                PostingDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("DocDate").Value, rs.Fields.Item("DocTime").Value).Value,
                DeliveryDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("DocDueDate").Value).Value,
                DocumentDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("TaxDate").Value).Value,

                CurrencyId = rs.Fields.Item("DocCur").Value.ToString(),

                ContactId = int.Parse(rs.Fields.Item("CntctCode").Value.ToString()),

                PaymentId = int.Parse(rs.Fields.Item("GroupNum").Value.ToString()),

                Rate = decimal.Parse(rs.Fields.Item("DocRate").Value.ToString()),

                SaleEmployeeId = int.Parse(rs.Fields.Item("SlpCode").Value.ToString()),

                ShipAddressId = rs.Fields.Item("ShipToCode").Value.ToString(),

                BillAddressId = rs.Fields.Item("PayToCode").Value.ToString(),

                Remarks = rs.Fields.Item("Comments").Value.ToString(),
                ReferenceNumber = rs.Fields.Item("NumAtCard").Value.ToString(),
                BpReferenceNumber = rs.Fields.Item("U_VS_OCCLIENTE").Value.ToString(),

                AfectoDetraccion = int.Parse(rs.Fields.Item("CHKDET").Value.ToString()) == 1,
                IncluyePercepcion = int.Parse(rs.Fields.Item("CHKPER").Value.ToString()) == 1,

                StatusId = (int)Utilities.StringToPurchaseOrderStatus(rs.Fields.Item("DocStatus").Value.ToString()),

                UserId = rs.Fields.Item("UserSign2").Value.ToString(),

                AgentId = rs.Fields.Item("U_CL_CODAGE").Value.ToString(),
                AgentAddressId = rs.Fields.Item("U_CL_ADDAGE").Value.ToString(),

                DNIConsignatario = rs.Fields.Item("U_CL_DNICON").Value.ToString(),
                FirstNameConsignatario = rs.Fields.Item("U_CL_NOCONS").Value.ToString(),
                LastNameConsignatario = rs.Fields.Item("U_CL_APECON").Value.ToString(),
                PhoneConsignatario = rs.Fields.Item("U_CL_CELCON").Value.ToString(),
                EmailConsignatario = rs.Fields.Item("U_CL_EMACON").Value.ToString(),

                SubTotal = decimal.Parse(rs.Fields.Item("SUB_TOTAL").Value.ToString()),
                Impuesto = decimal.Parse(rs.Fields.Item("IGV").Value.ToString()),
                Total = decimal.Parse(rs.Fields.Item("TOTAL").Value.ToString()),
                IsCustomer = rs.Fields.Item("U_CL_ISCUST").Value.Equals(1)
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, SaleOrder obj) => table;
    }
}
