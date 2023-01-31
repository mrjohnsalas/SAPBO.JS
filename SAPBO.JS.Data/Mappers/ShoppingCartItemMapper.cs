using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ShoppingCartItemMapper : ISapB1AutoMapper<ShoppingCartItem>
    {
        public ShoppingCartItem Mapper(IRecordset rs)
        {
            return new ShoppingCartItem
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                UserId = rs.Fields.Item("U_CL_USERID").Value.ToString(),
                ProductId = rs.Fields.Item("U_CL_CODPRD").Value.ToString(),
                Quantity = decimal.Parse(rs.Fields.Item("U_CL_CANPRD").Value.ToString()),
                ProductDetail = rs.Fields.Item("U_CL_PRDDTS").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ShoppingCartItem obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_USERID").Value = obj.UserId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CODPRD").Value = obj.ProductId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CANPRD").Value = (double)obj.Quantity;
            table.UserFields.Fields.Item("U_CL_PRDDTS").Value = obj.ProductDetail ?? string.Empty;

            return table;
        }
    }
}
