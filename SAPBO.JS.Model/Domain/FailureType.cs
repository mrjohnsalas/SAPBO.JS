using SAPBO.JS.Common;
using SAPBO.JS.Model.Helper;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Domain
{
    public class FailureType : AuditEntity
    {
        [Key]
        [Display(Name = "Tipo de falla Id")]
        public int Id { get; set; }

        [Display(Name = "Tipo de falla")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(30, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Description { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        public ICollection<MachineFailure> MachineFailures { get; set; }
    }
}
