using SAPBO.JS.Model.Dto;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class TopBilledProductMapper : ISapB1AutoMapper<TopBilledProduct>
    {
        public TopBilledProduct Mapper(IRecordset rs)
        {
            return new TopBilledProduct
            {
                ProductId = rs.Fields.Item("COD_ARTICULO").Value.ToString(),
                Quantity = decimal.Parse(rs.Fields.Item("CANTIDAD").Value.ToString()),
                TotalDolar = decimal.Parse(rs.Fields.Item("TOTAL_DOLAR").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, TopBilledProduct obj) => table;
    }
}
