using SAPBO.JS.Common;
using SAPBO.JS.Model.Helper;
using System.ComponentModel.DataAnnotations;

namespace SAPBO.JS.Model.Domain
{
    public class BusinessPartnerVehicle : AuditEntity
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Descripción")]
        public string Description => $"({Placa}) {Marca} - {Modelo} - {Year} - {PesoMaximo}";

        [Display(Name = "Año")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int Year { get; set; }

        [Display(Name = "Color")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(15, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Color { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(20, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Marca { get; set; }

        [Display(Name = "Modelo")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(25, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Modelo { get; set; }

        [Display(Name = "Placa")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(12, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Placa { get; set; }

        [Display(Name = "Peso máximo")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public decimal PesoMaximo { get; set; }

        [Display(Name = "Serie motor")]
        [StringLength(20, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string SerieMotor { get; set; }

        [Display(Name = "Vencimiento SOAT")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = AppFormats.FieldDate, ApplyFormatInEditMode = true)]
        public DateTime? VencimientoSoat { get; set; }

        [Display(Name = "Constancia inscripción")]
        [StringLength(20, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string ConstanciaInscripcion { get; set; }

        [Display(Name = "Certificado inscripción")]
        [StringLength(30, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string CertificadoInscripcion { get; set; }

        [Display(Name = "Configuración")]
        [StringLength(10, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string Configuracion { get; set; }

        [Display(Name = "Serie chasis")]
        [StringLength(30, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string SerieChasis { get; set; }

        [Display(Name = "Serie opcional")]
        [StringLength(30, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 0)]
        public string SerieOpcional { get; set; }

        [Display(Name = "Proveedor Id")]
        public string BusinessPartnerId { get; set; }

        [Display(Name = "Proveedor")]
        public BusinessPartner BusinessPartner { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;
    }
}
