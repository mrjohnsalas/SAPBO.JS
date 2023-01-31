using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class SaleQuotationDetailMapper : ISapB1AutoMapper<SaleQuotationDetail>
    {
        public SaleQuotationDetail Mapper(IRecordset rs)
        {
            return new SaleQuotationDetail
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                SaleQuotationId = int.Parse(rs.Fields.Item("U_CL_CODCOT").Value.ToString()),
                ProductFormulaId = int.Parse(rs.Fields.Item("U_CL_CODFOR").Value.ToString()),
                ProductMaterialTypeId = int.Parse(rs.Fields.Item("U_CL_CODTPA").Value.ToString()),
                ProductGrammageId = int.Parse(rs.Fields.Item("U_CL_CODGRA").Value.ToString()),
                ProductInkLevelId = int.Parse(rs.Fields.Item("U_CL_CODNTI").Value.ToString()),
                ProductFormatId = int.Parse(rs.Fields.Item("U_CL_CODFMT").Value.ToString()),
                Quantity = decimal.Parse(rs.Fields.Item("U_CL_CANTID").Value.ToString()),
                Ancho = decimal.Parse(rs.Fields.Item("U_CL_ANCHO").Value.ToString()),
                Alto = decimal.Parse(rs.Fields.Item("U_CL_ALTO").Value.ToString()),
                Panol = decimal.Parse(rs.Fields.Item("U_CL_PANOL").Value.ToString()),
                NroCopias = int.Parse(rs.Fields.Item("U_CL_NROCOP").Value.ToString()),
                TotalWeight = decimal.Parse(rs.Fields.Item("U_CL_PESTOT").Value.ToString()),
                TotalPrice = decimal.Parse(rs.Fields.Item("U_CL_PRETOT").Value.ToString()),
                Remark = rs.Fields.Item("U_CL_REMARK").Value.ToString(),
                PriceTypeId = int.Parse(rs.Fields.Item("U_CL_TIPPRE").Value.ToString()),
                ProductId = rs.Fields.Item("U_CL_CODART").Value.ToString(),
                StatusId = int.Parse(rs.Fields.Item("U_CL_STATUS").Value.ToString()),
                RejectReason = rs.Fields.Item("U_CL_MOTREC").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, SaleQuotationDetail obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_CODCOT").Value = obj.SaleQuotationId.ToString();
            table.UserFields.Fields.Item("U_CL_CODFOR").Value = obj.ProductFormulaId.ToString();
            table.UserFields.Fields.Item("U_CL_CODTPA").Value = obj.ProductMaterialTypeId.ToString();
            table.UserFields.Fields.Item("U_CL_CODGRA").Value = obj.ProductGrammageId.ToString();
            table.UserFields.Fields.Item("U_CL_CODNTI").Value = obj.ProductInkLevelId.ToString();
            table.UserFields.Fields.Item("U_CL_CODFMT").Value = obj.ProductFormatId.ToString();
            table.UserFields.Fields.Item("U_CL_CANTID").Value = (double)obj.Quantity;
            table.UserFields.Fields.Item("U_CL_ANCHO").Value = (double)obj.Ancho;
            table.UserFields.Fields.Item("U_CL_ALTO").Value = (double)obj.Alto;
            table.UserFields.Fields.Item("U_CL_PANOL").Value = (double)obj.Panol;
            table.UserFields.Fields.Item("U_CL_NROCOP").Value = obj.NroCopias;
            table.UserFields.Fields.Item("U_CL_PESTOT").Value = (double)obj.TotalWeight;
            table.UserFields.Fields.Item("U_CL_PRETOT").Value = (double)obj.TotalPrice;
            table.UserFields.Fields.Item("U_CL_REMARK").Value = obj.Remark ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_TIPPRE").Value = obj.PriceTypeId;
            table.UserFields.Fields.Item("U_CL_CODART").Value = obj.ProductId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_STATUS").Value = obj.StatusId;
            table.UserFields.Fields.Item("U_CL_MOTREC").Value = obj.RejectReason ?? string.Empty;

            return table;
        }
    }
}
