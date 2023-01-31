using SAPBO.JS.Common;
using SAPBO.JS.Model.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAPBO.JS.Model.Domain
{
    public class BusinessPartner : AuditEntity
    {
        [Key]
        [Display(Name = "Cliente Id")]
        public string Id { get; set; }

        [Display(Name = "Razón social")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "RUC")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(11, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 8)]
        public string Ruc { get; set; }

        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string Email { get; set; }

        [Display(Name = "Telefono 1")]
        [DataType(DataType.PhoneNumber)]
        public string Phone1 { get; set; }

        [Display(Name = "Telefono 2")]
        [DataType(DataType.PhoneNumber)]
        public string Phone2 { get; set; }

        [Display(Name = "Celular")]
        [DataType(DataType.PhoneNumber)]
        public string Cellular { get; set; }

        [Display(Name = "Sitio web")]
        [DataType(DataType.Url)]
        public string WebSite { get; set; }

        [Display(Name = "Línea de crédito")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal CreditLine { get; set; }

        [Display(Name = "Línea de crédito disponible")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal CreditLineAvailable { get; set; }

        [Display(Name = "Moneda por defecto")]
        public string DefaultCurrencyId { get; set; }

        public string WebAppAuthorizationValue { get; set; }

        [Display(Name = "Requiere autorización?")]
        public bool WebAppAuthorization => WebAppAuthorizationValue == null || WebAppAuthorizationValue.Equals("Y");

        public string WebAppAuthorizationDisplay => WebAppAuthorization ? "Si" : "No";

        [Display(Name = "Vendedor Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int SaleEmployeeId { get; set; }

        [Display(Name = "Empleado Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int EmployeeId { get; set; }

        [Display(Name = "Empleado")]
        public Employee Employee { get; set; }

        [Display(Name = "Es temporal")]
        public bool IsTemp { get; set; }

        public ICollection<BusinessPartnerContact> Contacts { get; set; }

        public ICollection<BusinessPartnerPayment> Payments { get; set; }

        public ICollection<BusinessPartnerAddress> Addresses { get; set; }

        public ICollection<CRMActivity> Activities { get; set; }

        public ICollection<SaleOpportunity> Opportunities { get; set; }

        public ICollection<ProductPrice> ProductPrices { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        public ICollection<SaleOrder> SaleOrders { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }

        public ICollection<ProductQuantityDiscount> ProductQuantityDiscounts { get; set; }

        public ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public ICollection<BusinessPartnerDriver> Drivers { get; set; }

        public ICollection<BusinessPartnerVehicle> Vehicles { get; set; }
    }
}
