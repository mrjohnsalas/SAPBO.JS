using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class CurrencyMapper : ISapB1AutoMapper<Currency>
    {
        public Currency Mapper(IRecordset rs)
        {
            return new Currency
            {
                Id = rs.Fields.Item("CurrCode").Value.ToString(),
                Name = rs.Fields.Item("CurrName").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, Currency obj) => table;
    }
}
