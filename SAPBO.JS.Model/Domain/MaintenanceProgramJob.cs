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
    public class MaintenanceProgramJob
    {
        [Key]
        [Display(Name = "PM Puesto trabajo Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int MaintenanceProgramId { get; set; }

        [Display(Name = "PM")]
        public MaintenanceProgram MaintenanceProgram { get; set; }

        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int JobId { get; set; }

        [Display(Name = "Puesto trabajo")]
        public Job Job { get; set; }

        [Display(Name = "Tiempo estimado")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(5, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string EstimatedTime { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal Quantity { get; set; }
    }
}
