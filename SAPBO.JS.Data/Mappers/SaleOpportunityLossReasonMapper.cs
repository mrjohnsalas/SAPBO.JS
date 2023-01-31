using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class SaleOpportunityLossReasonMapper : ISapB1AutoMapper<SaleOpportunityLossReason>
    {
        public SaleOpportunityLossReason Mapper(IRecordset rs)
        {
            return new SaleOpportunityLossReason
            {
                Id = int.Parse(rs.Fields.Item("Line").Value.ToString()),
                SaleOpportunityId = int.Parse(rs.Fields.Item("OpportId").Value.ToString()),
                LossReasonId = int.Parse(rs.Fields.Item("ReasondId").Value.ToString()),
                Notes = rs.Fields.Item("U_CL_DETMOT").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, SaleOpportunityLossReason obj) => table;
    }
}
