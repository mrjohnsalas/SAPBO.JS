using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class BusinessPartnerContactMapper : ISapB1AutoMapper<BusinessPartnerContact>
    {
        public BusinessPartnerContact Mapper(IRecordset rs)
        {
            return new BusinessPartnerContact
            {
                Id = int.Parse(rs.Fields.Item("CntctCode").Value.ToString()),
                LineNum = int.Parse(rs.Fields.Item("LineNum").Value.ToString()),
                Name = rs.Fields.Item("Name").Value.ToString(),
                FirstName = rs.Fields.Item("FirstName").Value.ToString(),
                MiddleName = rs.Fields.Item("MiddleName").Value.ToString(),
                LastName = rs.Fields.Item("LastName").Value.ToString(),
                Title = rs.Fields.Item("Title").Value.ToString(),
                Position = rs.Fields.Item("Position").Value.ToString(),
                Address = rs.Fields.Item("Address").Value.ToString(),
                Phone1 = rs.Fields.Item("Tel1").Value.ToString(),
                Phone2 = rs.Fields.Item("Tel2").Value.ToString(),
                MobilePhone = rs.Fields.Item("Cellolar").Value.ToString(),
                Email = rs.Fields.Item("E_MailL").Value.ToString(),
                Profession = rs.Fields.Item("Profession").Value.ToString(),
                BusinessPartnerId = rs.Fields.Item("CardCode").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, BusinessPartnerContact obj) => table;
    }
}
