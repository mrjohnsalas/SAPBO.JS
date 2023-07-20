using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class BillMapper : ISapB1AutoMapper<Bill>
    {
        public Bill Mapper(IRecordset rs)
        {
            var bill = new Bill
            {
                Id = int.Parse(rs.Fields.Item("DocEntry").Value.ToString()),
                FileId = int.Parse(rs.Fields.Item("AtcEntry").Value.ToString()),
                SapId = int.Parse(rs.Fields.Item("DocNum").Value.ToString()),

                SerieControl = rs.Fields.Item("U_BPV_SERI").Value.ToString(),
                NroControl = rs.Fields.Item("U_BPV_NCON2").Value.ToString(),

                SaleOrderTypeId = (int)Utilities.StringToSaleOrderType(rs.Fields.Item("TYPE").Value.ToString()),

                BillTypeId = (int)Utilities.StringToBillType(rs.Fields.Item("BILLTYPE").Value.ToString()),

                BusinessPartnerId = rs.Fields.Item("CardCode").Value.ToString(),

                PostingDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("DocDate").Value, rs.Fields.Item("DocTime").Value).Value,
                DeliveryDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("DocDueDate").Value).Value,
                DocumentDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("TaxDate").Value).Value,

                CurrencyId = rs.Fields.Item("DocCur").Value.ToString(),

                ContactId = int.Parse(rs.Fields.Item("CntctCode").Value.ToString()),

                PaymentId = int.Parse(rs.Fields.Item("GroupNum").Value.ToString()),

                Rate = decimal.Parse(rs.Fields.Item("DocRate").Value.ToString()),

                SaleEmployeeId = int.Parse(rs.Fields.Item("SlpCode").Value.ToString()),

                ShipAddressId = rs.Fields.Item("ShipToCode").Value.ToString(),

                BillAddressId = rs.Fields.Item("PayToCode").Value.ToString(),

                Remarks = rs.Fields.Item("Comments").Value.ToString(),
                ReferenceNumber = rs.Fields.Item("NumAtCard").Value.ToString(),

                SaleOrderId = Utilities.IntValueToIntOrNull(rs.Fields.Item("U_CL_NRO_OV").Value),

                AfectoDetraccion = int.Parse(rs.Fields.Item("CHKDET").Value.ToString()) == 1,
                IncluyePercepcion = int.Parse(rs.Fields.Item("CHKPER").Value.ToString()) == 1,

                StatusId = (int)Utilities.StringToBillStatus(rs.Fields.Item("DocStatus").Value.ToString()),

                UserId = rs.Fields.Item("UserSign2").Value.ToString(),

                AgentId = rs.Fields.Item("U_CL_CODAGE").Value.ToString(),
                AgentAddressId = rs.Fields.Item("U_CL_ADDAGE").Value.ToString(),

                DNIConsignatario = rs.Fields.Item("U_CL_DNICON").Value.ToString(),
                FirstNameConsignatario = rs.Fields.Item("U_CL_NOCONS").Value.ToString(),
                LastNameConsignatario = rs.Fields.Item("U_CL_APECON").Value.ToString(),
                PhoneConsignatario = rs.Fields.Item("U_CL_CELCON").Value.ToString(),
                EmailConsignatario = rs.Fields.Item("U_CL_EMACON").Value.ToString(),

                SubTotal = decimal.Parse(rs.Fields.Item("SUB_TOTAL").Value.ToString()),
                Impuesto = decimal.Parse(rs.Fields.Item("IGV").Value.ToString()),
                Total = decimal.Parse(rs.Fields.Item("TOTAL").Value.ToString()),

                PagadoHastaHoy = decimal.Parse(rs.Fields.Item("PAGADO_A_HOY").Value.ToString()),
                Saldo = decimal.Parse(rs.Fields.Item("SALDO").Value.ToString()),
                SaldoVencido = decimal.Parse(rs.Fields.Item("SALDO_VENCIDO").Value.ToString()),

                SaldoSoles = decimal.Parse(rs.Fields.Item("SALDO_SOLES").Value.ToString()),
                SaldoDolar = decimal.Parse(rs.Fields.Item("SALDO_DOLAR").Value.ToString())
            };

            var pdfLine = rs.Fields.Item("PDF_LINE").Value.ToString();
            if (pdfLine != null)
            {
                bill.PDFFile = true;
                //bill.Files.Add(new BillFile
                //{
                //    Id = int.Parse(pdfLine),
                //    BillId = bill.Id,
                //    FilePath = rs.Fields.Item("PDF_PATH").Value.ToString(),
                //    FileName = rs.Fields.Item("PDF_FULLFILENAME").Value.ToString(),
                //    FileDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("PDF_DATE").Value).Value,
                //    BillFileType = Enums.BillFileType.PDF
                //});
            }

            var xmlLine = rs.Fields.Item("XML_LINE").Value.ToString();
            if (xmlLine != null)
            {
                bill.XMLFile = true;
                //bill.Files.Add(new BillFile
                //{
                //    Id = int.Parse(xmlLine),
                //    BillId = bill.Id,
                //    FilePath = rs.Fields.Item("XML_PATH").Value.ToString(),
                //    FileName = rs.Fields.Item("XML_FULLFILENAME").Value.ToString(),
                //    FileDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("XML_DATE").Value).Value,
                //    BillFileType = Enums.BillFileType.XML
                //});
            }

            var zipLine = rs.Fields.Item("ZIP_LINE").Value.ToString();
            if (zipLine != null)
            {
                bill.ZipFile = true;
                //bill.Files.Add(new BillFile
                //{
                //    Id = int.Parse(zipLine),
                //    BillId = bill.Id,
                //    FilePath = rs.Fields.Item("ZIP_PATH").Value.ToString(),
                //    FileName = rs.Fields.Item("ZIP_FULLFILENAME").Value.ToString(),
                //    FileDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("ZIP_DATE").Value).Value,
                //    BillFileType = Enums.BillFileType.Zip
                //});
            }

            return bill;
        }

        public IUserTable SetValuesToUserTable(IUserTable table, Bill obj) => table;
    }
}
