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
    public class State
    {
        [Key]
        [Display(Name = "Departamento Id")]
        public string Id { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "País Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string CountryId { get; set; }

        [Display(Name = "País")]
        public Country Country { get; set; }
    }
}
