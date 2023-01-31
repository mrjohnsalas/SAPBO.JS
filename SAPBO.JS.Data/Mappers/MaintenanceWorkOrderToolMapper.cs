using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class MaintenanceWorkOrderToolMapper : ISapB1AutoMapper<MaintenanceWorkOrderTool>
    {
        public MaintenanceWorkOrderTool Mapper(IRecordset rs)
        {
            return new MaintenanceWorkOrderTool
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                MaintenanceWorkOrderId = int.Parse(rs.Fields.Item("U_CL_CODOTM").Value.ToString()),
                MaintenanceToolId = int.Parse(rs.Fields.Item("U_CL_CODHER").Value.ToString()),
                Quantity = decimal.Parse(rs.Fields.Item("U_CL_CANTID").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, MaintenanceWorkOrderTool obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_CODOTM").Value = obj.MaintenanceWorkOrderId.ToString();
            table.UserFields.Fields.Item("U_CL_CODHER").Value = obj.MaintenanceToolId.ToString();
            table.UserFields.Fields.Item("U_CL_CANTID").Value = (double)obj.Quantity;

            return table;
        }
    }
}
