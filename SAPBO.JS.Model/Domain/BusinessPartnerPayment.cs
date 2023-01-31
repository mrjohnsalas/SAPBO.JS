using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAPBO.JS.Model.Domain
{
    public class BusinessPartnerPayment
    {
        [Key]
        [Display(Name = "Condición de pago Id")]
        public int Id { get; set; }

        [Display(Name = "Condición de pago")]
        public string Name { get; set; }

        public ICollection<BusinessPartner> BusinessPartners { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        public ICollection<SaleOrder> SaleOrders { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }

        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
