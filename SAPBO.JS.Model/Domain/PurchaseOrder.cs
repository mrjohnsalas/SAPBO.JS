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
    public class PurchaseOrder
    {
        [Key]
        [Display(Name = "Orden compra Id")]
        public int Id { get; set; }

        [Display(Name = "Proveedor Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Proveedor")]
        public BusinessPartner BusinessPartner { get; set; }

        [Display(Name = "Fecha de contabilización")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime PostingDate { get; set; }

        [Display(Name = "Fecha de entrega")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Fecha del documento")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime DocumentDate { get; set; }

        [Display(Name = "Moneda Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(3, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public Currency Currency { get; set; }

        [Display(Name = "Contacto Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ContactId { get; set; }

        [Display(Name = "Contacto")]
        public BusinessPartnerContact Contact { get; set; }

        [Display(Name = "Condición de pago Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int PaymentId { get; set; }

        [Display(Name = "Condición de pago")]
        public BusinessPartnerPayment Payment { get; set; }

        [Display(Name = "Tipo cambio")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Rate { get; set; }

        [Display(Name = "Comprador Id")]
        public int PurchaseEmployeeId { get; set; }

        [Display(Name = "Comprador")]
        public Employee PurchaseEmployee { get; set; }

        [Display(Name = "Dirección destino Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string ShipAddressId { get; set; }

        [Display(Name = "Dirección destino")]
        public string ShipAddress { get; set; }

        [Display(Name = "Dirección factura Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string BillAddressId { get; set; }

        [Display(Name = "Dirección factura")]
        public BusinessPartnerAddress BillAddress { get; set; }

        [Display(Name = "Comentarios")]
        [DataType(DataType.MultilineText)]
        [StringLength(254, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string Remarks { get; set; }

        [Display(Name = "Nro. Referencia")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Coti. Proveedor")]
        [StringLength(25, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string CotizacionNumber { get; set; }

        [Display(Name = "Afecto a detracción?")]
        public bool AfectoDetraccion { get; set; }

        [Display(Name = "Incluye percepción?")]
        public bool IncluyePercepcion { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        public ICollection<PurchaseOrderDetail> Details { get; set; }

        [Display(Name = "User Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string UserId { get; set; }

        //------------------------->>

        [Display(Name = "Total sin descuento")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalSinDescuento => Details?.Sum(x => x.TotalWithoutDiscount) ?? 0;

        [Display(Name = "Descuento")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Descuento => Details?.Sum(x => x.TotalDiscount) ?? 0;

        [Display(Name = "Sub Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal SubTotal { get; set; }

        [Display(Name = "IGV")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Impuesto { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        public ICollection<PurchaseOrderAuthorization> Authorizations { get; set; }
    }
}
