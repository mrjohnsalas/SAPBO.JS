using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class SaleOpportunityMapper : ISapB1AutoMapper<SaleOpportunity>
    {
        public SaleOpportunity Mapper(IRecordset rs)
        {
            return new SaleOpportunity
            {
                Id = int.Parse(rs.Fields.Item("OpprId").Value.ToString()),
                SaleOpportunityType = Utilities.StringToSaleOpportunityType(rs.Fields.Item("OpprType").Value),
                BusinessPartnerId = rs.Fields.Item("CardCode").Value.ToString(),
                ContactId = int.Parse(rs.Fields.Item("CprCode").Value.ToString()),

                SaleEmployeeId = int.Parse(rs.Fields.Item("SlpCode").Value.ToString()),
                SaleEmployee = rs.Fields.Item("SaleEmployee").Value.ToString(),

                EmployeeId = int.Parse(rs.Fields.Item("Owner").Value.ToString()),
                Employee = rs.Fields.Item("Employee").Value.ToString(),

                Subject = rs.Fields.Item("Name").Value.ToString(),
                Notes = rs.Fields.Item("Memo").Value.ToString(),

                StartDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("OpenDate").Value),
                CloseDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("CloseDate").Value),
                ExpectedCloseDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("PredDate").Value).Value,

                ClosePercentage = decimal.Parse(rs.Fields.Item("CloPrcnt").Value.ToString()),
                PotentialAmount = decimal.Parse(rs.Fields.Item("MaxSumLoc").Value.ToString()),
                WeightedAmount = decimal.Parse(rs.Fields.Item("WtSumLoc").Value.ToString()),

                SaleOpportunityInterestLevel = Utilities.StringToSaleOpportunityInterestLevel(rs.Fields.Item("IntRate").Value.ToString()),
                SaleOpportunityStatus = Utilities.StringToSaleOpportunityStatus(rs.Fields.Item("Status").Value)
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, SaleOpportunity obj) => table;
    }
}
