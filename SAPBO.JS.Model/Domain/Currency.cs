using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAPBO.JS.Model.Domain
{
    public class Currency
    {
        [Key]
        [Display(Name = "Moneda Id")]
        public string Id { get; set; }

        [Display(Name = "Moneda")]
        public string Name { get; set; }

        [Display(Name = "Simbolo")]
        public string Symbol => Id.Equals("SOL") ? "S/" : Id.Equals("USD") ? "$" : "€";

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        public ICollection<SaleOrder> SaleOrders { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }

        public ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public ICollection<PurchaseOrderAuthorization> PurchaseOrderAuthorizations { get; set; }
    }
}
