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
    public class MaintenanceProgramReplacement
    {
        [Key]
        [Display(Name = "PM Repuesto Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int MaintenanceProgramId { get; set; }

        [Display(Name = "PM")]
        public MaintenanceProgram MaintenanceProgram { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string ReplacementId { get; set; }

        [Display(Name = "Repuesto")]
        public Replacement Replacement { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal Quantity { get; set; }
    }
}
