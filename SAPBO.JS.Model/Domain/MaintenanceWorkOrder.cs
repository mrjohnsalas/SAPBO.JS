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
    public class MaintenanceWorkOrder : AuditEntity
    {
        [Key]
        [Display(Name = "OTM Id")]
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

        public int? ProductionMachineId { get; set; }

        [Display(Name = "Maquina")]
        public ProductionMachine ProductionMachine { get; set; }

        public int? ProductionMachineZoneId { get; set; }

        [Display(Name = "Zona de Maquina")]
        public ProductionMachineZone ProductionMachineZone { get; set; }

        public string CostCenterId { get; set; }

        [Display(Name = "Centro costos")]
        public CostCenter CostCenter { get; set; }

        [Display(Name = "Fecha y hora de inicio")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha y hora de fin")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime? FinalDate { get; set; }

        [Display(Name = "Horas efectivas")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(5, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string EffectiveHours { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int EmployeeId { get; set; }

        [Display(Name = "Supervisor")]
        public Employee Employee { get; set; }

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

        [Display(Name = "PM Id")]
        public int? MaintenanceProgramId { get; set; }

        [Display(Name = "Tipo OTM Id")]
        public int OtmTypeId { get; set; }

        [Display(Name = "Tipo OTM")]
        public Enums.OTMType OTMType => (Enums.OTMType)OtmTypeId;

        public ICollection<MaintenanceWorkOrderEmployee> Employees { get; set; }

        public ICollection<MaintenanceWorkOrderReplacement> Replacements { get; set; }

        public ICollection<MaintenanceWorkOrderTool> Tools { get; set; }

        public ICollection<MachineFailure> Failures { get; set; }
    }
}
