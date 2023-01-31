using SAPBO.JS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAPBO.JS.Model.Domain
{
    public class BusinessPartnerAddress
    {
        [Key]
        [Display(Name = "Dirección Id")]
        public string Id { get; set; }

        [Display(Name = "Calle")]
        public string Street { get; set; }

        [Display(Name = "País Id")]
        public string CountryId { get; set; }

        [Display(Name = "País")]
        public Country Country { get; set; }

        [Display(Name = "Departamento Id")]
        public string StateId { get; set; }

        [Display(Name = "Departamento")]
        public State State { get; set; }

        [Display(Name = "Provincia")]
        public string City { get; set; }

        [Display(Name = "Distrito")]
        public string County { get; set; }

        [Display(Name = "Cliente Id")]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Cliente")]
        public BusinessPartner BusinessPartner { get; set; }

        [Display(Name = "Dirección")]
        public string FullDescription => $"{CountryId}, {City} - {County}, {Street}";

        public string FullIdAndDescription => $"{Id}: {FullDescription}";

        public bool EsLima { get; set; }

        public Enums.AddressType AddressType { get; set; }

        public ICollection<CRMActivity> Activities { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        public ICollection<SaleOrder> SaleOrders { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }

        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
