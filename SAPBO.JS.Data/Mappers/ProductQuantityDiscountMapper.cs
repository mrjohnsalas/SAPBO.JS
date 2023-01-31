using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductQuantityDiscountMapper : ISapB1AutoMapper<ProductQuantityDiscount>
    {
        public ProductQuantityDiscount Mapper(IRecordset rs)
        {
            return new ProductQuantityDiscount
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),

                ProductId = rs.Fields.Item("U_CL_CODART").Value.ToString(),

                MinQuantity = decimal.Parse(rs.Fields.Item("U_CL_MINRAN").Value.ToString()),
                MaxQuantity = decimal.Parse(rs.Fields.Item("U_CL_MAXRAN").Value.ToString()),
                XjeDiscount = decimal.Parse(rs.Fields.Item("U_CL_XJEVAL").Value.ToString()),

                StartDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECINI").Value).Value,
                FinalDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECFIN").Value).Value,

                BusinessPartnerId = rs.Fields.Item("U_CL_CODCLI").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductQuantityDiscount obj) => table;
    }
}
