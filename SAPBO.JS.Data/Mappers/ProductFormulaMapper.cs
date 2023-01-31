using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductFormulaMapper : ISapB1AutoMapper<ProductFormula>
    {
        public ProductFormula Mapper(IRecordset rs)
        {
            return new ProductFormula
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Name = rs.Fields.Item("U_CL_NAME").Value.ToString(),
                Description = rs.Fields.Item("U_CL_DESCRI").Value.ToString(),
                UnitOfMeasurementId = rs.Fields.Item("U_CL_UNDMED").Value.ToString(),
                ProductSuperGroupId = rs.Fields.Item("U_CL_SUPGRP").Value.ToString(),
                NroCopiasMinimo = decimal.Parse(rs.Fields.Item("U_CL_NRCOMI").Value.ToString()),
                NroCopiasMaximo = decimal.Parse(rs.Fields.Item("U_CL_NRCOMA").Value.ToString()),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STATUS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductFormula obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_NAME").Value = obj.Name ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_DESCRI").Value = obj.Description ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_UNDMED").Value = obj.UnitOfMeasurementId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_SUPGRP").Value = obj.ProductSuperGroupId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_NRCOMI").Value = (double)obj.NroCopiasMinimo;
            table.UserFields.Fields.Item("U_CL_NRCOMA").Value = (double)obj.NroCopiasMaximo;
            table.UserFields.Fields.Item("U_CL_STATUS").Value = obj.StatusId;

            return table;
        }
    }
}
