using SAPBO.JS.Common;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Dto
{
    public class TopBilledBusinessPartner
    {

        [Display(Name = "Cliente Id")]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Cliente")]
        public string BusinessPartner { get; set; }

        [Display(Name = "Total Dolar")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDolar { get; set; }

        [Display(Name = "Total Dolar Linea")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDolarLinea { get; set; }

        [Display(Name = "Total Dolar Impreso")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDolarImpreso { get; set; }

        [Display(Name = "Total Dolar Flexografia")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDolarFlexografia { get; set; }

        [Display(Name = "Total Dolar Compra y Venta")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDolarCompraVenta { get; set; }

        [Display(Name = "Total Dolar Exportacion")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDolarExportacion { get; set; }

        [Display(Name = "Total Dolar Otros")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDolarOtros { get; set; }
    }
}
