using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class TimeFrequencyMapper : ISapB1AutoMapper<TimeFrequency>
    {
        public TimeFrequency Mapper(IRecordset rs)
        {
            return new TimeFrequency
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("Name").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESCRI").Value.ToString(),
                StatusId = int.Parse(rs.Fields.Item("U_CL_FRESTS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, TimeFrequency obj)
        {
            table.Name = obj.Name;
            table.UserFields.Fields.Item("U_CL_DESCRI").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_FRESTS").Value = obj.StatusId;

            return table;
        }
    }
}
