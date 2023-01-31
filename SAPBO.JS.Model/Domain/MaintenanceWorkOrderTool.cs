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
    public class MaintenanceWorkOrderTool
    {
        [Key]
        [Display(Name = "OTM Herramienta Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int MaintenanceWorkOrderId { get; set; }

        [Display(Name = "OTM")]
        public MaintenanceWorkOrder MaintenanceWorkOrder { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int MaintenanceToolId { get; set; }

        [Display(Name = "Herramienta")]
        public MaintenanceTool MaintenanceTool { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal Quantity { get; set; }
    }
}
