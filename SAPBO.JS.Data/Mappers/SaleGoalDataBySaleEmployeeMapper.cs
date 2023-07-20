using SAPBO.JS.Model.Dto;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class SaleGoalDataBySaleEmployeeMapper : ISapB1AutoMapper<SaleGoalDataBySaleEmployee>
    {
        public SaleGoalDataBySaleEmployee Mapper(IRecordset rs)
        {
            return new SaleGoalDataBySaleEmployee
            {
                Year = int.Parse(rs.Fields.Item("ANIO").Value.ToString()),
                Month = int.Parse(rs.Fields.Item("MES").Value.ToString()),
                LineaGoal = decimal.Parse(rs.Fields.Item("META_LINEA").Value.ToString()),
                ImpresoGoal = decimal.Parse(rs.Fields.Item("META_IMPRESOS").Value.ToString()),
                FlexoGoal = decimal.Parse(rs.Fields.Item("META_FLEXOGRAFIA").Value.ToString()),
                OtroGoal = decimal.Parse(rs.Fields.Item("META_OTROS").Value.ToString()),
                TotalDolar = decimal.Parse(rs.Fields.Item("META_TOTAL").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, SaleGoalDataBySaleEmployee obj) => table;
    }
}
