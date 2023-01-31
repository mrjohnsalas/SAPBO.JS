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
    public class ProductFormulaConsumptionFactor
    {
        [Key]
        [Display(Name = "Factor de consumo Id")]
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

        [Display(Name = "Desde")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal From { get; set; }

        [Display(Name = "Hasta")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal Until { get; set; }

        [Display(Name = "Factor")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal Factor { get; set; }
    }
}
