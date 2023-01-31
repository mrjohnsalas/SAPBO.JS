using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class UnitOfMeasurementMapper : ISapB1AutoMapper<Model.Domain.UnitOfMeasurement>
    {
        public Model.Domain.UnitOfMeasurement Mapper(IRecordset rs)
        {
            return new Model.Domain.UnitOfMeasurement
            {
                Id = rs.Fields.Item("Code").Value.ToString(),
                Name = rs.Fields.Item("Name").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, Model.Domain.UnitOfMeasurement obj) => table;
    }
}
