using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class PurchaseOrderMapper : ISapB1AutoMapper<PurchaseOrder>
    {
        public PurchaseOrder Mapper(IRecordset rs)
        {
            return new PurchaseOrder
            {
                Id = int.Parse(rs.Fields.Item("DocEntry").Value.ToString()),

                BusinessPartnerId = rs.Fields.Item("CardCode").Value.ToString(),

                PostingDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("DocDate").Value).Value,
                DeliveryDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("DocDueDate").Value).Value,
                DocumentDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("TaxDate").Value).Value,

                CurrencyId = rs.Fields.Item("DocCur").Value.ToString(),

                ContactId = int.Parse(rs.Fields.Item("CntctCode").Value.ToString()),

                PaymentId = int.Parse(rs.Fields.Item("GroupNum").Value.ToString()),

                Rate = decimal.Parse(rs.Fields.Item("DocRate").Value.ToString()),

                PurchaseEmployeeId = int.Parse(rs.Fields.Item("SlpCode").Value.ToString()),

                ShipAddressId = rs.Fields.Item("Address2Id").Value.ToString(),
                ShipAddress = rs.Fields.Item("Address2").Value.ToString(),

                BillAddressId = rs.Fields.Item("PayToCode").Value.ToString(),

                Remarks = rs.Fields.Item("Comments").Value.ToString(),
                ReferenceNumber = rs.Fields.Item("NumAtCard").Value.ToString(),
                CotizacionNumber = rs.Fields.Item("U_CL_NROCOT").Value.ToString(),

                AfectoDetraccion = int.Parse(rs.Fields.Item("CHKDET").Value.ToString()) == 1,
                IncluyePercepcion = int.Parse(rs.Fields.Item("CHKPER").Value.ToString()) == 1,

                StatusId = (int)Utilities.StringToPurchaseOrderStatus(rs.Fields.Item("DocStatus").Value.ToString()),

                UserId = rs.Fields.Item("UserSign2").Value.ToString(),

                SubTotal = decimal.Parse(rs.Fields.Item("SUB_TOTAL").Value.ToString()),
                Impuesto = decimal.Parse(rs.Fields.Item("IGV").Value.ToString()),
                Total = decimal.Parse(rs.Fields.Item("TOTAL").Value.ToString()),
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, PurchaseOrder obj) => table;
    }
}
