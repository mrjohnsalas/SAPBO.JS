using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductFormulaConsumptionFactorMapper : ISapB1AutoMapper<ProductFormulaConsumptionFactor>
    {
        public ProductFormulaConsumptionFactor Mapper(IRecordset rs)
        {
            return new ProductFormulaConsumptionFactor
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                ProductFormulaId = int.Parse(rs.Fields.Item("U_CL_CODFOR").Value.ToString()),
                ProductMaterialTypeId = int.Parse(rs.Fields.Item("U_CL_CODTPA").Value.ToString()),
                From = decimal.Parse(rs.Fields.Item("U_CL_DESDE").Value.ToString()),
                Until = decimal.Parse(rs.Fields.Item("U_CL_HASTA").Value.ToString()),
                Factor = decimal.Parse(rs.Fields.Item("U_CL_FACTOR").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductFormulaConsumptionFactor obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_CODFOR").Value = obj.ProductFormulaId.ToString();
            table.UserFields.Fields.Item("U_CL_CODTPA").Value = obj.ProductMaterialTypeId.ToString();
            table.UserFields.Fields.Item("U_CL_DESDE").Value = (double)obj.From;
            table.UserFields.Fields.Item("U_CL_HASTA").Value = (double)obj.Until;
            table.UserFields.Fields.Item("U_CL_FACTOR").Value = (double)obj.Factor;

            return table;
        }
    }
}
