using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class SaleOpportunityStageMapper : ISapB1AutoMapper<SaleOpportunityStage>
    {
        public SaleOpportunityStage Mapper(IRecordset rs)
        {
            return new SaleOpportunityStage
            {
                Id = int.Parse(rs.Fields.Item("Line").Value.ToString()),
                SaleOpportunityId = int.Parse(rs.Fields.Item("OpprId").Value.ToString()),
                StartDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("OpenDate").Value).Value,
                CloseDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("CloseDate").Value).Value,

                SaleEmployeeId = int.Parse(rs.Fields.Item("SlpCode").Value.ToString()),
                SaleEmployee = rs.Fields.Item("SaleEmployee").Value.ToString(),
                OpportunityStageId = int.Parse(rs.Fields.Item("Step_Id").Value.ToString()),

                ClosePercentage = decimal.Parse(rs.Fields.Item("ClosePrcnt").Value.ToString()),
                PotentialAmount = decimal.Parse(rs.Fields.Item("MaxSumLoc").Value.ToString()),
                WeightedAmount = decimal.Parse(rs.Fields.Item("WtSumLoc").Value.ToString()),

                EmployeeId = int.Parse(rs.Fields.Item("Owner").Value.ToString()),
                Employee = rs.Fields.Item("Employee").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, SaleOpportunityStage obj) => table;
    }
}
