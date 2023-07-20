using SAPBO.JS.Model.Dto;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class TopBilledBusinessPartnerMapper : ISapB1AutoMapper<TopBilledBusinessPartner>
    {
        public TopBilledBusinessPartner Mapper(IRecordset rs)
        {
            return new TopBilledBusinessPartner
            {
                BusinessPartnerId = rs.Fields.Item("COD_SOCIO_NEGOCIO").Value.ToString(),
                BusinessPartner = rs.Fields.Item("DESC_SOCIO_NEGOCIO").Value.ToString(),
                TotalDolar = decimal.Parse(rs.Fields.Item("TOTAL_DOLAR").Value.ToString()),
                TotalDolarLinea = decimal.Parse(rs.Fields.Item("TOTAL_DOLAR_LINEA").Value.ToString()),
                TotalDolarImpreso = decimal.Parse(rs.Fields.Item("TOTAL_DOLAR_IMPRESO").Value.ToString()),
                TotalDolarFlexografia = decimal.Parse(rs.Fields.Item("TOTAL_DOLAR_FLEXO").Value.ToString()),
                TotalDolarCompraVenta = decimal.Parse(rs.Fields.Item("TOTAL_DOLAR_COMPRA_VENTA").Value.ToString()),
                TotalDolarExportacion = decimal.Parse(rs.Fields.Item("TOTAL_DOLAR_EXPORTACION").Value.ToString()),
                TotalDolarOtros = decimal.Parse(rs.Fields.Item("TOTAL_DOLAR_OTROS").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, TopBilledBusinessPartner obj) => table;
    }
}
