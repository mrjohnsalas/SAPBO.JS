using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class StateMapper : ISapB1AutoMapper<Model.Domain.State>
    {
        public Model.Domain.State Mapper(IRecordset rs)
        {
            return new Model.Domain.State
            {
                Id = rs.Fields.Item("Code").Value.ToString(),
                Name = rs.Fields.Item("Name").Value.ToString(),
                CountryId = rs.Fields.Item("Country").Value.ToString(),
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, Model.Domain.State obj) => table;
    }
}
