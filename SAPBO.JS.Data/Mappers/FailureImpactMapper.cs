using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class FailureImpactMapper : ISapB1AutoMapper<FailureImpact>
    {
        public FailureImpact Mapper(IRecordset rs)
        {
            return new FailureImpact
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("Name").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESIMP").Value.ToString(),
                StatusId = int.Parse(rs.Fields.Item("U_CL_FALSTS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, FailureImpact obj)
        {
            table.Name = obj.Name;
            table.UserFields.Fields.Item("U_CL_DESIMP").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_FALSTS").Value = obj.StatusId;

            return table;
        }
    }
}
