using SAPBO.JS.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAPBO.JS.Model.Domain
{
    public class ShoppingCart
    {
        [Display(Name = "Id")]
        public int SaleOrderId { get; set; }

        [Display(Name = "Cliente Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Cliente")]
        public BusinessPartner BusinessPartner { get; set; }

        [Display(Name = "Vendedor Id")]
        public int? SaleEmployeeId { get; set; }

        [Display(Name = "Moneda Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(3, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public Currency Currency { get; set; }

        [Display(Name = "Tipo cambio")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Rate { get; set; }

        [Display(Name = "Fecha de contabilización")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime RateDate { get; set; }

        [Display(Name = "Fecha de entrega")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Nro. Referencia")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string ReferenceNumber { get; set; }

        [Display(Name = "O/C del Cliente")]
        [StringLength(25, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string BpReferenceNumber { get; set; }

        [Display(Name = "O/C del Cliente")]
        public IFormFile BpReferenceFile { get; set; }

        [Display(Name = "Comentarios")]
        [DataType(DataType.MultilineText)]
        [StringLength(254, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string Remark { get; set; }

        [Display(Name = "Contacto Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ContactId { get; set; }

        [Display(Name = "Contacto")]
        public BusinessPartnerContact Contact { get; set; }

        [Display(Name = "Dirección destino Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string ShipAddressId { get; set; }

        [Display(Name = "Dirección destino")]
        public BusinessPartnerAddress ShipAddress { get; set; }

        [Display(Name = "Dirección factura Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string BillAddressId { get; set; }

        [Display(Name = "Dirección factura")]
        public BusinessPartnerAddress BillAddress { get; set; }

        [Display(Name = "Agencia Id")]
        [StringLength(15, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 13)]
        public string AgentId { get; set; }

        [Display(Name = "Agencia")]
        public BusinessPartner Agent { get; set; }

        [Display(Name = "Dirección agencia Id")]
        [StringLength(250, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string AgentAddressId { get; set; }

        [Display(Name = "Dirección de agencia")]
        public BusinessPartnerAddress AgentAddress { get; set; }

        [Display(Name = "DNI")]
        [StringLength(8, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 8)]
        public string DNIConsignatario { get; set; }

        [Display(Name = "Nombre")]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string FirstNameConsignatario { get; set; }

        [Display(Name = "Apellidos")]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string LastNameConsignatario { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullNameConsignatario => $"{LastNameConsignatario}, {FirstNameConsignatario}";

        [Display(Name = "Celular")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(12, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string PhoneConsignatario { get; set; }

        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string EmailConsignatario { get; set; }

        [Display(Name = "Condición de pago Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int PaymentId { get; set; }

        [Display(Name = "Condición de pago")]
        public BusinessPartnerPayment Payment { get; set; }

        [Display(Name = "User Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string CreatedBy { get; set; }

        [Display(Name = "Es Cliente?")]
        // [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public bool IsCustomer { get; set; }

        [Display(Name = "Total Without Discount")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalWithoutDiscount => ShoppingCartItems?.Sum(x => x.Product != null && x.Product.ProductPrice != null ? x.Product.ProductPrice.TotalWithoutDiscount : 0) ?? 0;

        [Display(Name = "Total Discount")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDiscount => ShoppingCartItems?.Sum(x => x.Product != null && x.Product.ProductPrice != null ? x.Product.ProductPrice.TotalDiscount : 0) ?? 0;

        [Display(Name = "SAP Total Discount")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal SapTotalDiscount => ShoppingCartItems?.Sum(x => x.Product != null && x.Product.ProductPrice != null ? x.Product.ProductPrice.SapTotalDiscount : 0) ?? 0;

        [Display(Name = "Sub Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal SubTotal => TotalWithoutDiscount - TotalDiscount;

        [Display(Name = "IGV")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Igv => Total - SubTotal;

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Total => decimal.Round(SubTotal * AppDefaultValues.TaxValue, AppFormats.Total);

        [Display(Name = "SAP Sub Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal SapSubTotal => TotalWithoutDiscount - SapTotalDiscount;

        [Display(Name = "SAP IGV")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal SapIgv => SapTotal - SapSubTotal;

        [Display(Name = "SAP Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal SapTotal => decimal.Round(SapSubTotal * AppDefaultValues.TaxValue, AppFormats.Total);

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
