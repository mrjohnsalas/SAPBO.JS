using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class RateMapper : ISapB1AutoMapper<Rate>
    {
        public Rate Mapper(IRecordset rs)
        {
            return new Rate
            {
                Date = Utilities.DateValueToDateOrNull(rs.Fields.Item("RateDate").Value).Value,
                CurrencyId = rs.Fields.Item("Currency").Value.ToString(),
                Value = decimal.Parse(rs.Fields.Item("Rate").Value.ToString()),
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, Rate obj) => table;
    }
}
