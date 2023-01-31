using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class SaleOrderDetailMapper : ISapB1AutoMapper<SaleOrderDetail>
    {
        public SaleOrderDetail Mapper(IRecordset rs)
        {
            return new SaleOrderDetail
            {
                Id = int.Parse(rs.Fields.Item("LineNum").Value.ToString()),
                SaleOrderId = int.Parse(rs.Fields.Item("DocEntry").Value.ToString()),

                ProductId = rs.Fields.Item("ItemCode").Value.ToString(),
                ProductDetail = rs.Fields.Item("Text").Value.ToString(),

                MedidaId = rs.Fields.Item("unitMsr").Value.ToString(),

                WarehouseId = rs.Fields.Item("WhsCode").Value.ToString(),

                Quantity = decimal.Parse(rs.Fields.Item("Quantity").Value.ToString()),
                PendingQuantity = decimal.Parse(rs.Fields.Item("OpenInvQty").Value.ToString()),

                BasePrice = decimal.Parse(rs.Fields.Item("PRECIO_BASE").Value.ToString()),
                BaseTotal = decimal.Parse(rs.Fields.Item("TOTAL_SIN_DESC").Value.ToString()),

                XjeCustomerDiscount = decimal.Parse(rs.Fields.Item("XJE_DESC_CLI").Value.ToString()),
                TotalCustomerDiscount = decimal.Parse(rs.Fields.Item("DESC_CLI").Value.ToString()),
                CustomerPrice = decimal.Parse(rs.Fields.Item("PRECIO_CLI").Value.ToString()),
                CustomerTotal = decimal.Parse(rs.Fields.Item("TOTAL_C_DESC_CLI").Value.ToString()),

                XjeQuantityDiscount = decimal.Parse(rs.Fields.Item("XJE_DESC_CANT").Value.ToString()),
                TotalQuantityDiscount = decimal.Parse(rs.Fields.Item("DESC_CANT").Value.ToString()),
                FinalPrice = decimal.Parse(rs.Fields.Item("PRECIO_FINAL").Value.ToString()),
                FinalTotal = decimal.Parse(rs.Fields.Item("TOTAL_C_DESC_CANT").Value.ToString()),

                XjeComision = decimal.Parse(rs.Fields.Item("XJE_COMISION").Value.ToString()),
                TotalComision = decimal.Parse(rs.Fields.Item("TOTAL_COMISION").Value.ToString()),

                StatusId = rs.Fields.Item("LineStatus").Value.ToString() == "C" ? (int)Enums.StatusType.Completado : (int)Enums.StatusType.Abierto
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, SaleOrderDetail obj) => table;
    }
}
