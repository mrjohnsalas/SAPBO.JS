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
    public class CRMActivity
    {
        [Key]
        [Display(Name = "Actividad Id")]
        public int Id { get; set; }

        [Display(Name = "Tipo de Actividad")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public Enums.ActivityType ActivityType { get; set; }

        public int ActivityClass => -1;

        public string ActivitySubject => "-1";

        public int? AssignedToEmployeeId { get; set; }

        public string AssignedToEmployee { get; set; }

        public int? AssignedToUserId { get; set; }

        public string AssignedToUser { get; set; }

        public int AssignedByUserId { get; set; }

        public string AssignedByUser { get; set; }

        [Display(Name = "Cliente Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Cliente")]
        public BusinessPartner BusinessPartner { get; set; }

        [Display(Name = "Contacto Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ContactId { get; set; }

        [Display(Name = "Contacto")]
        public BusinessPartnerContact Contact { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.PhoneNumber)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 6)]
        public string ContactPhone { get; set; }

        [Display(Name = "Nota")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.MultilineText)]
        [StringLength(256000, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 10)]
        public string Notes { get; set; }

        // Notes, Meetings and Calls

        [Display(Name = "Comentario")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 10)]
        public string Comment { get; set; }

        [Display(Name = "Fecha y hora de inicio")]
        // [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Prioridad de Actividad")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public Enums.ActivityPriority ActivityPriority { get; set; }

        // Meetings and Calls

        [Display(Name = "Duración")]
        public decimal ActivityDuration { get; set; }

        [Display(Name = "Tipo de Duración")]
        public Enums.ActivityDurationType ActivityDurationType { get; set; }

        // Calls

        [Display(Name = "Fecha y hora de fin")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        //Meetings

        public Enums.ActivityLocation ActivityLocation { get; set; }

        [Display(Name = "Dirección Id")]
        public string AddressId { get; set; }

        [Display(Name = "Dirección")]
        public BusinessPartnerAddress Address { get; set; }

        [Display(Name = "País Id")]
        public string CountryId { get; set; }

        [Display(Name = "País")]
        public Country Country { get; set; }

        [Display(Name = "Departamento Id")]
        public string StateId { get; set; }

        [Display(Name = "Departamento")]
        public State State { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string City { get; set; }

        [Display(Name = "Calle")]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Street { get; set; }

        [Display(Name = "Sala")]
        [MaxLength(50, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Room { get; set; }

        public bool IsClosed { get; set; }

        public DateTime? ClosedDate { get; set; }
    }
}
