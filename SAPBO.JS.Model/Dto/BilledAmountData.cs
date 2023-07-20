using SAPBO.JS.Common;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Dto
{
    public class BilledAmountData
    {
        [Display(Name = "Año")]
        public int Year { get; set; }

        [Display(Name = "Mes")]
        public int Month { get; set; }

        [Display(Name = "Super grupo Id")]
        public string ProductSuperGroupId { get; set; }

        [Display(Name = "Super grupo")]
        public string ProductSuperGroupName { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Quantity { get; set; }

        [Display(Name = "Total Dolar")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDolar { get; set; }
    }
}
