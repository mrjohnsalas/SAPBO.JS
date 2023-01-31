using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductGroupMapper : ISapB1AutoMapper<ProductGroup>
    {
        public ProductGroup Mapper(IRecordset rs)
        {
            return new ProductGroup
            {
                Id = rs.Fields.Item("Code").Value.ToString(),
                Name = rs.Fields.Item("Name").Value.ToString(),
                Description = rs.Fields.Item("U_CL_GRPDES").Value.ToString(),
                ProductSuperGroupId = rs.Fields.Item("U_CL_SUPGRP").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductGroup obj) => table;
    }
}
