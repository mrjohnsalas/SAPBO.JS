using SAPBO.JS.Common;
using SAPBO.JS.Model.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAPBO.JS.Model.Domain
{
    public class MaintenanceProgram : AuditEntity
    {
        [Key]
        [Display(Name = "PM Id")]
        public int Id { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int MaintenancePriorityId { get; set; }

        [Display(Name = "Prioridad de mantenimiento")]
        public MaintenancePriority MaintenancePriority { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int MaintenanceTypeId { get; set; }

        [Display(Name = "Tipo de mantenimiento")]
        public MaintenanceType MaintenanceType { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductionMachineId { get; set; }

        [Display(Name = "Maquina")]
        public ProductionMachine ProductionMachine { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int TimeFrequencyId { get; set; }

        [Display(Name = "Frecuencia de tiempo")]
        public TimeFrequency TimeFrequency { get; set; }

        [Display(Name = "Frecuencia de tiempo")]
        public Enums.TimeFrequencyType TimeFrequencyType => (Enums.TimeFrequencyType)TimeFrequencyId;

        [Display(Name = "Valor Frecuencia de tiempo")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal TimeFrequencyValue { get; set; }

        [Display(Name = "Tiempo estimado")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(5, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string EstimatedTime { get; set; }

        [Display(Name = "Fecha y hora del último")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime? LastDate { get; set; }

        [Display(Name = "Comentario")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Remark { get; set; }

        [Display(Name = "¿Parar planta?")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public bool StopPlant { get; set; }

        [Display(Name = "¿Parar maquina?")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public bool StopMachine { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        [NotMapped]
        public bool Create { get; set; }
        [NotMapped]
        public bool SendAlert { get; set; }
        [NotMapped]
        public DateTime NextDate { get; set; }

        public ICollection<MaintenanceProgramJob> Jobs { get; set; }

        public ICollection<MaintenanceProgramTool> Tools { get; set; }

        public ICollection<MaintenanceProgramReplacement> Replacements { get; set; }
    }
}
