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
    public class PurchaseOrderDetail
    {
        [Key]
        [Display(Name = "Linea Id")]
        public int Id { get; set; }

        [Display(Name = "Orden compra Id")]
        public int PurchaseOrderId { get; set; }

        [Display(Name = "Orden compra")]
        public PurchaseOrder PurchaseOrder { get; set; }

        [Display(Name = "Articulo Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(20, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 13)]
        public string ProductId { get; set; }

        [Display(Name = "Articulo")]
        public Product Product { get; set; }

        [Display(Name = "Detalle articulo")]
        [StringLength(256000, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string ProductDetail { get; set; }

        [Display(Name = "U. Medida")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(8, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string MedidaId { get; set; }

        [Display(Name = "Almacén Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(8, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string WarehouseId { get; set; }

        [Display(Name = "Almacén")]
        public Warehouse Warehouse { get; set; }

        [Display(Name = "Fecha de entrega")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Quantity { get; set; }

        [Display(Name = "Cant. Pendiente")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal PendingQuantity { get; set; }

        [Display(Name = "Precio sin descuento")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal PriceWithoutDiscount { get; set; }

        [Display(Name = "Total sin descuento")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalWithoutDiscount { get; set; }

        [Display(Name = "% Descuento")]
        [DisplayFormat(DataFormatString = AppFormats.FieldPercentage, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal XjeDiscount { get; set; }

        [Display(Name = "Descuento total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDiscount { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;
    }
}
