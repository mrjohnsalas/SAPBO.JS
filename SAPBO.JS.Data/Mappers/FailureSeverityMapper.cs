using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class FailureSeverityMapper : ISapB1AutoMapper<FailureSeverity>
    {
        public FailureSeverity Mapper(IRecordset rs)
        {
            return new FailureSeverity
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("Name").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESSEV").Value.ToString(),
                StatusId = int.Parse(rs.Fields.Item("U_CL_FALSTS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, FailureSeverity obj)
        {
            table.Name = obj.Name;
            table.UserFields.Fields.Item("U_CL_DESSEV").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_FALSTS").Value = obj.StatusId;

            return table;
        }
    }
}
