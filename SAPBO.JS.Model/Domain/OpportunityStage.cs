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
    public class OpportunityStage
    {
        [Key]
        [Display(Name = "Etapa Id")]
        public int Id { get; set; }

        [Display(Name = "Etapa")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(30, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Orden")]
        public int Index { get; set; }

        [Display(Name = "% de cierre")]
        public decimal ClosePercentage { get; set; }

        public ICollection<SaleOpportunityStage> SaleOpportunityStages { get; set; }
    }
}
