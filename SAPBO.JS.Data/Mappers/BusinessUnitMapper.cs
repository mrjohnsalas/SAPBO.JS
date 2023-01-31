using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class BusinessUnitMapper : ISapB1AutoMapper<BusinessUnit>
    {
        public BusinessUnit Mapper(IRecordset rs)
        {
            return new BusinessUnit
            {
                Id = rs.Fields.Item("Code").Value.ToString(),
                Name = rs.Fields.Item("Name").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, BusinessUnit obj) => table;
    }
}
