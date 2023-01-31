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
    public class Job : AuditEntity
    {
        [Key]
        [Display(Name = "Puesto trabajo Id")]
        public int Id { get; set; }

        [Display(Name = "Puesto trabajo")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(30, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Description { get; set; }

        [Display(Name = "Costo por hora S/")]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal CostHour { get; set; }

        [Display(Name = "Unidad de negocio Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string BusinessUnitId { get; set; }

        [Display(Name = "Unidad de negocio")]
        public BusinessUnit BusinessUnit { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        public ICollection<Employee> Employees { get; set; }

        public ICollection<MaintenanceProgramJob> MaintenanceProgramJobs { get; set; }
    }
}
