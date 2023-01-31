using SAPBO.JS.Common;
using SAPBO.JS.Model.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAPBO.JS.Model.Domain
{
    public class Employee : AuditEntity
    {
        [Key]
        [Display(Name = "Empleado Id")]
        public int Id { get; set; }

        [Display(Name = "Nombres")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string LastName { get; set; }

        [Display(Name = "Nombre completo")]
        public string FullName => $"{LastName} {FirstName}";

        [Display(Name = "Cod. Grafipapel")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(10, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 5)]
        public string GrafipapelId { get; set; }

        [Display(Name = "DNI")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(8, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 8)]
        public string DNI { get; set; }

        [Display(Name = "Costo por hora S/")]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal CostHour { get; set; }

        [Display(Name = "Puesto trabajo Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int JobId { get; set; }

        [Display(Name = "Puesto trabajo")]
        public Job Job { get; set; }

        [Display(Name = "Es supervisor?")]
        public bool IsSuper { get; set; }

        [Display(Name = "Accede a WebApp?")]
        public bool WebApp { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Foto")]
        public string ProfilePhotoPath { get; set; }

        [Display(Name = "Vendedor Id")]
        public int? SaleEmployeeId { get; set; }

        [Display(Name = "Comprador Id")]
        public int? PurchaseEmployeeId { get; set; }

        [Display(Name = "Empleado Id")]
        public int? EmployeeId { get; set; }

        [Display(Name = "Unidad de negocio Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string BusinessUnitId { get; set; }

        [Display(Name = "Unidad de negocio")]
        public BusinessUnit BusinessUnit { get; set; }

        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        public ICollection<MaintenanceWorkOrderEmployee> MaintenanceWorkOrderEmployees { get; set; }

        public ICollection<MaintenanceWorkOrder> MaintenanceWorkOrders { get; set; }

        public ICollection<BusinessPartner> BusinessPartners { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        public ICollection<SaleOrder> SaleOrders { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }
    }
}
