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
    public class SaleOpportunityStage
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Oportunidad Id")]
        public int SaleOpportunityId { get; set; }

        [Display(Name = "Oportunidad")]
        public SaleOpportunity SaleOpportunity { get; set; }

        [Display(Name = "Fecha de inicio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha de cierre")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = AppFormats.FieldFullDate, ApplyFormatInEditMode = true)]
        public DateTime CloseDate { get; set; }

        [Display(Name = "Sale Empleado Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int SaleEmployeeId { get; set; }

        public string SaleEmployee { get; set; }

        [Display(Name = "Etapa Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int OpportunityStageId { get; set; }

        [Display(Name = "Etapa")]
        public OpportunityStage OpportunityStage { get; set; }

        [Display(Name = "% de cierre")]
        public decimal? ClosePercentage { get; set; }

        [Display(Name = "Monto potencial")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal? PotentialAmount { get; set; }

        [Display(Name = "Monto ponderado")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal? WeightedAmount { get; set; }

        [Display(Name = "Empleado Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int EmployeeId { get; set; }

        public string Employee { get; set; }
    }
}
