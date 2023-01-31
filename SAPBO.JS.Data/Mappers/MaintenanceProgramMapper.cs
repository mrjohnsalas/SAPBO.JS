using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class MaintenanceProgramMapper : ISapB1AutoMapper<MaintenanceProgram>
    {
        public MaintenanceProgram Mapper(IRecordset rs)
        {
            return new MaintenanceProgram
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Description = rs.Fields.Item("U_CL_DESCPM").Value.ToString(),
                MaintenancePriorityId = int.Parse(rs.Fields.Item("U_CL_CODPRI").Value.ToString()),
                MaintenanceTypeId = int.Parse(rs.Fields.Item("U_CL_CODTMP").Value.ToString()),
                ProductionMachineId = int.Parse(rs.Fields.Item("U_CL_CODMAQ").Value.ToString()),
                TimeFrequencyId = int.Parse(rs.Fields.Item("U_CL_CODFRE").Value.ToString()),
                TimeFrequencyValue = decimal.Parse(rs.Fields.Item("U_CL_VALFRE").Value.ToString()),
                EstimatedTime = Utilities.ValueToClock(rs.Fields.Item("U_CL_TIEEST").Value),
                LastDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECULT").Value, rs.Fields.Item("U_CL_HORULT").Value),
                Remark = rs.Fields.Item("U_CL_COMEPM").Value.ToString(),
                StopPlant = rs.Fields.Item("U_CL_CHKPLA").Value.ToString().Equals("1"),
                StopMachine = rs.Fields.Item("U_CL_CHKMAQ").Value.ToString().Equals("1"),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STATUS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, MaintenanceProgram obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_DESCPM").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CODPRI").Value = obj.MaintenancePriorityId.ToString();
            table.UserFields.Fields.Item("U_CL_CODTMP").Value = obj.MaintenanceTypeId.ToString();
            table.UserFields.Fields.Item("U_CL_CODMAQ").Value = obj.ProductionMachineId.ToString("00");
            table.UserFields.Fields.Item("U_CL_CODFRE").Value = obj.TimeFrequencyId.ToString();
            table.UserFields.Fields.Item("U_CL_VALFRE").Value = (double)obj.TimeFrequencyValue;
            table.UserFields.Fields.Item("U_CL_TIEEST").Value = double.Parse(obj.EstimatedTime.Replace(":", "."));

            if (obj.LastDate.HasValue)
            {
                table.UserFields.Fields.Item("U_CL_FECULT").Value = obj.LastDate.Value.ToString(AppFormats.Date);
                table.UserFields.Fields.Item("U_CL_HORULT").Value = obj.LastDate.Value.ToString(AppFormats.Time);
            }

            table.UserFields.Fields.Item("U_CL_COMEPM").Value = obj.Remark ?? string.Empty;

            table.UserFields.Fields.Item("U_CL_CHKPLA").Value = obj.StopPlant ? 1 : 0;
            table.UserFields.Fields.Item("U_CL_CHKMAQ").Value = obj.StopMachine ? 1 : 0;

            table.UserFields.Fields.Item("U_CL_STATUS").Value = obj.StatusId;

            return table;
        }
    }
}
