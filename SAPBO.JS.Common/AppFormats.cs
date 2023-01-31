using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Common
{
    public class AppFormats
    {
        public const string MomentDate = "YYYY-MM-DD";
        public const string Date = "yyyy-MM-dd";
        public const string FieldDate = "{0:" + Date + "}";

        public const string Time = "HH:mm";
        public const string FieldTime = "{0:" + Time + "}";
        public const string TimeFormat = "00:00";

        public const string MomentFullDate = "YYYY-MM-DD HH:mm";
        public const string FullDate = "yyyy-MM-dd HH:mm";
        public const string FieldFullDate = "{0:" + FullDate + "}";

        public const int Total = 2;
        public const string FieldTotal = "{0:N2}";
        public const string StringTotal = "0.00";

        public const int Quantity = 3;
        public const string FieldQuantity = "{0:N3}";
        public const string StringQuantity = "0.000";

        public const int Percentage = 4;
        public const string FieldPercentage = "{0:N4}";
        public const string StringPercentage = "0.0000";

        public const int UnitPrice = 6;
        public const string FieldUnitPrice = "{0:N6}";
        public const string StringUnitPrice = "0.000000";
    }
}
