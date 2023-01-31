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
    public class ProductMaterial : AuditEntity
    {
        [Key]
        [Display(Name = "Tipo de falla Id")]
        public int Id { get; set; }

        [Display(Name = "Tipo de falla")]
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

        [Display(Name = "Precio Local ($)")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal PrecioLocal { get; set; }

        [Display(Name = "Precio Importado ($)")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal PrecioImportado { get; set; }

        [Display(Name = "Precio Licitación ($)")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal PrecioLicitacion { get; set; }

        [Display(Name = "Stock")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Stock { get; set; }

        [Display(Name = "Nro. Copias")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int NroCopias { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductGrammageId { get; set; }

        [Display(Name = "Gramaje")]
        public ProductGrammage ProductGrammage { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductionProcessTypeCostId { get; set; }

        [Display(Name = "Tipo de costo")]
        public ProductionProcessTypeCost ProductionProcessTypeCost { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductMaterialTypeId { get; set; }

        [Display(Name = "Tipo Material")]
        public ProductMaterialType ProductMaterialType { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;
    }
}
