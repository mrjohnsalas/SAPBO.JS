using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class MaintenanceWorkOrderEmployeeMapper : ISapB1AutoMapper<MaintenanceWorkOrderEmployee>
    {
        public MaintenanceWorkOrderEmployee Mapper(IRecordset rs)
        {
            return new MaintenanceWorkOrderEmployee
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                MaintenanceWorkOrderId = int.Parse(rs.Fields.Item("U_CL_CODOTM").Value.ToString()),
                EmployeeId = int.Parse(rs.Fields.Item("U_CL_CODEMP").Value.ToString()),
                Task = rs.Fields.Item("U_CL_TASKEM").Value.ToString(),
                EstimatedTime = Utilities.ValueToClock(rs.Fields.Item("U_CL_TIEEST").Value)
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, MaintenanceWorkOrderEmployee obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_CODOTM").Value = obj.MaintenanceWorkOrderId.ToString();
            table.UserFields.Fields.Item("U_CL_CODEMP").Value = obj.EmployeeId.ToString();
            table.UserFields.Fields.Item("U_CL_TASKEM").Value = obj.Task ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_TIEEST").Value = double.Parse(obj.EstimatedTime.Replace(":", "."));

            return table;
        }
    }
}
