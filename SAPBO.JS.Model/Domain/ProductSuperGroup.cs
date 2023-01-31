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
    public class ProductSuperGroup
    {
        [Key]
        [Display(Name = "Super grupo Id")]
        public string Id { get; set; }

        [Display(Name = "Super grupo Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(30, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        public ICollection<ProductGroup> ProductGroups { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<ProductFormula> ProductFormulas { get; set; }
    }
}
