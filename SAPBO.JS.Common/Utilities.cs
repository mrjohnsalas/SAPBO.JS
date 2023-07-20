using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Common
{
    public static class Utilities
    {
        public static int GetStatusIdByText(string text)
        {
            if (text.Equals("N") || text.Equals("A"))
            {
                return (int)Enums.StatusType.Activo;
            }
            if (text.Equals("Y") || text.Equals("I"))
            {
                return (int)Enums.StatusType.Anulado;
            }
            return (int)Enums.StatusType.Anulado;
        }

        public static string GetTextByStatusId(int statusId, bool type2)
        {
            if (type2)
            {
                return statusId.Equals(1) ? "N" : "Y";
            }
            else
            {
                return statusId.Equals(1) ? "A" : "I";
            }
        }

        public static DateTime ConcatDateTime(DateTime date, DateTime time)
        {
            date = date.AddHours(time.Hour);
            date = date.AddMinutes(time.Minute);
            date = date.AddSeconds(time.Second);
            return date;
        }

        public static DateTime ConcatDateTime(dynamic date, dynamic time)
        {
            var d = (DateTime)date;
            var t = ((int)time).ToString("00:00");
            d = d.AddHours(int.Parse(t.Substring(0, 2)));
            d = d.AddMinutes(int.Parse(t.Substring(3, 2)));
            return d;
        }

        public static string ValueToClock(dynamic value)
        {
            return $"{decimal.Round(decimal.Parse(value.ToString()), AppFormats.Total):00.00}".Replace(".", ":");
        }

        public static int? ClockToValue(dynamic value)
        {
            string newValue = value.ToString().Replace(":", "");
            return string.IsNullOrEmpty(newValue) ? null : int.Parse(newValue);
        }

        public static DateTime? DateValueToDateOrNull(dynamic dateValue, dynamic timeValue = null)
        {
            if (dateValue == null) return null;

            var date = (DateTime)dateValue;
            if (date.ToString(AppFormats.Date).Equals(AppMessages.SapDateMinValue))
                return null;

            return timeValue == null
                    ? date
                    : ConcatDateTime(date, DateTime.Parse(((int)timeValue).ToString(AppFormats.TimeFormat)));
        }

        public static int? IntValueToIntOrNull(dynamic value)
        {
            return value == null ? null : (int?)value;
        }

        public static Enums.ActivityType StringToActivityType(dynamic value)
        {
            switch (value)
            {
                case "C":
                    return Enums.ActivityType.PhoneCall;
                case "M":
                    return Enums.ActivityType.Meeting;
                case "T":
                    return Enums.ActivityType.Task;
                case "E":
                    return Enums.ActivityType.Note;
                case "P":
                    return Enums.ActivityType.Campaign;
                case "N":
                    return Enums.ActivityType.Other;
                default:
                    return Enums.ActivityType.Other;
            }
        }

        public static Enums.ActivityDurationType StringToActivityDurationType(dynamic value)
        {
            switch (value)
            {
                case "S":
                    return Enums.ActivityDurationType.Seconds;
                case "M":
                    return Enums.ActivityDurationType.Minutes;
                case "H":
                    return Enums.ActivityDurationType.Hours;
                case "D":
                    return Enums.ActivityDurationType.Days;
                default:
                    return Enums.ActivityDurationType.Minutes;
            }
        }

        public static Enums.SaleOpportunityType StringToSaleOpportunityType(dynamic value)
        {
            switch (value)
            {
                case "R":
                    return Enums.SaleOpportunityType.Sale;
                case "P":
                    return Enums.SaleOpportunityType.Purchase;
                default:
                    return Enums.SaleOpportunityType.Sale;
            }
        }

        public static Enums.SaleOpportunityStatus StringToSaleOpportunityStatus(dynamic value)
        {
            switch (value)
            {
                case "O":
                    return Enums.SaleOpportunityStatus.Open;
                case "L":
                    return Enums.SaleOpportunityStatus.Lost;
                case "W":
                    return Enums.SaleOpportunityStatus.Won;
                default:
                    return Enums.SaleOpportunityStatus.Open;
            }
        }

        public static Enums.PurchaseOrderAuthorizationStatus StringToPurchaseOrderAuthorizationStatus(dynamic value)
        {
            switch (value)
            {
                case "P":
                    return Enums.PurchaseOrderAuthorizationStatus.Pendiente;
                case "A":
                    return Enums.PurchaseOrderAuthorizationStatus.Autorizado;
                case "R":
                    return Enums.PurchaseOrderAuthorizationStatus.Rechazado;
                default:
                    return Enums.PurchaseOrderAuthorizationStatus.Pendiente;
            }
        }

        public static string PurchaseOrderAuthorizationStatusToString(Enums.PurchaseOrderAuthorizationStatus value)
        {
            switch (value)
            {
                case Enums.PurchaseOrderAuthorizationStatus.Pendiente:
                    return "P";
                case Enums.PurchaseOrderAuthorizationStatus.Autorizado:
                    return "A";
                case Enums.PurchaseOrderAuthorizationStatus.Rechazado:
                    return "R";
                default:
                    return "";
            }
        }

        public static Enums.StatusType StringToPurchaseOrderStatus(string value)
        {
            switch (value)
            {
                case "D":
                    return Enums.StatusType.Anulado;
                case "C":
                    return Enums.StatusType.Cerrado;
                case "O":
                    return Enums.StatusType.Abierto;
                default:
                    return Enums.StatusType.Abierto;
            }
        }

        public static Enums.StatusType StringToBillStatus(string value)
        {
            switch (value)
            {
                case "D":
                    return Enums.StatusType.Anulado;
                case "C":
                    return Enums.StatusType.Pagado;
                case "O":
                    return Enums.StatusType.Pendiente;
                default:
                    return Enums.StatusType.Pendiente;
            }
        }

        public static Enums.SaleOrderType StringToSaleOrderType(string value)
        {
            switch (value)
            {
                case "LIN":
                    return Enums.SaleOrderType.Linea;
                case "IMP":
                    return Enums.SaleOrderType.Impreso;
                case "FLE":
                    return Enums.SaleOrderType.Flexografia;
                case "-":
                    return Enums.SaleOrderType.Otros;
                default:
                    return Enums.SaleOrderType.Otros;
            }
        }

        public static Enums.BillType StringToBillType(string value)
        {
            switch (value)
            {
                case "FC":
                    return Enums.BillType.FacturaDeudores;
                case "FR":
                    return Enums.BillType.FacturaReserva;
                case "FE":
                    return Enums.BillType.FacturaExportacion;
                case "BO":
                    return Enums.BillType.Boleta;
                case "FA":
                    return Enums.BillType.Anticipo;
                case "LT":
                    return Enums.BillType.Letra;
                case "ND":
                    return Enums.BillType.NotaDebito;
                default:
                    return Enums.BillType.FacturaDeudores;
            }
        }

        public static string BillFileTypeToContentType(Enums.BillFileType billFileType)
        {
            switch (billFileType)
            {
                case Enums.BillFileType.PDF:
                    return AppDefaultValues.PDFApplication;
                case Enums.BillFileType.XML:
                    return AppDefaultValues.XMLApplication;
                case Enums.BillFileType.Zip:
                    return AppDefaultValues.ZipApplication;
                default:
                    return "";
            }
        }

        public static Enums.SaleOpportunityInterestLevel StringToSaleOpportunityInterestLevel(dynamic value)
        {
            switch (value)
            {
                case "1":
                    return Enums.SaleOpportunityInterestLevel.Low;
                case "2":
                    return Enums.SaleOpportunityInterestLevel.Half;
                case "3":
                    return Enums.SaleOpportunityInterestLevel.High;
                default:
                    return Enums.SaleOpportunityInterestLevel.Low;
            }
        }

        public static decimal GetMaxCustomerQuantityValue(decimal maxCustomerQuantity, decimal multipleQuantity)
        {
            return maxCustomerQuantity - (maxCustomerQuantity % multipleQuantity);
        }

        public static string GetCurrentDirectory(string container, string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), container, fileName);
        }

        public static string GetWebPath(string container, string fileName)
        {
            return Path.Combine(GetCurrentDirectory("wwwroot", ""), container, fileName);
        }

        public static int GetStatusTypeIdFromSaleOrderAuthorizationStatus(dynamic value)
        {
            switch (value)
            {
                case "A":
                    return (int)Enums.StatusType.Autorizado;
                case "P":
                    return (int)Enums.StatusType.Pendiente;
                case "R":
                    return (int)Enums.StatusType.Rechazado;
                default:
                    return (int)Enums.StatusType.Pendiente;
            }
        }

        public static string GetSaleOrderAuthorizationStatusFromStatusType(Enums.StatusType value)
        {
            switch (value)
            {
                case Enums.StatusType.Autorizado:
                    return "A";
                case Enums.StatusType.Pendiente:
                    return "P";
                case Enums.StatusType.Rechazado:
                    return "R";
                default:
                    return "P";
            }
        }
    }
}
