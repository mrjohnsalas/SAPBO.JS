using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class PurchaseOrderDetailMapper : ISapB1AutoMapper<PurchaseOrderDetail>
    {
        public PurchaseOrderDetail Mapper(IRecordset rs)
        {
            return new PurchaseOrderDetail
            {
                Id = int.Parse(rs.Fields.Item("LineNum").Value.ToString()),
                PurchaseOrderId = int.Parse(rs.Fields.Item("DocEntry").Value.ToString()),

                ProductId = rs.Fields.Item("ItemCode").Value.ToString(),
                ProductDetail = rs.Fields.Item("Text").Value.ToString(),

                MedidaId = rs.Fields.Item("unitMsr").Value.ToString(),

                WarehouseId = rs.Fields.Item("WhsCode").Value.ToString(),

                DeliveryDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("ShipDate").Value).Value,

                Quantity = decimal.Parse(rs.Fields.Item("Quantity").Value.ToString()),
                PendingQuantity = decimal.Parse(rs.Fields.Item("OpenInvQty").Value.ToString()),
                PriceWithoutDiscount = decimal.Parse(rs.Fields.Item("PRECIO_S_DESC").Value.ToString()),
                TotalWithoutDiscount = decimal.Parse(rs.Fields.Item("TOTAL_S_DESC").Value.ToString()),
                XjeDiscount = decimal.Parse(rs.Fields.Item("DiscPrcnt").Value.ToString()),
                TotalDiscount = decimal.Parse(rs.Fields.Item("TOTAL_DESC").Value.ToString()),
                Total = decimal.Parse(rs.Fields.Item("TOTAL_C_DESC").Value.ToString()),

                StatusId = rs.Fields.Item("LineStatus").Value.ToString() == "C" ? (int)Enums.StatusType.Completado : (int)Enums.StatusType.Abierto
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, PurchaseOrderDetail obj) => table;
    }
}
