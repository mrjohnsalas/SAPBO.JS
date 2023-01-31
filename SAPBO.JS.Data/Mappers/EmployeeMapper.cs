using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class EmployeeMapper : ISapB1AutoMapper<Employee>
    {
        public Employee Mapper(IRecordset rs)
        {
            var mapper = new Employee
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                FirstName = rs.Fields.Item("U_CL_NOMBRE").Value.ToString(),
                LastName = rs.Fields.Item("U_CL_APELLI").Value.ToString(),
                GrafipapelId = rs.Fields.Item("U_CL_CODGRA").Value.ToString(),
                DNI = rs.Fields.Item("U_CL_DNI").Value.ToString(),
                CostHour = decimal.Parse(rs.Fields.Item("U_CL_COSXHH").Value.ToString()),
                JobId = int.Parse(rs.Fields.Item("U_CL_CODPTR").Value.ToString()),
                IsSuper = rs.Fields.Item("U_CL_CHKSUP").Value.ToString().Equals("1"),
                WebApp = rs.Fields.Item("U_CL_CHKWAP").Value.ToString().Equals("1"),
                Phone = rs.Fields.Item("U_CL_PHONE").Value.ToString(),
                BusinessUnitId = rs.Fields.Item("U_CL_UNDNEG").Value.ToString(),
                ProfilePhotoPath = rs.Fields.Item("U_CL_PHOPAT").Value.ToString(),
                Email = rs.Fields.Item("U_CL_EMAIL").Value.ToString(),
                StatusId = Utilities.GetStatusIdByText(rs.Fields.Item("U_CL_ESTADO").Value.ToString()),
            };

            var saleEmployeeId = rs.Fields.Item("U_CL_CODVEN").Value.ToString();
            if (!string.IsNullOrEmpty(saleEmployeeId))
                mapper.SaleEmployeeId = int.Parse(saleEmployeeId);

            var employeeId = rs.Fields.Item("empID").Value.ToString();
            if (!string.IsNullOrEmpty(employeeId))
                mapper.EmployeeId = int.Parse(employeeId);

            return mapper;
        }

        public IUserTable SetValuesToUserTable(IUserTable table, Employee obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_CL_NOMBRE").Value = obj.FirstName ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_APELLI").Value = obj.LastName ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CODGRA").Value = obj.GrafipapelId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_DNI").Value = obj.DNI ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_COSXHH").Value = (double)obj.CostHour;
            table.UserFields.Fields.Item("U_CL_CODPTR").Value = obj.JobId.ToString();
            table.UserFields.Fields.Item("U_CL_CHKSUP").Value = obj.IsSuper ? 1 : 0;
            table.UserFields.Fields.Item("U_CL_CHKWAP").Value = obj.WebApp ? 1 : 0;
            table.UserFields.Fields.Item("U_CL_PHONE").Value = obj.Phone ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_UNDNEG").Value = obj.BusinessUnitId ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_PHOPAT").Value = obj.ProfilePhotoPath ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_EMAIL").Value = obj.Email ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_ESTADO").Value = Utilities.GetTextByStatusId(obj.StatusId, false);

            if (obj.SaleEmployeeId.HasValue)
                table.UserFields.Fields.Item("U_CL_CODVEN").Value = obj.SaleEmployeeId.ToString();
            else
                table.UserFields.Fields.Item("U_CL_CODVEN").SetNullValue();

            return table;
        }
    }
}
