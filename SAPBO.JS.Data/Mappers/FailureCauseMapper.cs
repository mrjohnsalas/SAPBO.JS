using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class FailureCauseMapper : ISapB1AutoMapper<FailureCause>
    {
        public FailureCause Mapper(IRecordset rs)
        {
            return new FailureCause
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("Name").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESCAU").Value.ToString(),
                StatusId = int.Parse(rs.Fields.Item("U_CL_FALSTS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, FailureCause obj)
        {
            table.Name = obj.Name;
            table.UserFields.Fields.Item("U_CL_DESCAU").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_FALSTS").Value = obj.StatusId;

            return table;
        }
    }
}
