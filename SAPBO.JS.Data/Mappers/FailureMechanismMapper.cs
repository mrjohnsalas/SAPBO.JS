using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class FailureMechanismMapper : ISapB1AutoMapper<FailureMechanism>
    {
        public FailureMechanism Mapper(IRecordset rs)
        {
            return new FailureMechanism
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("Name").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESMEC").Value.ToString(),
                StatusId = int.Parse(rs.Fields.Item("U_CL_FALSTS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, FailureMechanism obj)
        {
            table.Name = obj.Name;
            table.UserFields.Fields.Item("U_CL_DESMEC").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_FALSTS").Value = obj.StatusId;

            return table;
        }
    }
}
