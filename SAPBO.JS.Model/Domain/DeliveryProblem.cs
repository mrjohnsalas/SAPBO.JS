using SAPBO.JS.Common;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Domain
{
    public class DeliveryProblem
    {
        [Key]
        [Display(Name = "Entrega Id")]
        public int Id { get; set; }

        [Display(Name = "Transportista Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Transportista")]
        public BusinessPartner BusinessPartner { get; set; }

        [Display(Name = "Fecha y hora del último")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime ProblemDate { get; set; }

        [Display(Name = "Comentarios")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.MultilineText)]
        [StringLength(254, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string Problem { get; set; }
    }
}
