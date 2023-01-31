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
    public class ProductFormula : AuditEntity
    {
        [Key]
        [Display(Name = "Formula de producto Id")]
        public int Id { get; set; }

        [Display(Name = "Formula de producto")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Description { get; set; }

        [Display(Name = "U.Med")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(10, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 2)]
        public string UnitOfMeasurementId { get; set; }

        [Display(Name = "Medida")]
        public UnitOfMeasurement UnitOfMeasurement { get; set; }

        [Display(Name = "Super grupo Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string ProductSuperGroupId { get; set; }

        [Display(Name = "Super grupo")]
        public ProductSuperGroup ProductSuperGroup { get; set; }

        [Display(Name = "Nro. Copias Min.")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal NroCopiasMinimo { get; set; }

        [Display(Name = "Nro. Copias Max.")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal NroCopiasMaximo { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        public ICollection<ProductFormulaConsumptionFactor> ConsumptionFactors { get; set; }

        public ICollection<ProductFormulaProductionProcess> ProductionProcesses { get; set; }
    }
}
