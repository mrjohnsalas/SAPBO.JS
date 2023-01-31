using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class MaintenanceProgramJobMapper : ISapB1AutoMapper<MaintenanceProgramJob>
    {
        public MaintenanceProgramJob Mapper(IRecordset rs)
        {
            return new MaintenanceProgramJob
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                MaintenanceProgramId = int.Parse(rs.Fields.Item("U_CL_CODPMA").Value.ToString()),
                JobId = int.Parse(rs.Fields.Item("U_CL_CODEMP").Value.ToString()),
                EstimatedTime = Utilities.ValueToClock(rs.Fields.Item("U_CL_TIEEST").Value),
                Quantity = decimal.Parse(rs.Fields.Item("U_CL_CANTID").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, MaintenanceProgramJob obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_CODPMA").Value = obj.MaintenanceProgramId.ToString();
            table.UserFields.Fields.Item("U_CL_CODEMP").Value = obj.JobId.ToString();
            table.UserFields.Fields.Item("U_CL_TIEEST").Value = double.Parse(obj.EstimatedTime.Replace(":", "."));
            table.UserFields.Fields.Item("U_CL_CANTID").Value = (double)obj.Quantity;

            return table;
        }
    }
}
