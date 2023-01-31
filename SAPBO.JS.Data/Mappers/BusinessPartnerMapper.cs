using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class BusinessPartnerMapper : ISapB1AutoMapper<BusinessPartner>
    {
        public BusinessPartner Mapper(IRecordset rs)
        {
            return new BusinessPartner
            {
                Id = rs.Fields.Item("CardCode").Value.ToString(),
                Name = rs.Fields.Item("CardName").Value.ToString(),
                Ruc = rs.Fields.Item("LicTradNum").Value.ToString(),
                Email = rs.Fields.Item("E_Mail").Value.ToString(),
                Phone1 = rs.Fields.Item("Phone1").Value.ToString(),
                Phone2 = rs.Fields.Item("Phone2").Value.ToString(),
                Cellular = rs.Fields.Item("Cellular").Value.ToString(),
                WebSite = rs.Fields.Item("IntrntSite").Value.ToString(),
                CreditLine = decimal.Parse(rs.Fields.Item("CreditLine").Value.ToString()),
                CreditLineAvailable = decimal.Parse(rs.Fields.Item("CreditLineAvailable").Value.ToString()),
                DefaultCurrencyId = rs.Fields.Item("U_CL_MONCLI").Value.ToString(),
                WebAppAuthorizationValue = rs.Fields.Item("U_CL_WAPAUT").Value.ToString(),
                SaleEmployeeId = int.Parse(rs.Fields.Item("SlpCode").Value.ToString()),
                IsTemp = rs.Fields.Item("ISTEMP").Value.ToString().Equals("Y")
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, BusinessPartner obj)
        {
            table.Code = obj.Id;
            table.Name = obj.Id;
            table.UserFields.Fields.Item("U_CL_RAZSOC").Value = obj.Name ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_RUC").Value = obj.Ruc ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CODVEN").Value = obj.SaleEmployeeId;
            table.UserFields.Fields.Item("U_CL_EMAIL").Value = obj.Email ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_PHONE1").Value = obj.Phone1 ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_PHONE2").Value = obj.Phone2 ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CELLU").Value = obj.Cellular ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_WEBSIT").Value = obj.WebSite ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_STATUS").Value = 1;

            if (obj.CreatedAt != null)
                table.UserFields.Fields.Item("U_CL_CREDAT").Value = obj.CreatedAt?.ToString(AppFormats.Date);
            table.UserFields.Fields.Item("U_CL_CREABY").Value = obj.CreatedBy ?? string.Empty;

            if (obj.UpdatedAt != null)
                table.UserFields.Fields.Item("U_CL_UPDDAT").Value = obj.UpdatedAt?.ToString(AppFormats.Date);
            table.UserFields.Fields.Item("U_CL_UPDABY").Value = obj.UpdatedBy ?? string.Empty;

            if (obj.DeletedAt != null)
                table.UserFields.Fields.Item("U_CL_DELDAT").Value = obj.DeletedAt?.ToString(AppFormats.Date);
            table.UserFields.Fields.Item("U_CL_DELEBY").Value = obj.DeletedBy ?? string.Empty;

            return table;
        }
    }
}
