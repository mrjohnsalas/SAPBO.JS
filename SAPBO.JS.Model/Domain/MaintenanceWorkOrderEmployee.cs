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
    public class MaintenanceWorkOrderEmployee
    {
        [Key]
        [Display(Name = "OTM Empleado Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int MaintenanceWorkOrderId { get; set; }

        [Display(Name = "OTM")]
        public MaintenanceWorkOrder MaintenanceWorkOrder { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int EmployeeId { get; set; }

        [Display(Name = "Empleado")]
        public Employee Employee { get; set; }

        [Display(Name = "Tarea")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(250, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Task { get; set; }

        [Display(Name = "Tiempo estimado")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(5, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string EstimatedTime { get; set; }
    }
}
