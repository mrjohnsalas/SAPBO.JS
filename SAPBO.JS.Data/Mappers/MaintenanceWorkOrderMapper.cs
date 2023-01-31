using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class MaintenanceWorkOrderMapper : ISapB1AutoMapper<MaintenanceWorkOrder>
    {
        public MaintenanceWorkOrder Mapper(IRecordset rs)
        {
            var mapper = new MaintenanceWorkOrder
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Description = rs.Fields.Item("U_CL_DESOTM").Value.ToString(),
                MaintenancePriorityId = int.Parse(rs.Fields.Item("U_CL_CODPRI").Value.ToString()),
                MaintenanceTypeId = int.Parse(rs.Fields.Item("U_CL_CODOTM").Value.ToString()),

                StartDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECINI").Value, rs.Fields.Item("U_CL_HORINI").Value).Value,
                FinalDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECFIN").Value, rs.Fields.Item("U_CL_HORFIN").Value),

                EmployeeId = int.Parse(rs.Fields.Item("U_CL_CODSUP").Value.ToString()),

                Remark = rs.Fields.Item("U_CL_COMOTM").Value.ToString(),
                StopPlant = rs.Fields.Item("U_CL_CHKPLA").Value.ToString().Equals("1"),
                StopMachine = rs.Fields.Item("U_CL_CHKMAQ").Value.ToString().Equals("1"),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STSOTM").Value.ToString()),
                OtmTypeId = int.Parse(rs.Fields.Item("U_CL_TIPOTM").Value.ToString()),
                CostCenterId = rs.Fields.Item("U_CL_CODCC").Value.ToString(),
                EffectiveHours = Utilities.ValueToClock(rs.Fields.Item("U_CL_HOREFE").Value)
            };

            var mpId = rs.Fields.Item("U_CL_IDPMTO").Value.ToString();
            if (!string.IsNullOrEmpty(mpId))
                mapper.MaintenanceProgramId = int.Parse(mpId);

            var productionMachineZoneId = rs.Fields.Item("U_CL_CODZON").Value.ToString();
            if (!string.IsNullOrEmpty(productionMachineZoneId))
                mapper.ProductionMachineZoneId = int.Parse(productionMachineZoneId);

            var productionMachineId = rs.Fields.Item("U_CL_CODMAQ").Value.ToString();
            if (!string.IsNullOrEmpty(productionMachineId))
                mapper.ProductionMachineId = int.Parse(productionMachineId);

            return mapper;
        }

        public IUserTable SetValuesToUserTable(IUserTable table, MaintenanceWorkOrder obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_DESOTM").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CODPRI").Value = obj.MaintenancePriorityId.ToString();
            table.UserFields.Fields.Item("U_CL_CODOTM").Value = obj.MaintenanceTypeId.ToString();

            table.UserFields.Fields.Item("U_CL_TIPOTM").Value = obj.OtmTypeId;

            if (obj.ProductionMachineId.HasValue)
                table.UserFields.Fields.Item("U_CL_CODMAQ").Value = obj.ProductionMachineId.Value.ToString("00");

            if (!string.IsNullOrEmpty(obj.CostCenterId))
                table.UserFields.Fields.Item("U_CL_CODCC").Value = obj.CostCenterId;

            table.UserFields.Fields.Item("U_CL_FECINI").Value = obj.StartDate.ToString(AppFormats.Date);
            table.UserFields.Fields.Item("U_CL_HORINI").Value = obj.StartDate.ToString(AppFormats.Time);

            if (obj.FinalDate.HasValue)
            {
                table.UserFields.Fields.Item("U_CL_FECFIN").Value = obj.FinalDate.Value.ToString(AppFormats.Date);
                table.UserFields.Fields.Item("U_CL_HORFIN").Value = obj.FinalDate.Value.ToString(AppFormats.Time);
            }

            table.UserFields.Fields.Item("U_CL_CODSUP").Value = obj.EmployeeId.ToString();
            table.UserFields.Fields.Item("U_CL_COMOTM").Value = obj.Remark ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CHKPLA").Value = obj.StopPlant ? 1 : 0;
            table.UserFields.Fields.Item("U_CL_CHKMAQ").Value = obj.StopMachine ? 1 : 0;

            table.UserFields.Fields.Item("U_CL_STSOTM").Value = obj.StatusId;

            table.UserFields.Fields.Item("U_CL_IDPMTO").Value = obj.MaintenanceProgramId.HasValue
                ? obj.MaintenanceProgramId.Value.ToString()
                : string.Empty;

            table.UserFields.Fields.Item("U_CL_HOREFE").Value = double.Parse(obj.EffectiveHours.Replace(":", "."));

            table.UserFields.Fields.Item("U_CL_CODZON").Value = obj.ProductionMachineZoneId.HasValue
                ? obj.ProductionMachineZoneId.Value.ToString()
                : string.Empty;

            return table;
        }
    }
}
