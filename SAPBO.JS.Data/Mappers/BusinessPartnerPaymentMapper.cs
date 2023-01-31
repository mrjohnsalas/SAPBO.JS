using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class BusinessPartnerPaymentMapper : ISapB1AutoMapper<BusinessPartnerPayment>
    {
        public BusinessPartnerPayment Mapper(IRecordset rs)
        {
            return new BusinessPartnerPayment
            {
                Id = int.Parse(rs.Fields.Item("GroupNum").Value.ToString()),
                Name = rs.Fields.Item("PymntGroup").Value.ToString()
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, BusinessPartnerPayment obj)
        {
            return table;
        }
    }
}
