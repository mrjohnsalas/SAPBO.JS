using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class WarehouseMapper : ISapB1AutoMapper<Warehouse>
    {
        public Warehouse Mapper(IRecordset rs)
        {
            return new Warehouse
            {
                Id = rs.Fields.Item("WhsCode").Value.ToString(),
                Name = rs.Fields.Item("WhsName").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, Warehouse obj) => table;
    }
}
