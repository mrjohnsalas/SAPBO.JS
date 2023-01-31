using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductSuperGroupMapper : ISapB1AutoMapper<ProductSuperGroup>
    {
        public ProductSuperGroup Mapper(IRecordset rs)
        {
            return new ProductSuperGroup
            {
                Id = rs.Fields.Item("Code").Value.ToString(),
                Name = rs.Fields.Item("Name").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductSuperGroup obj) => table;
    }
}
