using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class JobMapper : ISapB1AutoMapper<Job>
    {
        public Job Mapper(IRecordset rs)
        {
            return new Job
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("Name").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESPTR").Value.ToString(),
                BusinessUnitId = rs.Fields.Item("U_CL_UNDNEG").Value.ToString(),
                CostHour = decimal.Parse(rs.Fields.Item("U_CL_COSXHH").Value.ToString()),
                StatusId = int.Parse(rs.Fields.Item("U_CL_PUTSTS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, Job obj)
        {
            table.Name = obj.Name;
            table.UserFields.Fields.Item("U_CL_DESPTR").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_UNDNEG").Value = obj.BusinessUnitId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_COSXHH").Value = (double)obj.CostHour;
            table.UserFields.Fields.Item("U_CL_PUTSTS").Value = obj.StatusId;

            return table;
        }
    }
}
