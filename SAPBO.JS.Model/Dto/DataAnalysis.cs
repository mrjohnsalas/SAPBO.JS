using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Dto
{
    public class DataAnalysis<T>
    {
        [Display(Name = "Actualizado al")]
        public DateTime UpdatedTo { get; set; }

        [Display(Name = "Vendedor Id")]
        public int SaleEmployeeId { get; set; }

        [Display(Name = "Cliente Id")]
        public string BusinessPartnerId { get; set; }

        public ICollection<T> Data { get; set; }
    }
}
