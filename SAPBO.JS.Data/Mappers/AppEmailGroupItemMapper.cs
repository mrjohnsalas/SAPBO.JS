using SAPBO.JS.Common;
using SAPBO.JS.Model.Dto;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class AppEmailGroupItemMapper : ISapB1AutoMapper<AppEmailGroupItem>
    {
        public AppEmailGroupItem Mapper(IRecordset rs)
        {
            return new AppEmailGroupItem
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                AppEmailGroupId = rs.Fields.Item("U_CL_GROEMA").Value.ToString(),
                EmailToType = (Enums.EmailToType)int.Parse(rs.Fields.Item("U_CL_TOTYPE_ID").Value.ToString()),
                EmailAddress = rs.Fields.Item("U_CL_EMAIL").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, AppEmailGroupItem obj) => table;
    }
}
