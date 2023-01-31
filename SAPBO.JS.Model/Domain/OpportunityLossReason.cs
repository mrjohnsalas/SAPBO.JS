﻿using SAPBO.JS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAPBO.JS.Model.Domain
{
    public class OpportunityLossReason
    {
        [Key]
        [Display(Name = "Razón de pérdida Id")]
        public int Id { get; set; }

        [Display(Name = "Razón de pérdida")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(30, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        public ICollection<SaleOpportunityLossReason> SaleOpportunityLossReasons { get; set; }
    }
}
