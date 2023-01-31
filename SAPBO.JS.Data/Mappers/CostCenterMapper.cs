using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class CostCenterMapper : ISapB1AutoMapper<CostCenter>
    {
        public CostCenter Mapper(IRecordset rs)
        {
            return new CostCenter
            {
                Id = rs.Fields.Item("OcrCode").Value.ToString(),
                Name = rs.Fields.Item("OcrName").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, CostCenter obj) => table;
    }
}
