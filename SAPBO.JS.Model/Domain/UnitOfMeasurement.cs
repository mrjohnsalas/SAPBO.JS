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
    public class UnitOfMeasurement
    {
        [Key]
        [Display(Name = "Medida Id")]
        public string Id { get; set; }

        [Display(Name = "Medida")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(30, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        public ICollection<ProductFormat> ProductFormats { get; set; }

        public ICollection<ProductMaterial> ProductMaterials { get; set; }

        public ICollection<ProductFormula> ProductFormulas { get; set; }
    }
}
