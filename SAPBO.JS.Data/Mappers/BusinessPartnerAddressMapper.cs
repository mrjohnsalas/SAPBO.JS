using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class BusinessPartnerAddressMapper : ISapB1AutoMapper<BusinessPartnerAddress>
    {
        public BusinessPartnerAddress Mapper(IRecordset rs)
        {
            return new BusinessPartnerAddress
            {
                Id = rs.Fields.Item("Address").Value.ToString(),
                Street = rs.Fields.Item("Street").Value.ToString(),
                CountryId = rs.Fields.Item("Country").Value.ToString(),
                City = rs.Fields.Item("City").Value.ToString(),
                County = rs.Fields.Item("County").Value.ToString(),
                EsLima = rs.Fields.Item("ES_LIMA").Value.Equals(1),
                AddressType = rs.Fields.Item("AdresType").Value.Equals("B") ? Enums.AddressType.Bill : Enums.AddressType.Ship,
                StateId = rs.Fields.Item("State").Value.ToString(),
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, BusinessPartnerAddress obj) => table;
    }
}
