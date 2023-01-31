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
    public class BusinessPartnerContact
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Index")]
        public int LineNum { get; set; }

        [Display(Name = "Contacto Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Display(Name = "Segundo nombre")]
        [MaxLength(50, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string MiddleName { get; set; }

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string LastName { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName => $"{LastName}, {FirstName} {MiddleName}";

        [Display(Name = "Titulo")]
        [MaxLength(10, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Title { get; set; }

        [Display(Name = "Posición")]
        [MaxLength(90, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Position { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Address { get; set; }

        [Display(Name = "Teléfono 1")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Phone1 { get; set; }

        [Display(Name = "Teléfono 2")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Phone2 { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(50, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string MobilePhone { get; set; }

        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Email { get; set; }

        [Display(Name = "Profesión")]
        [MaxLength(50, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Profession { get; set; }

        [Display(Name = "Cliente Id")]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Cliente")]
        public BusinessPartner BusinessPartner { get; set; }

        public ICollection<CRMActivity> Activities { get; set; }

        public ICollection<SaleOpportunity> Opportunities { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        public ICollection<SaleOrder> SaleOrders { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }

        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
