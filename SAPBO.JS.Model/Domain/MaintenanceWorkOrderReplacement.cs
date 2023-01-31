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
    public class MaintenanceWorkOrderReplacement
    {
        [Key]
        [Display(Name = "OTM Herramienta Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int MaintenanceWorkOrderId { get; set; }

        [Display(Name = "OTM")]
        public MaintenanceWorkOrder MaintenanceWorkOrder { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string ReplacementId { get; set; }

        [Display(Name = "Repuesto")]
        public Replacement Replacement { get; set; }

        [Display(Name = "Cant. Planificada")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal PlannedQuantity { get; set; }

        [Display(Name = "Cant. Consumida")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal ConsumedQuantity { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int TimeFrequencyId { get; set; }

        [Display(Name = "Frecuencia de tiempo")]
        public TimeFrequency TimeFrequency { get; set; }

        [Display(Name = "Frecuencia de tiempo")]
        public Enums.TimeFrequencyType TimeFrequencyType => (Enums.TimeFrequencyType)TimeFrequencyId;

        [Display(Name = "Valor Frecuencia de tiempo")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal TimeFrequencyValue { get; set; }
    }
}
