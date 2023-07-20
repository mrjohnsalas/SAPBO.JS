using SAPBO.JS.Common;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Domain
{
    public class BillFile
    {
        [Key]
        [Display(Name = "Line Id")]
        public int Id { get; set; }

        [Display(Name = "Factura Id")]
        public int BillId { get; set; }

        [Display(Name = "Factura")]
        public Bill Bill { get; set; }

        [Display(Name = "Ruta archivo")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(3, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string FilePath { get; set; }

        [Display(Name = "Nombre archivo")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(3, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string FileName { get; set; }

        [Display(Name = "Fecha anexo")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime FileDate { get; set; }

        public string FullFilePath => $"{FilePath}\\{FileName}";

        public Enums.BillFileType BillFileType { get; set; }
    }
}
