using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductMaterialMapper : ISapB1AutoMapper<ProductMaterial>
    {
        public ProductMaterial Mapper(IRecordset rs)
        {
            return new ProductMaterial
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("U_CL_NAME").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESCRI").Value.ToString(),
                UnitOfMeasurementId = rs.Fields.Item("U_CL_UNDMED").Value.ToString(),
                PrecioLocal = decimal.Parse(rs.Fields.Item("U_CL_PRELOC").Value.ToString()),
                PrecioImportado = decimal.Parse(rs.Fields.Item("U_CL_PREIMP").Value.ToString()),
                PrecioLicitacion = decimal.Parse(rs.Fields.Item("U_CL_PRELIC").Value.ToString()),
                Stock = decimal.Parse(rs.Fields.Item("U_CL_STOCK").Value.ToString()),
                NroCopias = int.Parse(rs.Fields.Item("U_CL_NROCOP").Value.ToString()),
                ProductGrammageId = int.Parse(rs.Fields.Item("U_CL_CODGRA").Value.ToString()),
                ProductionProcessTypeCostId = int.Parse(rs.Fields.Item("U_CL_CODTCO").Value.ToString()),
                ProductMaterialTypeId = int.Parse(rs.Fields.Item("U_CL_CODCAT").Value.ToString()),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STATUS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductMaterial obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_NAME").Value = obj.Name ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_DESCRI").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_UNDMED").Value = obj.UnitOfMeasurementId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_PRELOC").Value = (double)obj.PrecioLocal;
            table.UserFields.Fields.Item("U_CL_PREIMP").Value = (double)obj.PrecioImportado;
            table.UserFields.Fields.Item("U_CL_PRELIC").Value = (double)obj.PrecioLicitacion;
            table.UserFields.Fields.Item("U_CL_STOCK").Value = (double)obj.Stock;
            table.UserFields.Fields.Item("U_CL_NROCOP").Value = obj.NroCopias;
            table.UserFields.Fields.Item("U_CL_CODGRA").Value = obj.ProductGrammageId;
            table.UserFields.Fields.Item("U_CL_CODTCO").Value = obj.ProductionProcessTypeCostId;
            table.UserFields.Fields.Item("U_CL_CODCAT").Value = obj.ProductMaterialTypeId;
            table.UserFields.Fields.Item("U_CL_STATUS").Value = obj.StatusId;

            return table;
        }
    }
}
