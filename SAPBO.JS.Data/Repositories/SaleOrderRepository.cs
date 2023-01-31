using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Utility;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Data.Repositories
{
    public class SaleOrderRepository : SapB1GenericRepository<SaleOrder>
    {
        public SaleOrderRepository(SapB1Context context, ISapB1AutoMapper<SaleOrder> mapper) : base(context, mapper)
        {

        }

        public void Create(ShoppingCart shoppingCart)
        {
            _context.Connect();

            var saleOrder = (IDocuments)_context.Company.GetBusinessObject(BoObjectTypes.oOrders);

            saleOrder.CardCode = shoppingCart.BusinessPartner.Id;
            saleOrder.CardName = shoppingCart.BusinessPartner.Name;

            saleOrder.ContactPersonCode = shoppingCart.ContactId;

            saleOrder.DocDate = shoppingCart.RateDate;
            saleOrder.DocDueDate = shoppingCart.DeliveryDate;
            saleOrder.TaxDate = shoppingCart.RateDate;

            saleOrder.DocCurrency = shoppingCart.CurrencyId;
            saleOrder.DocRate = (double)shoppingCart.Rate;

            saleOrder.PaymentGroupCode = shoppingCart.PaymentId;

            saleOrder.NumAtCard = shoppingCart.ReferenceNumber ?? string.Empty;
            saleOrder.UserFields.Fields.Item("U_VS_OCCLIENTE").Value = shoppingCart.BpReferenceNumber ?? string.Empty;

            saleOrder.SalesPersonCode = shoppingCart.IsCustomer ? AppDefaultValues.SaleEmployeeId : shoppingCart.SaleEmployeeId.Value;

            saleOrder.Comments = shoppingCart.Remark ?? string.Empty;

            saleOrder.ShipToCode = shoppingCart.ShipAddressId;
            saleOrder.PayToCode = shoppingCart.BillAddressId;

            if (!shoppingCart.IsCustomer && !shoppingCart.ShipAddress.EsLima)
            {
                saleOrder.UserFields.Fields.Item("U_CL_CODAGE").Value = shoppingCart.AgentId;
                saleOrder.UserFields.Fields.Item("U_CL_ADDAGE").Value = shoppingCart.AgentAddressId;
            }

            saleOrder.UserFields.Fields.Item("U_CL_ISCUST").Value = shoppingCart.IsCustomer ? 1 : 0;

            saleOrder.UserFields.Fields.Item("U_CL_MONCLI").Value = saleOrder.DocCurrency;
            saleOrder.UserFields.Fields.Item("U_CL_CONPAG").Value = saleOrder.PaymentGroupCode;
            saleOrder.JournalMemo = saleOrder.CardName.Length > 50 ? saleOrder.CardName.Substring(0, 50) : saleOrder.CardName;

            //CONSIGNATARIO
            if (!string.IsNullOrEmpty(shoppingCart.DNIConsignatario))
                saleOrder.UserFields.Fields.Item("U_CL_DNICON").Value = shoppingCart.DNIConsignatario;

            if (!string.IsNullOrEmpty(shoppingCart.FirstNameConsignatario))
                saleOrder.UserFields.Fields.Item("U_CL_NOCONS").Value = shoppingCart.FirstNameConsignatario;

            if (!string.IsNullOrEmpty(shoppingCart.LastNameConsignatario))
                saleOrder.UserFields.Fields.Item("U_CL_APECON").Value = shoppingCart.LastNameConsignatario;

            if (!string.IsNullOrEmpty(shoppingCart.PhoneConsignatario))
                saleOrder.UserFields.Fields.Item("U_CL_CELCON").Value = shoppingCart.PhoneConsignatario;

            if (!string.IsNullOrEmpty(shoppingCart.EmailConsignatario))
                saleOrder.UserFields.Fields.Item("U_CL_EMACON").Value = shoppingCart.EmailConsignatario;

            //LOGIC VALUES ORDER
            saleOrder.UserFields.Fields.Item("U_VS_FESTAT").Value = "N"; //ESTADO FACTURA ELECTRONICA
            saleOrder.UserFields.Fields.Item("U_VS_AFEDET").Value = "Y"; //AFECTO A DETRACCION
            saleOrder.UserFields.Fields.Item("U_VS_APLANT").Value = "Y"; //APLICA ANTICIPO
            saleOrder.UserFields.Fields.Item("U_VS_GRATEN").Value = "1"; //A TITULO GRATUITO
            saleOrder.UserFields.Fields.Item("U_VS_RSPTCORRG").Value = "N"; //CORRECCION AL RECHAZO
            saleOrder.UserFields.Fields.Item("U_CL_FLGAPR").Value = "N"; //APROBACION?
            saleOrder.UserFields.Fields.Item("U_VS_INCPRCP").Value = "N"; //INCLUYE PERCEPCION
            saleOrder.UserFields.Fields.Item("U_CL_INCNC").Value = shoppingCart.TotalDiscount > 0 ? "Y" : "N"; //INCLUYE NOTA DE CREDITO / DESCUENTO

            //DEFAULT VALUES ORDER : CABECERA
            saleOrder.UserFields.Fields.Item("U_CL_ENDOSO").Value = "N";
            saleOrder.UserFields.Fields.Item("U_CL_HOJSEG").Value = "N";
            saleOrder.UserFields.Fields.Item("U_CL_ESTPRO").Value = "N";
            saleOrder.UserFields.Fields.Item("U_CL_FICTEC").Value = "N";
            saleOrder.UserFields.Fields.Item("U_CL_VENEXT").Value = "N";
            saleOrder.UserFields.Fields.Item("U_OK1_Anulada").Value = "N";
            saleOrder.UserFields.Fields.Item("U_VS_USRSV").Value = "N";
            saleOrder.UserFields.Fields.Item("U_VS_INCPRCP").Value = "N";
            saleOrder.UserFields.Fields.Item("U_VS_METVAL").Value = "N";
            saleOrder.UserFields.Fields.Item("U_LN_TIPOCAP").Value = "-";
            saleOrder.UserFields.Fields.Item("U_BPP_MDBI").Value = "A";
            saleOrder.UserFields.Fields.Item("U_BPP_MDTS").Value = "TSI";
            saleOrder.UserFields.Fields.Item("U_VS_TIPOPER").Value = "01";
            saleOrder.UserFields.Fields.Item("U_BPP_CDAD").Value = "000";
            saleOrder.UserFields.Fields.Item("U_CL_TIPRES").Value = "G";
            saleOrder.UserFields.Fields.Item("U_CL_TR_DSC").Value = "GRAFIPAPEL";
            saleOrder.UserFields.Fields.Item("U_CL_EXCLPP").Value = "N";
            saleOrder.UserFields.Fields.Item("U_CL_DESCLI").Value = "N";
            saleOrder.UserFields.Fields.Item("U_VS_FLGCOTIZA").Value = "N";
            saleOrder.UserFields.Fields.Item("U_CL_CERTAN").Value = "N";
            saleOrder.UserFields.Fields.Item("U_CL_FROMAPP").Value = "N";
            saleOrder.UserFields.Fields.Item("U_CL_MOVTRA").Value = string.Empty;
            saleOrder.UserFields.Fields.Item("U_CL_CMBDAT").Value = "0";
            saleOrder.UserFields.Fields.Item("U_VS_SDOCTOTAL").Value = 0;
            saleOrder.UserFields.Fields.Item("U_CL_PORCOM").Value = 0;
            saleOrder.UserFields.Fields.Item("U_CL_PORCM2").Value = 0;
            saleOrder.UserFields.Fields.Item("U_CL_PORCM3").Value = 0;
            saleOrder.UserFields.Fields.Item("U_CL_PORDET").Value = 0;
            saleOrder.UserFields.Fields.Item("U_CL_SALDO").Value = 0;
            saleOrder.UserFields.Fields.Item("U_CL_PESBAL").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_PORDET").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_MONDET").Value = 0;
            saleOrder.UserFields.Fields.Item("U_CL_TOTDES").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_EXW").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_FCA").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_FAS").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_CFR").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_CPT").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_CIP").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_DAF").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_DES").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_DEQ").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_DDU").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_DDP").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_FOB_I").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_CIF_I").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_SEGURO_I").Value = 0;
            saleOrder.UserFields.Fields.Item("U_VS_FLETE_I").Value = 0;

            foreach (var item in shoppingCart.ShoppingCartItems)
            {
                saleOrder.Lines.ItemCode = item.Product.Id;
                saleOrder.Lines.ItemDescription = item.Product.Name;
                saleOrder.Lines.ItemDetails = item.ProductDetail;
                saleOrder.Lines.Quantity = (double)item.Quantity;
                saleOrder.Lines.TaxCode = AppDefaultValues.TaxCode;
                saleOrder.Lines.SalesPersonCode = saleOrder.SalesPersonCode;
                saleOrder.Lines.WarehouseCode = item.Product.SalesWarehouseId;
                saleOrder.Lines.VatGroup = saleOrder.Lines.TaxCode;
                saleOrder.Lines.FreeText = saleOrder.Lines.ItemDescription;
                saleOrder.Lines.UserFields.Fields.Item("U_tipoOpT12").Value = "01";

                saleOrder.Lines.Currency = saleOrder.DocCurrency;
                saleOrder.Lines.Price = (double)item.Product.ProductPrice.SapFinalUnitPrice;
                saleOrder.Lines.UserFields.Fields.Item("U_CL_PU_LST").Value = (double)item.Product.ProductPrice.BaseUnitPrice;
                saleOrder.Lines.UserFields.Fields.Item("U_CL_PU_RES").Value = saleOrder.Lines.Price;
                saleOrder.Lines.UserFields.Fields.Item("U_CL_PUREBA").Value = saleOrder.Lines.Price;
                saleOrder.Lines.UserFields.Fields.Item("U_CL_PORDNC").Value = (double)(item.Product.ProductPrice.ProductDiscountXje * 100);
                saleOrder.Lines.UserFields.Fields.Item("U_CL_PORCOM").Value = (double)item.Product.ProductPrice.CommissionXje;

                saleOrder.Lines.Add();
            }

            int resultCode = saleOrder.Add();

            if (resultCode.Equals(0))
            {
                shoppingCart.SaleOrderId = GetValue("GP_WEB_APP_048", "DocEntry", new List<dynamic> { saleOrder.SalesPersonCode });
                return;
            }

            var ex = SapB1ExceptionBuilder.BuildException(resultCode, _context.Company.GetLastErrorDescription());
            if (!string.IsNullOrEmpty(ex.Message))
            {
                GC.Collect();
                // TODO task.run exception - user-unmanaged
                throw ex;
            }
        }

        public void Delete(int id)
        {
            _context.Connect();

            var saleOrder = (IDocuments)_context.Company.GetBusinessObject(BoObjectTypes.oOrders);
            saleOrder.GetByKey(id);
            var resultCode = saleOrder.Cancel();

            if (resultCode.Equals(0)) return;

            var ex = SapB1ExceptionBuilder.BuildException(resultCode, _context.Company.GetLastErrorDescription());
            if (!string.IsNullOrEmpty(ex.Message))
            {
                GC.Collect();
                // TODO task.run exception - user-unmanaged
                throw ex;
            }
        }

        public void AddFileAttachment(int saleOrderId, string path, string fileNameWithoutExtension, string fileExtensionWithoutDot)
        {
            _context.Connect();

            var saleOrder = (SAPbobsCOM.IDocuments)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
            var fileAttachment = (SAPbobsCOM.Attachments2)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);

            fileAttachment.Lines.SourcePath = path;
            fileAttachment.Lines.FileName = fileNameWithoutExtension;
            fileAttachment.Lines.FileExtension = fileExtensionWithoutDot;
            fileAttachment.Lines.Override = BoYesNoEnum.tNO;

            int errorCode = fileAttachment.Add();

            if (!errorCode.Equals(0))
            {
                var ex = SapB1ExceptionBuilder.BuildException(errorCode, _context.Company.GetLastErrorDescription());
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    GC.Collect();
                    // TODO task.run exception - user-unmanaged
                    throw ex;
                }
            }

            var newId = int.Parse(_context.Company.GetNewObjectKey());
            saleOrder.GetByKey(saleOrderId);
            saleOrder.AttachmentEntry = newId;

            errorCode = saleOrder.Update();

            if (!errorCode.Equals(0))
            {
                var ex = SapB1ExceptionBuilder.BuildException(errorCode, _context.Company.GetLastErrorDescription());
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    GC.Collect();
                    // TODO task.run exception - user-unmanaged
                    throw ex;
                }
            }

        }
    }
}
