using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class QuotationIndirectSpendingMapper : ISapB1AutoMapper<QuotationIndirectSpending>
    {
        public QuotationIndirectSpending Mapper(IRecordset rs)
        {
            return new QuotationIndirectSpending
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("U_CL_NAME").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESCRI").Value.ToString(),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STATUS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, QuotationIndirectSpending obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_NAME").Value = obj.Name ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_DESCRI").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_STATUS").Value = obj.StatusId;

            return table;
        }
    }
}
