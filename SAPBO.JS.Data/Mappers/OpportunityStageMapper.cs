using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class OpportunityStageMapper : ISapB1AutoMapper<OpportunityStage>
    {
        public OpportunityStage Mapper(IRecordset rs)
        {
            return new OpportunityStage
            {
                Id = int.Parse(rs.Fields.Item("Num").Value.ToString()),
                Name = rs.Fields.Item("Descript").Value.ToString(),
                Index = int.Parse(rs.Fields.Item("StepId").Value.ToString()),
                ClosePercentage = decimal.Parse(rs.Fields.Item("CloPrcnt").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, OpportunityStage obj) => table;
    }
}
