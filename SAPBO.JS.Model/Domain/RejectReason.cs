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
    public class RejectReason
    {
        [Display(Name = "Motivo de rechazo")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string Reason { get; set; }
    }
}
