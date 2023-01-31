using SAPBO.JS.Common;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Domain
{
    public class PurchaseOrderAuthorization
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Orden compra Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int PurchaseOrderId { get; set; }

        [Display(Name = "Orden compra")]
        public PurchaseOrder PurchaseOrder { get; set; }

        [Display(Name = "Moneda Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(3, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string PurchaseOrderCurrencyId { get; set; }

        [Display(Name = "Moneda OC")]
        public Currency PurchaseOrderCurrency { get; set; }

        [Display(Name = "Total OC")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal PurchaseOrderTotal { get; set; }

        [Display(Name = "User Id solicitante")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string UserIdSolicitante { get; set; }

        [Display(Name = "Fecha de solicitud")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Motivo de rechazo")]
        [DataType(DataType.MultilineText)]
        public string RejectReason { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public Enums.PurchaseOrderAuthorizationStatus Status { get; set; }

        [Display(Name = "User Id 1")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string FirstUserId { get; set; }

        [Display(Name = "Fecha de autorización 1")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime? FirstDate { get; set; }

        [Display(Name = "Check 1")]
        public bool FirstCheck { get; set; }


        [Display(Name = "Nombre comprador")]
        [StringLength(150, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string PurchaseOrderUserName { get; set; }

        [Display(Name = "Correo comprador")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string PurchaseOrderEmail { get; set; }


        [Display(Name = "Solicitud compra Id")]
        public int? PurchaseRequestId { get; set; }

        [Display(Name = "User Id de solicitud")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string PurchaseRequestUserId { get; set; }

        [Display(Name = "Nombre solicitante")]
        [StringLength(150, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string PurchaseRequestUserName { get; set; }

        [Display(Name = "Correo solicitante")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string PurchaseRequestEmail { get; set; }
    }
}
