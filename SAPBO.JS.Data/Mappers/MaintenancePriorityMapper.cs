using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class MaintenancePriorityMapper : ISapB1AutoMapper<MaintenancePriority>
    {
        public MaintenancePriority Mapper(IRecordset rs)
        {
            return new MaintenancePriority
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("Name").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESPRI").Value.ToString(),
                StatusId = int.Parse(rs.Fields.Item("U_CL_PRISTS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, MaintenancePriority obj)
        {
            table.Name = obj.Name;
            table.UserFields.Fields.Item("U_CL_DESPRI").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_PRISTS").Value = obj.StatusId;

            return table;
        }
    }
}
