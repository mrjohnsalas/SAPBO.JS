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
    public class SaleQuotationDetail
    {
        [Key]
        [Display(Name = "Detalle Id")]
        public int Id { get; set; }

        [Display(Name = "Cotización Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int SaleQuotationId { get; set; }

        [Display(Name = "Cotización")]
        public SaleQuotation SaleQuotation { get; set; }

        [Display(Name = "Formula de producto Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductFormulaId { get; set; }

        [Display(Name = "Formula")]
        public ProductFormula ProductFormula { get; set; }

        [Display(Name = "Tipo material Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductMaterialTypeId { get; set; }

        [Display(Name = "Tipo material")]
        public ProductMaterialType ProductMaterialType { get; set; }

        [Display(Name = "Gramaje Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductGrammageId { get; set; }

        [Display(Name = "Gramaje")]
        public ProductGrammage ProductGrammage { get; set; }

        [Display(Name = "Nivel de tinta Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductInkLevelId { get; set; }

        [Display(Name = "Nivel de tinta")]
        public ProductInkLevel ProductInkLevel { get; set; }

        [Display(Name = "Formato Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int ProductFormatId { get; set; }

        [Display(Name = "Formato")]
        public ProductFormat ProductFormat { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal Quantity { get; set; }

        [Display(Name = "Ancho (m)")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal Ancho { get; set; }

        [Display(Name = "Alto (m)")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal Alto { get; set; }

        [Display(Name = "Panol")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [DisplayFormat(DataFormatString = AppFormats.FieldQuantity, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal Panol { get; set; }

        [Display(Name = "Nro. Copias")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [Range(0, int.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public int NroCopias { get; set; }

        [Display(Name = "Peso unit.")]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal UnitWeight => decimal.Round(TotalWeight / Quantity, AppFormats.UnitPrice);

        [Display(Name = "Precio unit.")]
        [DisplayFormat(DataFormatString = AppFormats.FieldUnitPrice, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal UnitPrice => decimal.Round(TotalPrice / Quantity, AppFormats.UnitPrice);

        [Display(Name = "Peso total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal TotalWeight { get; set; }

        [Display(Name = "Importe total")]
        [DisplayFormat(DataFormatString = AppFormats.FieldTotal, ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = AppMessages.ValueGreaterThanFieldErrorMessage)]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Comentario")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = AppMessages.StringMaxFieldErrorMessage)]
        public string Remark { get; set; }

        [Display(Name = "Tipo precio Id")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        public int PriceTypeId { get; set; }

        [Display(Name = "Tipo precio")]
        public Enums.PriceType PriceType => (Enums.PriceType)PriceTypeId;

        [Display(Name = "Articulo Id")]
        public string ProductId { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        [Display(Name = "Motivo de rechazo")]
        [DataType(DataType.MultilineText)]
        public string RejectReason { get; set; }
    }
}
