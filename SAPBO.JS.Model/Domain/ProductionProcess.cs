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
    public class ProductionProcess : AuditEntity
    {
        [Key]
        [Display(Name = "Proceso de produccíon Id")]
        public int Id { get; set; }

        [Display(Name = "Proceso de produccíon")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductionProcessTypeCostId { get; set; }

        [Display(Name = "Tipo de costo")]
        public ProductionProcessTypeCost ProductionProcessTypeCost { get; set; }

        [Display(Name = "Costo $")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Costo { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        public ICollection<ProductFormulaProductionProcess> ProductionProcesses { get; set; }
    }
}
