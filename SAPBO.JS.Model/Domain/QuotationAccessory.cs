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
    public class QuotationAccessory : AuditEntity
    {
        [Key]
        [Display(Name = "Accesorio Id")]
        public int Id { get; set; }

        [Display(Name = "Accesorio")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Description { get; set; }

        [Display(Name = "% valor")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldPercentage, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, 100, ErrorMessage = "El campo {0} debe estar entre {2} y {1}")]
        public decimal ValueXje { get; set; }

        [Display(Name = "Orden")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int Index { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;
    }
}
