using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class CountryMapper : ISapB1AutoMapper<Model.Domain.Country>
    {
        public Model.Domain.Country Mapper(IRecordset rs)
        {
            return new Model.Domain.Country
            {
                Id = rs.Fields.Item("Code").Value.ToString(),
                Name = rs.Fields.Item("Name").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, Model.Domain.Country obj) => table;
    }
}
