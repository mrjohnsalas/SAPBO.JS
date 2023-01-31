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
    public class ShoppingCartItem
    {
        [Key]
        [Display(Name = "CartItem Id")]
        public int Id { get; set; }

        [Display(Name = "User Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string UserId { get; set; }

        [Display(Name = "Articulo Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(20, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 13)]
        public string ProductId { get; set; }

        [Display(Name = "Articulo")]
        public Product Product { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal Quantity { get; set; }

        [Display(Name = "Detalle articulo")]
        [DataType(DataType.MultilineText)]
        [StringLength(256000, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string ProductDetail { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Total => Product != null && Product.ProductPrice != null ? decimal.Round(Product.ProductPrice.FinalUnitPrice * Quantity, AppFormats.Total) : 0;

        [Display(Name = "SAP Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal SapTotal => Product != null && Product.ProductPrice != null ? decimal.Round(Product.ProductPrice.SapFinalUnitPrice * Quantity, AppFormats.Total) : 0;
    }
}
