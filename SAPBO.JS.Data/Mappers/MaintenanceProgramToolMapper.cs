using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class MaintenanceProgramToolMapper : ISapB1AutoMapper<MaintenanceProgramTool>
    {
        public MaintenanceProgramTool Mapper(IRecordset rs)
        {
            return new MaintenanceProgramTool
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                MaintenanceProgramId = int.Parse(rs.Fields.Item("U_CL_CODPMA").Value.ToString()),
                MaintenanceToolId = int.Parse(rs.Fields.Item("U_CL_CODHER").Value.ToString()),
                Quantity = decimal.Parse(rs.Fields.Item("U_CL_CANTID").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, MaintenanceProgramTool obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_CODPMA").Value = obj.MaintenanceProgramId.ToString();
            table.UserFields.Fields.Item("U_CL_CODHER").Value = obj.MaintenanceToolId.ToString();
            table.UserFields.Fields.Item("U_CL_CANTID").Value = (double)obj.Quantity;

            return table;
        }
    }
}
