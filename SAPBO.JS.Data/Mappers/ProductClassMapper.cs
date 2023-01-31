using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductClassMapper : ISapB1AutoMapper<ProductClass>
    {
        public ProductClass Mapper(IRecordset rs)
        {
            return new ProductClass
            {
                Id = rs.Fields.Item("Code").Value.ToString(),
                Name = rs.Fields.Item("Name").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductClass obj) => table;
    }
}
