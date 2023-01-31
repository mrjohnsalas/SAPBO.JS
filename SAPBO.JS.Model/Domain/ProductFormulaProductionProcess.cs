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
    public class ProductFormulaProductionProcess
    {
        [Key]
        [Display(Name = "Proceso de produccíon Id")]
        public int Id { get; set; }

        [Display(Name = "Formula de producto Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductFormulaId { get; set; }

        [Display(Name = "Formula")]
        public ProductFormula ProductFormula { get; set; }

        [Display(Name = "Tipo material Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductMaterialTypeId { get; set; }

        [Display(Name = "Tipo material")]
        public ProductMaterialType ProductMaterialType { get; set; }

        [Display(Name = "Proceso de produccíon Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductionProcessId { get; set; }

        [Display(Name = "Proceso de produccíon")]
        public ProductionProcess ProductionProcess { get; set; }

        [Display(Name = "Tiempo preparación")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(5, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string PreparationTime { get; set; }

        [Display(Name = "Rendimiento")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Performance { get; set; }
    }
}
