using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductFormatMapper : ISapB1AutoMapper<ProductFormat>
    {
        public ProductFormat Mapper(IRecordset rs)
        {
            return new ProductFormat
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("U_CL_NAME").Value.ToString(),
                UnitOfMeasurementId = rs.Fields.Item("U_CL_CODUND").Value.ToString(),
                Ancho = decimal.Parse(rs.Fields.Item("U_CL_ANCHO").Value.ToString()),
                Largo = decimal.Parse(rs.Fields.Item("U_CL_LARGO").Value.ToString()),
                Panol = decimal.Parse(rs.Fields.Item("U_CL_PANOL").Value.ToString()),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STATUS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductFormat obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_NAME").Value = obj.Name ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CODUND").Value = obj.UnitOfMeasurementId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_ANCHO").Value = (double)obj.Ancho;
            table.UserFields.Fields.Item("U_CL_LARGO").Value = (double)obj.Largo;
            table.UserFields.Fields.Item("U_CL_PANOL").Value = (double)obj.Panol;
            table.UserFields.Fields.Item("U_CL_STATUS").Value = obj.StatusId;

            return table;
        }
    }
}
