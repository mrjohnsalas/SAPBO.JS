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
    public class ProductGroup
    {
        [Key]
        [Display(Name = "Grupo Id")]
        public string Id { get; set; }

        [Display(Name = "Grupo Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(30, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Description { get; set; }

        [Display(Name = "Super grupo Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string ProductSuperGroupId { get; set; }

        [Display(Name = "Super grupo")]
        public ProductSuperGroup ProductSuperGroup { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
