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
    public class SaleQuotation : AuditEntity
    {
        [Key]
        [Display(Name = "Cotización Id")]
        public int Id { get; set; }

        [Display(Name = "Cotización Id")]
        public string IdFull => $"CT-{CreatedAt?.Year:0000}-{CreatedAt?.Month:00}-{Id:0000}";

        [Display(Name = "Cliente Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Cliente")]
        public BusinessPartner BusinessPartner { get; set; }

        [Display(Name = "Fecha de entrega")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Moneda Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(3, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public Currency Currency { get; set; }

        [Display(Name = "Contacto Id")]
        public int ContactId { get; set; }

        [Display(Name = "Contacto")]
        public BusinessPartnerContact Contact { get; set; }

        [Display(Name = "Condición de pago Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int PaymentId { get; set; }

        [Display(Name = "Condición de pago")]
        public BusinessPartnerPayment Payment { get; set; }

        [Display(Name = "Tipo cambio")]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Rate { get; set; }

        [Display(Name = "Vendedor Id")]
        public int SaleEmployeeId { get; set; }

        [Display(Name = "Vendedor")]
        public Employee SaleEmployee { get; set; }

        [Display(Name = "Validez (días)")]
        [Range(0, int.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public int DaysValidValue { get; set; }

        [Display(Name = "Comentarios")]
        [DataType(DataType.MultilineText)]
        [StringLength(254, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string Remarks { get; set; }

        [Display(Name = "Motivo de rechazo")]
        [DataType(DataType.MultilineText)]
        public string RejectReason { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal Total { get; set; }

        [Display(Name = "Total aceptado")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal AceptedTotal { get; set; }

        [Display(Name = "Total rechazado")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal RejectedTotal { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        [Display(Name = "¿Es de cliente?")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public bool IsCustomer { get; set; }

        [Display(Name = "Fecha de termino")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public ICollection<SaleQuotationDetail> Products { get; set; }
    }
}
