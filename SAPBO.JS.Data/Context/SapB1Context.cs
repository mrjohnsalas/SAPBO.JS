using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Context
{
    public class SapB1Context
    {
        public Company Company { get; set; }

        public string CompanyConnected { get; set; }

        public SapB1Context(IConfiguration configuration)
        {
            Company = new Company
            {
                Server = configuration["SapServer"],
                LicenseServer = configuration["SapLicenseServer"],
                // CompanyDB = configuration["SapProductionDb"],
                CompanyDB = configuration["SapTestDb"],

                DbServerType = BoDataServerTypes.dst_MSSQL2019,
                UseTrusted = false,
                language = BoSuppLangs.ln_English,

                UserName = configuration["SapUserId"],
                Password = configuration["SapPassword"],

                DbUserName = configuration["DbUserId"],
                DbPassword = configuration["DbPassword"]
            };
        }

        public bool GetConnectionStatus() => Company != null && Company.Connected;

        public DateTime GetServerDate() => Company.GetCompanyDate();

        public void Disconnect()
        {
            if (Company.Connected)
            {
                Company.Disconnect();
                Marshal.FinalReleaseComObject(Company);
            }
            Company = null;
            CompanyConnected = string.Empty;
        }

        public void Connect()
        {
            if (GetConnectionStatus()) return;
            try
            {
                if (Company.Connect() != 0)
                    throw new Exception($"Error connecting to SAP. Error code: {Company.GetLastErrorCode()}, Error message: {Company.GetLastErrorDescription()}");
                CompanyConnected = Company.CompanyName;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
