using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductionMachineMapper : ISapB1AutoMapper<ProductionMachine>
    {
        public ProductionMachine Mapper(IRecordset rs)
        {
            return new ProductionMachine
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("Name").Value.ToString(),
                StatusId = Utilities.GetStatusIdByText(rs.Fields.Item("U_CL_MAQSTS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductionMachine obj)
        {
            table.Name = obj.Name;
            table.UserFields.Fields.Item("U_CL_UNDNEG").Value = "PP";
            table.UserFields.Fields.Item("U_CL_MAQSTS").Value = Utilities.GetTextByStatusId(obj.StatusId, false);

            return table;
        }
    }
}
