using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class QuotationAccessoryMapper : ISapB1AutoMapper<QuotationAccessory>
    {
        public QuotationAccessory Mapper(IRecordset rs)
        {
            return new QuotationAccessory
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("U_CL_NAME").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESCRI").Value.ToString(),
                ValueXje = decimal.Parse(rs.Fields.Item("U_CL_PORVAL").Value.ToString()),
                Index = int.Parse(rs.Fields.Item("U_CL_INDEX").Value.ToString()),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STATUS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, QuotationAccessory obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_NAME").Value = obj.Name ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_DESCRI").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_PORVAL").Value = (double)obj.ValueXje;
            table.UserFields.Fields.Item("U_CL_INDEX").Value = obj.Index;
            table.UserFields.Fields.Item("U_CL_STATUS").Value = obj.StatusId;

            return table;
        }
    }
}
