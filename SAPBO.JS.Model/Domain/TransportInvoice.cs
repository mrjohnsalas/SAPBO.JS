using SAPBO.JS.Common;
using SAPBO.JS.Model.Helper;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Domain
{
    public class TransportInvoice : AuditEntity
    {
        [Key]
        [Display(Name = "Entrega Id")]
        public int Id { get; set; }

        [Display(Name = "Transportista Id")]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Transportista")]
        public BusinessPartner BusinessPartner { get; set; }

        [Display(Name = "Conductor Id")]
        public int BusinessPartnerDriverId { get; set; }

        [Display(Name = "Conductor")]
        public BusinessPartnerDriver BusinessPartnerDriver { get; set; }

        [Display(Name = "Vehículo Id")]
        public int BusinessPartnerVehicleId { get; set; }

        [Display(Name = "Vehículo")]
        public BusinessPartnerVehicle BusinessPartnerVehicle { get; set; }

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

        [Display(Name = "Comentarios")]
        [DataType(DataType.MultilineText)]
        [StringLength(254, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string Remarks { get; set; }
    }
}
