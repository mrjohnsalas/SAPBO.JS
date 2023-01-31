using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ReplacementMapper : ISapB1AutoMapper<Replacement>
    {
        public Replacement Mapper(IRecordset rs)
        {
            return new Replacement
            {
                Id = rs.Fields.Item("ItemCode").Value.ToString(),
                Name = rs.Fields.Item("ItemName").Value.ToString(),
                MedidaId = rs.Fields.Item("InvntryUom").Value.ToString(),
                Stock = decimal.Parse(rs.Fields.Item("Available").Value.ToString()),
                Cost = decimal.Parse(rs.Fields.Item("AvgPrice").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, Replacement obj) => table;
    }
}
