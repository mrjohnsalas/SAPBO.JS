using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class MachineFailureMapper : ISapB1AutoMapper<MachineFailure>
    {
        public MachineFailure Mapper(IRecordset rs)
        {
            var mapper = new MachineFailure
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                ProductionMachineId = int.Parse(rs.Fields.Item("U_CL_CODMAQ").Value.ToString()),

                StartDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECINI").Value, rs.Fields.Item("U_CL_HORINI").Value).Value,
                FinalDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECFIN").Value, rs.Fields.Item("U_CL_HORFIN").Value).Value,

                FailureTypeId = int.Parse(rs.Fields.Item("U_CL_CODTFA").Value.ToString()),
                FailureSeverityId = int.Parse(rs.Fields.Item("U_CL_CODSFA").Value.ToString()),
                FailureCauseId = int.Parse(rs.Fields.Item("U_CL_CODCFA").Value.ToString()),
                FailureMechanismId = int.Parse(rs.Fields.Item("U_CL_CODMFA").Value.ToString()),
                FailureImpactId = int.Parse(rs.Fields.Item("U_CL_CODIFA").Value.ToString()),
                Remark = rs.Fields.Item("U_CL_MOTFAL").Value.ToString(),
                StopMachine = rs.Fields.Item("U_CL_CHKPLA").Value.ToString().Equals("1"),
                StatusId = int.Parse(rs.Fields.Item("U_ID_STATUS").Value.ToString())
            };

            if (mapper.StopMachine)
            {
                mapper.StopStartDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECINP").Value, rs.Fields.Item("U_CL_HORINP").Value);
                mapper.StopFinalDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECFIP").Value, rs.Fields.Item("U_CL_HORFIP").Value);
            }

            var otmId = rs.Fields.Item("U_CL_CODOTM").Value.ToString();
            if (!string.IsNullOrEmpty(otmId))
                mapper.MaintenanceWorkOrderId = int.Parse(otmId);

            var productionMachineZoneId = rs.Fields.Item("U_CL_CODZON").Value.ToString();
            if (!string.IsNullOrEmpty(productionMachineZoneId))
                mapper.ProductionMachineZoneId = int.Parse(productionMachineZoneId);

            return mapper;
        }

        public IUserTable SetValuesToUserTable(IUserTable table, MachineFailure obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_CODMAQ").Value = obj.ProductionMachineId.ToString("00");
            table.UserFields.Fields.Item("U_CL_FECINI").Value = obj.StartDate.ToString(AppFormats.Date);
            table.UserFields.Fields.Item("U_CL_HORINI").Value = obj.StartDate.ToString(AppFormats.Time);
            table.UserFields.Fields.Item("U_CL_FECFIN").Value = obj.FinalDate.ToString(AppFormats.Date);
            table.UserFields.Fields.Item("U_CL_HORFIN").Value = obj.FinalDate.ToString(AppFormats.Time);
            table.UserFields.Fields.Item("U_CL_CODTFA").Value = obj.FailureTypeId.ToString();
            table.UserFields.Fields.Item("U_CL_CODSFA").Value = obj.FailureSeverityId.ToString();
            table.UserFields.Fields.Item("U_CL_CODCFA").Value = obj.FailureCauseId.ToString();
            table.UserFields.Fields.Item("U_CL_CODMFA").Value = obj.FailureMechanismId.ToString();
            table.UserFields.Fields.Item("U_CL_CODIFA").Value = obj.FailureImpactId.ToString();
            table.UserFields.Fields.Item("U_CL_MOTFAL").Value = obj.Remark ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CHKPLA").Value = obj.StopMachine ? 1 : 0;

            if (obj.StopStartDate.HasValue)
            {
                table.UserFields.Fields.Item("U_CL_FECINP").Value = obj.StopStartDate.Value.ToString(AppFormats.Date);
                table.UserFields.Fields.Item("U_CL_HORINP").Value = obj.StopStartDate.Value.ToString(AppFormats.Time);
            }

            if (obj.StopFinalDate.HasValue)
            {
                table.UserFields.Fields.Item("U_CL_FECFIP").Value = obj.StopFinalDate.Value.ToString(AppFormats.Date);
                table.UserFields.Fields.Item("U_CL_HORFIP").Value = obj.StopFinalDate.Value.ToString(AppFormats.Time);
            }

            table.UserFields.Fields.Item("U_ID_STATUS").Value = obj.StatusId;

            table.UserFields.Fields.Item("U_CL_CODOTM").Value = obj.MaintenanceWorkOrderId.HasValue
                ? obj.MaintenanceWorkOrderId.Value.ToString()
                : string.Empty;

            table.UserFields.Fields.Item("U_CL_CODZON").Value = obj.ProductionMachineZoneId.HasValue
                ? obj.ProductionMachineZoneId.Value.ToString()
                : string.Empty;

            return table;
        }
    }
}
