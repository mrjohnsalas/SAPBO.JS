using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class MaintenanceTypeMapper : ISapB1AutoMapper<MaintenanceType>
    {
        public MaintenanceType Mapper(IRecordset rs)
        {
            return new MaintenanceType
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("Name").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESTIP").Value.ToString(),
                RequireFailure = rs.Fields.Item("U_CL_REQFAL").Value.ToString().Equals("1"),
                StatusId = int.Parse(rs.Fields.Item("U_CL_TMASTS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, MaintenanceType obj)
        {
            table.Name = obj.Name;
            table.UserFields.Fields.Item("U_CL_DESTIP").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_REQFAL").Value = obj.RequireFailure ? 1 : 0;
            table.UserFields.Fields.Item("U_CL_TMASTS").Value = obj.StatusId;

            return table;
        }
    }
}
