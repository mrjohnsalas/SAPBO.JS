using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class MaintenanceProgramReplacementMapper : ISapB1AutoMapper<MaintenanceProgramReplacement>
    {
        public MaintenanceProgramReplacement Mapper(IRecordset rs)
        {
            return new MaintenanceProgramReplacement
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                MaintenanceProgramId = int.Parse(rs.Fields.Item("U_CL_CODPMA").Value.ToString()),
                ReplacementId = rs.Fields.Item("U_CL_CODREP").Value.ToString(),
                Quantity = decimal.Parse(rs.Fields.Item("U_CL_CANTID").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, MaintenanceProgramReplacement obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_CODPMA").Value = obj.MaintenanceProgramId.ToString();
            table.UserFields.Fields.Item("U_CL_CODREP").Value = obj.ReplacementId;
            table.UserFields.Fields.Item("U_CL_CANTID").Value = (double)obj.Quantity;

            return table;
        }
    }
}
