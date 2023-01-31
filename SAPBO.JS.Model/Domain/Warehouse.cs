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
    public class Warehouse
    {
        [Key]
        [Display(Name = "Almacén Id")]
        public string Id { get; set; }

        [Display(Name = "Almacén Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public ICollection<SaleOrderDetail> SaleOrderDetails { get; set; }

        public ICollection<DeliveryDetail> DeliveryDetails { get; set; }
    }
}
