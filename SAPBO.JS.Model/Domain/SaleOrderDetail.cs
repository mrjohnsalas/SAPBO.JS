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
    public class SaleOrderDetail
    {
        [Key]
        [Display(Name = "Detalle orden venta Id")]
        public int Id { get; set; }

        [Display(Name = "Orden venta Id")]
        public int SaleOrderId { get; set; }

        [Display(Name = "Orden venta")]
        public SaleOrder SaleOrder { get; set; }

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

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Quantity { get; set; }

        [Display(Name = "Cant. Pendiente")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal PendingQuantity { get; set; }

        [Display(Name = "Precio base")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal BasePrice { get; set; }

        [Display(Name = "Total base")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal BaseTotal { get; set; }

        [Display(Name = "% Descuento cliente")]
        [DisplayFormat(DataFormatString = AppFormats.FieldPercentage, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal XjeCustomerDiscount { get; set; }

        [Display(Name = "Total descuento cliente")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalCustomerDiscount { get; set; }

        [Display(Name = "Precio cliente")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal CustomerPrice { get; set; }

        [Display(Name = "Total cliente")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal CustomerTotal { get; set; }

        [Display(Name = "% Descuento cantidad")]
        [DisplayFormat(DataFormatString = AppFormats.FieldPercentage, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal XjeQuantityDiscount { get; set; }

        [Display(Name = "Total descuento cantidad")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalQuantityDiscount { get; set; }

        [Display(Name = "Precio final")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal FinalPrice { get; set; }

        [Display(Name = "Total final")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal FinalTotal { get; set; }

        [Display(Name = "Total descuento")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDiscount => TotalCustomerDiscount + TotalQuantityDiscount;

        //[Display(Name = "Total")]
        //[DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        //[DataType(DataType.Currency)]
        //public decimal Total { get; set; }

        [Display(Name = "% Comisión")]
        [DisplayFormat(DataFormatString = AppFormats.FieldPercentage, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal XjeComision { get; set; }

        [Display(Name = "Comisión total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalComision { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        public ICollection<DeliveryDetail> DeliveryDetails { get; set; }
    }
}
