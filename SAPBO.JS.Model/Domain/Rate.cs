using SAPBO.JS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAPBO.JS.Model.Domain
{
    public class Rate
    {
        [Key]
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Display(Name = "Moneda Id")]
        public string CurrencyId { get; set; }

        [Display(Name = "Tipo cambio")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Value { get; set; }

        public string DisplayRate => $"{Value} ({Date.ToString(AppFormats.Date)})";
    }
}
