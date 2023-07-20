using SAPBO.JS.Common;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Dto
{
    public class SaleGoalDataBySaleEmployee
    {
        [Display(Name = "Año")]
        public int Year { get; set; }

        [Display(Name = "Mes")]
        public int Month { get; set; }

        [Display(Name = "Meta Linea")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal LineaGoal { get; set; }

        [Display(Name = "Meta Impreso")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal ImpresoGoal { get; set; }

        [Display(Name = "Meta Flexo")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal FlexoGoal { get; set; }

        [Display(Name = "Meta Otro")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal OtroGoal { get; set; }

        [Display(Name = "Total Dolar")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal TotalDolar { get; set; }
    }
}
