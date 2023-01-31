using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductionMachineZoneMapper : ISapB1AutoMapper<ProductionMachineZone>
    {
        public ProductionMachineZone Mapper(IRecordset rs)
        {
            return new ProductionMachineZone
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                ProductionMachineId = int.Parse(rs.Fields.Item("U_CL_CODMAQ").Value.ToString()),
                Name = rs.Fields.Item("U_CL_NAME").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESMAQ").Value.ToString(),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STATUS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductionMachineZone obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_NAME").Value = obj.Name ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CODMAQ").Value = obj.ProductionMachineId.ToString("00");
            table.UserFields.Fields.Item("U_CL_DESMAQ").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_STATUS").Value = obj.StatusId;

            return table;
        }
    }
}
