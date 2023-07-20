using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Common
{
    public class AppDefaultValues
    {
        public const string CurrencyIdDolar = "USD";
        public const string CurrencyIdSol = "SOL";
        public const string CurrencySymbolDolar = "$";
        public const string CurrencySymbolSol = "S/";
        public const int SaleEmployeeId = 1;
        public const decimal RateForSolCurrency = 1;
        public const string TaxCode = "I18";
        public const decimal TaxValue = 1.18M;
        public const int FacturaContadoId = 6;
        public const int BoletaContadoId = 7;
        public const int DefaultDaysForSaleQuotation = 7;
        public const string AttachmentPathServer = @"\\Sapb1\b1_shr\GRAFIPAPEL\B1DOC\Anx\";

        public const string MimType = "";
        public const int PDFExtension = 1;
        public const string PDFApplication = "application/pdf";
        public const string XMLApplication = "application/xml";
        public const string ZipApplication = "application/zip";

        public const int MaxFileWeightInMb = 5;
    }
}
