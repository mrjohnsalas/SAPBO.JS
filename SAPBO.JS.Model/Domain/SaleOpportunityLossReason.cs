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
    public class SaleOpportunityLossReason
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Oportunidad Id")]
        public int SaleOpportunityId { get; set; }

        [Display(Name = "Oportunidad")]
        public SaleOpportunity SaleOpportunity { get; set; }

        [Display(Name = "Razón de pérdida Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int LossReasonId { get; set; }

        [Display(Name = "Razón de pérdida")]
        public OpportunityLossReason LossReason { get; set; }

        [Display(Name = "Comentario")]
        [DataType(DataType.MultilineText)]
        [MaxLength(256000, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Notes { get; set; }
    }
}
