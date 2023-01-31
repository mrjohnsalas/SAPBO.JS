using SAPBO.JS.Common;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Helper
{
    public class AuditEntity
    {
        [Display(Name = "Creado el")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Creado por")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string CreatedBy { get; set; }

        [Display(Name = "Actualizado el")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Actualizado por")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string UpdatedBy { get; set; }

        [Display(Name = "Anulado el")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime? DeletedAt { get; set; }

        [Display(Name = "Anulado por")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string DeletedBy { get; set; }
    }
}
