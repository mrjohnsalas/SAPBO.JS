using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductionProcessMapper : ISapB1AutoMapper<ProductionProcess>
    {
        public ProductionProcess Mapper(IRecordset rs)
        {
            return new ProductionProcess
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("U_CL_NAME").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESCRI").Value.ToString(),
                ProductionProcessTypeCostId = int.Parse(rs.Fields.Item("U_CL_CODTIP").Value.ToString()),
                Costo = decimal.Parse(rs.Fields.Item("U_CL_COSTO").Value.ToString()),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STATUS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductionProcess obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_NAME").Value = obj.Name ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_DESCRI").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CODTIP").Value = obj.ProductionProcessTypeCostId;
            table.UserFields.Fields.Item("U_CL_COSTO").Value = (double)obj.Costo;
            table.UserFields.Fields.Item("U_CL_STATUS").Value = obj.StatusId;

            return table;
        }
    }
}
