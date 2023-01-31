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
    public class SaleOpportunity
    {
        [Key]
        [Display(Name = "Oportunidad Id")]
        public int Id { get; set; }

        [Display(Name = "Tipo de Oportunidad")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public Enums.SaleOpportunityType SaleOpportunityType { get; set; }

        [Display(Name = "Cliente Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Cliente")]
        public BusinessPartner BusinessPartner { get; set; }

        [Display(Name = "Contacto Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ContactId { get; set; }

        [Display(Name = "Contacto")]
        public BusinessPartnerContact Contact { get; set; }

        [Display(Name = "Sale Empleado Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int SaleEmployeeId { get; set; }

        public string SaleEmployee { get; set; }

        [Display(Name = "Empleado Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int EmployeeId { get; set; }

        public string Employee { get; set; }

        [Display(Name = "Asunto")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 10)]
        public string Subject { get; set; }

        [Display(Name = "Comentario")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.MultilineText)]
        [StringLength(256000, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 10)]
        public string Notes { get; set; }

        [Display(Name = "Fecha de inicio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha de cierre")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime? CloseDate { get; set; }

        [Display(Name = "Fecha de cierre esperada")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime ExpectedCloseDate { get; set; }

        [Display(Name = "% de cierre")]
        public decimal? ClosePercentage { get; set; }

        [Display(Name = "Monto potencial")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal PotentialAmount { get; set; }

        [Display(Name = "Monto ponderado")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal? WeightedAmount { get; set; }

        [Display(Name = "Nivel de interés")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public Enums.SaleOpportunityInterestLevel SaleOpportunityInterestLevel { get; set; }

        [Display(Name = "Status de Oportunidad")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public Enums.SaleOpportunityStatus SaleOpportunityStatus { get; set; }

        public ICollection<SaleOpportunityStage> Stages { get; set; }

        public ICollection<SaleOpportunityLossReason> LossReasons { get; set; }
    }
}
