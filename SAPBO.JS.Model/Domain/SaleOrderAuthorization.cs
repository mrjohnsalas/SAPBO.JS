using SAPBO.JS.Common;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Domain
{
    public class SaleOrderAuthorization
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Orden venta Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int SaleOrderId { get; set; }

        [Display(Name = "Orden venta")]
        public SaleOrder SaleOrder { get; set; }

        //--> Solicitante

        [Display(Name = "User Id solicitante")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string RequestingUserId { get; set; }

        [Display(Name = "Fecha de solicitud")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Motivo de rechazo")]
        [DataType(DataType.MultilineText)]
        public string RejectReason { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        //--> First user

        [Display(Name = "User Id 1")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string FirstUserId { get; set; }

        [Display(Name = "Fecha de autorización 1")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime? FirstDate { get; set; }

        [Display(Name = "Check 1")]
        public bool FirstCheck { get; set; }

        [Display(Name = "User Id 1 Name")]
        public string FirstUserName { get; set; }
    }
}
