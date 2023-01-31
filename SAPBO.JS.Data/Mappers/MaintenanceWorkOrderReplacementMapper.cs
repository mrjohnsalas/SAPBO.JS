using SAPBO.JS.Model.Domain;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Data.Mappers
{
    public class MaintenanceWorkOrderReplacementMapper : ISapB1AutoMapper<MaintenanceWorkOrderReplacement>
    {
        public MaintenanceWorkOrderReplacement Mapper(IRecordset rs)
        {
            return new MaintenanceWorkOrderReplacement
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                MaintenanceWorkOrderId = int.Parse(rs.Fields.Item("U_CL_CODOTM").Value.ToString()),
                ReplacementId = rs.Fields.Item("U_CL_CODREP").Value.ToString(),
                PlannedQuantity = decimal.Parse(rs.Fields.Item("U_CL_CANTID").Value.ToString()),
                ConsumedQuantity = decimal.Parse(rs.Fields.Item("CANT_CONSUMO").Value.ToString()),
                TimeFrequencyId = int.Parse(rs.Fields.Item("U_CL_CODFRE").Value.ToString()),
                TimeFrequencyValue = decimal.Parse(rs.Fields.Item("U_CL_VALFRE").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, MaintenanceWorkOrderReplacement obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_CODOTM").Value = obj.MaintenanceWorkOrderId.ToString();
            table.UserFields.Fields.Item("U_CL_CODREP").Value = obj.ReplacementId;
            table.UserFields.Fields.Item("U_CL_CANTID").Value = (double)obj.PlannedQuantity;
            table.UserFields.Fields.Item("U_CL_CODFRE").Value = obj.TimeFrequencyId.ToString();
            table.UserFields.Fields.Item("U_CL_VALFRE").Value = (double)obj.TimeFrequencyValue;

            return table;
        }
    }
}
