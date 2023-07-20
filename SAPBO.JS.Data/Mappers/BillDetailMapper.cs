using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class BillDetailMapper : ISapB1AutoMapper<BillDetail>
    {
        public BillDetail Mapper(IRecordset rs)
        {
            return new BillDetail
            {
                Id = int.Parse(rs.Fields.Item("LineNum").Value.ToString()),
                
                BillId = int.Parse(rs.Fields.Item("DocEntry").Value.ToString()),

                ProductId = rs.Fields.Item("ItemCode").Value.ToString(),
                ProductDetail = rs.Fields.Item("Text").Value.ToString(),

                MedidaId = rs.Fields.Item("unitMsr").Value.ToString(),

                WarehouseId = rs.Fields.Item("WhsCode").Value.ToString(),

                Quantity = decimal.Parse(rs.Fields.Item("Quantity").Value.ToString()),

                UnitPrice = decimal.Parse(rs.Fields.Item("UNIT_PRICE").Value.ToString()),
                Total = decimal.Parse(rs.Fields.Item("TOTAL").Value.ToString()),

                StatusId = rs.Fields.Item("LineStatus").Value.ToString() == "C" 
                ? (int)Enums.StatusType.Completado 
                : (int)Enums.StatusType.Abierto
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, BillDetail obj) => table;
    }
}
