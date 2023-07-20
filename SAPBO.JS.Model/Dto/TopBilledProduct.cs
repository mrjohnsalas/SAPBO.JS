using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Dto
{
    public class TopBilledProduct
    {
        [Display(Name = "Cod. Producto")]
        public string ProductId { get; set; }

        [Display(Name = "Producto")]
        public Product Product { get; set; }

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
