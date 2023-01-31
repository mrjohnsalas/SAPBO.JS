using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductFormulaProductionProcessMapper : ISapB1AutoMapper<ProductFormulaProductionProcess>
    {
        public ProductFormulaProductionProcess Mapper(IRecordset rs)
        {
            return new ProductFormulaProductionProcess
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                ProductFormulaId = int.Parse(rs.Fields.Item("U_CL_CODFOR").Value.ToString()),
                ProductMaterialTypeId = int.Parse(rs.Fields.Item("U_CL_CODTPA").Value.ToString()),
                ProductionProcessId = int.Parse(rs.Fields.Item("U_CL_CODPRO").Value.ToString()),
                PreparationTime = Utilities.ValueToClock(rs.Fields.Item("U_CL_TIEPRE").Value),
                Performance = decimal.Parse(rs.Fields.Item("U_CL_RENDIM").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductFormulaProductionProcess obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_CODFOR").Value = obj.ProductFormulaId.ToString();
            table.UserFields.Fields.Item("U_CL_CODTPA").Value = obj.ProductMaterialTypeId.ToString();
            table.UserFields.Fields.Item("U_CL_CODPRO").Value = obj.ProductionProcessId.ToString();
            table.UserFields.Fields.Item("U_CL_TIEPRE").Value = double.Parse(obj.PreparationTime.Replace(":", "."));
            table.UserFields.Fields.Item("U_CL_RENDIM").Value = (double)obj.Performance;

            return table;
        }
    }
}
