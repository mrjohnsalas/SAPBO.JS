using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Dto;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class BilledAmountDataMapper : ISapB1AutoMapper<BilledAmountData>
    {
        public BilledAmountData Mapper(IRecordset rs)
        {
            return new BilledAmountData
            {
                Year = int.Parse(rs.Fields.Item("ANO_CONTABILIZACION").Value.ToString()),
                Month = int.Parse(rs.Fields.Item("MES_CONTABILIZACION").Value.ToString()),

                ProductSuperGroupId = rs.Fields.Item("COD_SUPER_GRUPO").Value.ToString(),
                ProductSuperGroupName = rs.Fields.Item("DESC_SUPER_GRUPO").Value.ToString(),

                Quantity = decimal.Parse(rs.Fields.Item("CANTIDAD").Value.ToString()),
                TotalDolar = decimal.Parse(rs.Fields.Item("TOTAL_DOLAR").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, BilledAmountData obj) => table;
    }
}
