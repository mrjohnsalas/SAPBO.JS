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
    public class Product
    {
        [Key]
        [Display(Name = "Articulo Id")]
        public string Id { get; set; }

        [Display(Name = "Articulo Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(200, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "U.Med")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(10, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 2)]
        public string MedidaId { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Remark { get; set; }

        [Display(Name = "Grupo Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string ProductGroupId { get; set; }

        [Display(Name = "Grupo")]
        public ProductGroup ProductGroup { get; set; }

        [Display(Name = "Super grupo Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string ProductSuperGroupId { get; set; }

        [Display(Name = "Super grupo")]
        public ProductSuperGroup ProductSuperGroup { get; set; }

        [Display(Name = "Clase Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string ProductClassId { get; set; }

        [Display(Name = "Clase")]
        public ProductClass ProductClass { get; set; }

        [Display(Name = "Almacén por defecto Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(15, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 13)]
        public string DefaultWarehouseId { get; set; }

        [Display(Name = "Almacén por defecto")]
        public Warehouse DefaultWarehouse { get; set; }

        [Display(Name = "Almacén ventas Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(15, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 13)]
        public string SalesWarehouseId { get; set; }

        [Display(Name = "Almacén ventas")]
        public Warehouse SalesWarehouse { get; set; }

        [Display(Name = "Und. X paquete")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal UnidadesxPaquete { get; set; }

        [Display(Name = "Ancho paquete")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal AnchoPaquete { get; set; }

        [Display(Name = "Largo paquete")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal LargoPaquete { get; set; }

        [Display(Name = "Alto paquete")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal AltoPaquete { get; set; }

        [Display(Name = "Pqt. X caja")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal PaquetesxCaja { get; set; }

        [Display(Name = "Ancho caja")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal AnchoCaja { get; set; }

        [Display(Name = "Largo caja")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal LargoCaja { get; set; }

        [Display(Name = "Alto caja")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal AltoCaja { get; set; }

        [Display(Name = "Cant. Min. Venta")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal CantidadMinimaVenta { get; set; }

        [Display(Name = "Múltiplo de cantidad")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal MultiploCantidad { get; set; }

        [Display(Name = "Precio unitario base $")]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal BaseUnitPrice { get; set; }

        public ProductPrice ProductPrice { get; set; }

        [Display(Name = "Stock")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Stock { get; set; }

        [Display(Name = "Comprometido")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal CommittedStock { get; set; }

        [Display(Name = "Disponible")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal AvailableStock { get; set; }

        [Display(Name = "Max Customer Quantity")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal MaxCustomerQuantity { get; set; }

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public ICollection<SaleOrderDetail> SaleOrderDetails { get; set; }

        public ICollection<DeliveryDetail> DeliveryDetails { get; set; }

        public ICollection<ProductQuantityDiscount> ProductQuantityDiscounts { get; set; }
    }
}
