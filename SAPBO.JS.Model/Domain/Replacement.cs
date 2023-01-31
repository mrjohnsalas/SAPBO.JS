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
    public class Replacement
    {
        [Key]
        [Display(Name = "Repuesto Id")]
        public string Id { get; set; }

        [Display(Name = "Repuesto")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(200, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "U.Med")]
        [MaxLength(50, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string MedidaId { get; set; }

        [Display(Name = "Stock")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal Stock { get; set; }

        [Display(Name = "Costo S/")]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal Cost { get; set; }

        public ICollection<MaintenanceProgramReplacement> MaintenanceProgramReplacements { get; set; }

        public ICollection<MaintenanceWorkOrderReplacement> MaintenanceWorkOrderReplacements { get; set; }
    }
}
