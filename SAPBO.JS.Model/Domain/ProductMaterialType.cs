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
    public class ProductMaterialType : AuditEntity
    {
        [Key]
        [Display(Name = "Tipo material Id")]
        public int Id { get; set; }

        [Display(Name = "Tipo material")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Description { get; set; }

        [Display(Name = "Mostrar gramaje?")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public bool ShowGramaje { get; set; }

        [Display(Name = "Activar copias?")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public bool EnableCopias { get; set; }

        [Display(Name = "Es Papel?")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public bool IsPaper { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        public ICollection<ProductMaterial> ProductMaterials { get; set; }

        public ICollection<ProductFormulaConsumptionFactor> ConsumptionFactors { get; set; }

        public ICollection<ProductFormulaProductionProcess> ProductionProcesses { get; set; }
    }
}
