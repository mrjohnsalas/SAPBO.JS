using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class BusinessPartnerDriverMapper : ISapB1AutoMapper<BusinessPartnerDriver>
    {
        public BusinessPartnerDriver Mapper(IRecordset rs)
        {
            return new BusinessPartnerDriver
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                FirstName = rs.Fields.Item("U_CL_NOMBRE").Value.ToString(),
                LastName = rs.Fields.Item("U_CL_APELLI").Value.ToString(),
                LicenseId = rs.Fields.Item("U_BPP_CHLI").Value.ToString(),
                Phone = rs.Fields.Item("U_CL_CELULA").Value.ToString(),
                Email = rs.Fields.Item("U_CL_EMAIL").Value.ToString(),
                BusinessPartnerId = rs.Fields.Item("U_CL_CODPRO").Value.ToString(),
                LicenseExpirationDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_VS_VCTO_CHLI").Value),
                StatusId = Utilities.GetStatusIdByText(rs.Fields.Item("U_VS_FROZEN").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, BusinessPartnerDriver obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_NOMBRE").Value = obj.FirstName ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_APELLI").Value = obj.LastName ?? string.Empty;
            table.UserFields.Fields.Item("U_BPP_CHLI").Value = obj.LicenseId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CELULA").Value = obj.Phone ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_EMAIL").Value = obj.Email ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CODPRO").Value = obj.BusinessPartnerId ?? string.Empty;

            if (obj.LicenseExpirationDate.HasValue)
                table.UserFields.Fields.Item("U_VS_VCTO_CHLI").Value = obj.LicenseExpirationDate.Value.ToString(AppFormats.Date);

            table.UserFields.Fields.Item("U_VS_FROZEN").Value = Utilities.GetTextByStatusId(obj.StatusId, true);

            return table;
        }
    }
}
