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
    public class BusinessUnit
    {
        [Key]
        [Display(Name = "Unidad de negocio Id")]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 2)]
        public string Id { get; set; }

        [Display(Name = "Unidad de negocio")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 2)]
        public string Name { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public ICollection<Job> Jobs { get; set; }
    }
}
