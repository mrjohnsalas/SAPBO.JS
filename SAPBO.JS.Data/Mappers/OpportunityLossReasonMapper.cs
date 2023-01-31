using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class OpportunityLossReasonMapper : ISapB1AutoMapper<OpportunityLossReason>
    {
        public OpportunityLossReason Mapper(IRecordset rs)
        {
            return new OpportunityLossReason
            {
                Id = int.Parse(rs.Fields.Item("Num").Value.ToString()),
                Name = rs.Fields.Item("Descript").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, OpportunityLossReason obj) => table;
    }
}
