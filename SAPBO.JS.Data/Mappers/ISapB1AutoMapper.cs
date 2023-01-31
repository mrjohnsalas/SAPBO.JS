using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public interface ISapB1AutoMapper<T> where T : class
    {
        T Mapper(IRecordset rs);

        IUserTable SetValuesToUserTable(IUserTable table, T obj);
    }
}
