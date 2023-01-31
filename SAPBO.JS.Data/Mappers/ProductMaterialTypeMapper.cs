using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductMaterialTypeMapper : ISapB1AutoMapper<ProductMaterialType>
    {
        public ProductMaterialType Mapper(IRecordset rs)
        {
            return new ProductMaterialType
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("U_CL_NAME").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESCRI").Value.ToString(),
                ShowGramaje = rs.Fields.Item("U_CL_SHWGRA").Value.Equals(1),
                EnableCopias = rs.Fields.Item("U_CL_ACTCOP").Value.Equals(1),
                IsPaper = rs.Fields.Item("U_CL_ESPAPE").Value.Equals(1),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STATUS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductMaterialType obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_NAME").Value = obj.Name ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_DESCRI").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_SHWGRA").Value = obj.ShowGramaje ? 1 : 0;
            table.UserFields.Fields.Item("U_CL_ACTCOP").Value = obj.EnableCopias ? 1 : 0;
            table.UserFields.Fields.Item("U_CL_ESPAPE").Value = obj.IsPaper ? 1 : 0;
            table.UserFields.Fields.Item("U_CL_STATUS").Value = obj.StatusId;

            return table;
        }
    }
}
