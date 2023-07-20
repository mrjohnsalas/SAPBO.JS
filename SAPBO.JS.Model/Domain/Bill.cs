using SAPBO.JS.Common;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Domain
{
    public class Bill
    {
        [Key]
        [Display(Name = "Factura Id")]
        public int Id { get; set; }

        public int SapId { get; set; }

        public int FileId { get; set; }

        [Display(Name = "Serie control")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string SerieControl { get; set; }

        [Display(Name = "Nro. Control")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string NroControl { get; set; }

        public string FullNroControl => BillType == Enums.BillType.Letra ? $"LT-{SerieControl}-{NroControl}" : $"{SerieControl}-{NroControl}";

        public int SaleOrderTypeId { get; set; }

        [Display(Name = "Tipo factura")]
        public Enums.SaleOrderType SaleOrderType => (Enums.SaleOrderType)SaleOrderTypeId;

        public int BillTypeId { get; set; }

        [Display(Name = "Tipo factura")]
        public Enums.BillType BillType => (Enums.BillType)BillTypeId;

        [Display(Name = "Socio negocio Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Socio negocio")]
        public BusinessPartner BusinessPartner { get; set; }

        [Display(Name = "Fecha de contabilización")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime PostingDate { get; set; }

        [Display(Name = "Fecha de entrega")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Fecha del documento")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime DocumentDate { get; set; }

        [Display(Name = "Moneda Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(3, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public Currency Currency { get; set; }

        [Display(Name = "Contacto Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
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

        [Display(Name = "Dirección destino Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string ShipAddressId { get; set; }

        [Display(Name = "Dirección destino")]
        public BusinessPartnerAddress ShipAddress { get; set; }

        [Display(Name = "Dirección factura Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string BillAddressId { get; set; }

        [Display(Name = "Dirección factura")]
        public BusinessPartnerAddress BillAddress { get; set; }

        [Display(Name = "Comentarios")]
        [DataType(DataType.MultilineText)]
        [StringLength(254, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string Remarks { get; set; }

        [Display(Name = "Nro. Referencia")]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Orden de venta Id")]
        public int? SaleOrderId { get; set; }

        [Display(Name = "Afecto a detracción?")]
        public bool AfectoDetraccion { get; set; }

        [Display(Name = "Incluye percepción?")]
        public bool IncluyePercepcion { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        public ICollection<BillDetail> Details { get; set; }

        public ICollection<BillFile> Files { get; set; }

        public bool PDFFile { get; set; }

        public bool XMLFile { get; set; }

        public bool ZipFile { get; set; }

        [Display(Name = "User Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(100, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 18)]
        public string UserId { get; set; }

        //AGENCIA------------------>>

        [Display(Name = "Agencia Id")]
        [StringLength(15, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 13)]
        public string AgentId { get; set; }

        [Display(Name = "Agencia")]
        public BusinessPartner Agent { get; set; }

        [Display(Name = "Dirección agencia Id")]
        [StringLength(250, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string AgentAddressId { get; set; }

        [Display(Name = "Dirección de agencia")]
        public BusinessPartnerAddress AgentAddress { get; set; }

        [Display(Name = "DNI")]
        [StringLength(8, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 8)]
        public string DNIConsignatario { get; set; }

        [Display(Name = "Nombre")]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string FirstNameConsignatario { get; set; }

        [Display(Name = "Apellidos")]
        [StringLength(50, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string LastNameConsignatario { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullNameConsignatario => $"{LastNameConsignatario}, {FirstNameConsignatario}";

        [Display(Name = "Celular")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(12, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string PhoneConsignatario { get; set; }

        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string EmailConsignatario { get; set; }

        //------------------------->>

        [Display(Name = "Sub Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal SubTotal { get; set; }

        [Display(Name = "IGV")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Impuesto { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [Display(Name = "Saldo")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Saldo { get; set; }

        [Display(Name = "Pagado hasta hoy")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal PagadoHastaHoy { get; set; }

        [Display(Name = "Saldo vencido")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal SaldoVencido { get; set; }

        [Display(Name = "Saldo Soles")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal SaldoSoles { get; set; }

        [Display(Name = "Saldo Dolar")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal SaldoDolar { get; set; }
    }
}
