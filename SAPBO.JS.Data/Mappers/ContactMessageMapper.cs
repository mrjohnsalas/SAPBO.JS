using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Dto;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ContactMessageMapper : ISapB1AutoMapper<ContactMessage>
    {
        public ContactMessage Mapper(IRecordset rs) => new ContactMessage();

        public IUserTable SetValuesToUserTable(IUserTable table, ContactMessage obj) => table;
    }
}
