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
    public class MachineFailure : AuditEntity
    {
        [Key]
        [Display(Name = "Falla de maquina Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductionMachineId { get; set; }

        [Display(Name = "Maquina")]
        public ProductionMachine ProductionMachine { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int? ProductionMachineZoneId { get; set; }

        [Display(Name = "Zona de Maquina")]
        public ProductionMachineZone ProductionMachineZone { get; set; }

        [Display(Name = "Fecha y hora de inicio")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha y hora de fin")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime FinalDate { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int FailureTypeId { get; set; }

        [Display(Name = "Tipo de falla")]
        public FailureType FailureType { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int FailureSeverityId { get; set; }

        [Display(Name = "Severidad de falla")]
        public FailureSeverity FailureSeverity { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int FailureCauseId { get; set; }

        [Display(Name = "Causa de falla")]
        public FailureCause FailureCause { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int FailureMechanismId { get; set; }

        [Display(Name = "Mecanismo de falla")]
        public FailureMechanism FailureMechanism { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int FailureImpactId { get; set; }

        [Display(Name = "Impacto de falla")]
        public FailureImpact FailureImpact { get; set; }

        [Display(Name = "Motivo")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Remark { get; set; }

        [Display(Name = "¿Parar maquina?")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public bool StopMachine { get; set; }

        [Display(Name = "Fecha de inicio parada")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime? StopStartDate { get; set; }

        [Display(Name = "Fecha de fin parada")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime? StopFinalDate { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        [Display(Name = "OTM Id")]
        public int? MaintenanceWorkOrderId { get; set; }

        [Display(Name = "OTM")]
        public MaintenanceWorkOrder MaintenanceWorkOrder { get; set; }
    }
}
