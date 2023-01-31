using SAPBO.JS.Common;
using SAPBO.JS.Model.Helper;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Domain
{
    public class BusinessPartnerDriver : AuditEntity
    {
        [Key]
        [Display(Name = "Conductor Id")]
        public int Id { get; set; }

        [Display(Name = "Nombres")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string LastName { get; set; }

        [Display(Name = "Conductor")]
        public string FullName => $"{LastName}, {FirstName}";

        [Display(Name = "Licencia de conducir")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(15, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string LicenseId { get; set; }

        [Display(Name = "Fec. Vencimiento lic.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime? LicenseExpirationDate { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string Phone { get; set; }

        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string Email { get; set; }

        [Display(Name = "Proveedor Id")]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Proveedor")]
        public BusinessPartner BusinessPartner { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;
    }
}
