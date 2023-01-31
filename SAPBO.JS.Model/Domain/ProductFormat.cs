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
    public class ProductFormat : AuditEntity
    {
        [Key]
        [Display(Name = "Formato Id")]
        public int Id { get; set; }

        [Display(Name = "Formato")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "U.Med")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(10, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 2)]
        public string UnitOfMeasurementId { get; set; }

        [Display(Name = "Medida")]
        public UnitOfMeasurement UnitOfMeasurement { get; set; }

        [Display(Name = "Ancho")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Ancho { get; set; }

        [Display(Name = "Largo")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Largo { get; set; }

        [Display(Name = "Panol")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Panol { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;
    }
}
