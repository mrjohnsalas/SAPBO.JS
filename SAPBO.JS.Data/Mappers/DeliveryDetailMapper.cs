using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Data.Mappers
{
    public class DeliveryDetailMapper : ISapB1AutoMapper<DeliveryDetail>
    {
        public DeliveryDetail Mapper(IRecordset rs)
        {
            return new DeliveryDetail
            {
                Id = int.Parse(rs.Fields.Item("LineNum").Value.ToString()),
                DeliveryId = int.Parse(rs.Fields.Item("DocEntry").Value.ToString()),

                SaleOrderId = int.Parse(rs.Fields.Item("BaseEntry").Value.ToString()),
                SaleOrderLineNumId = int.Parse(rs.Fields.Item("BaseLine").Value.ToString()),

                ProductId = rs.Fields.Item("ItemCode").Value.ToString(),
                ProductDetail = rs.Fields.Item("Text").Value.ToString(),

                MedidaId = rs.Fields.Item("unitMsr").Value.ToString(),

                WarehouseId = rs.Fields.Item("WhsCode").Value.ToString(),

                Quantity = decimal.Parse(rs.Fields.Item("Quantity").Value.ToString()),
                PendingQuantity = decimal.Parse(rs.Fields.Item("OpenInvQty").Value.ToString()),

                IsCustodia = rs.Fields.Item("U_CL_ENVCUS").Value.ToString().Equals("Y"),
                CustodiaQuantity = decimal.Parse(rs.Fields.Item("U_CL_CANCUS").Value.ToString()),

                UnitWeight = decimal.Parse(rs.Fields.Item("UNIT_WEIGHT").Value.ToString()),
                TotalWeight = decimal.Parse(rs.Fields.Item("TOTAL_WEIGHT").Value.ToString()),

                BasePrice = decimal.Parse(rs.Fields.Item("PRECIO_BASE").Value.ToString()),
                BaseTotal = decimal.Parse(rs.Fields.Item("TOTAL_SIN_DESC").Value.ToString()),

                XjeCustomerDiscount = decimal.Parse(rs.Fields.Item("XJE_DESC_CLI").Value.ToString()),
                TotalCustomerDiscount = decimal.Parse(rs.Fields.Item("DESC_CLI").Value.ToString()),
                CustomerPrice = decimal.Parse(rs.Fields.Item("PRECIO_CLI").Value.ToString()),
                CustomerTotal = decimal.Parse(rs.Fields.Item("TOTAL_C_DESC_CLI").Value.ToString()),

                XjeQuantityDiscount = decimal.Parse(rs.Fields.Item("XJE_DESC_CANT").Value.ToString()),
                TotalQuantityDiscount = decimal.Parse(rs.Fields.Item("DESC_CANT").Value.ToString()),
                FinalPrice = decimal.Parse(rs.Fields.Item("PRECIO_FINAL").Value.ToString()),
                FinalTotal = decimal.Parse(rs.Fields.Item("TOTAL_C_DESC_CANT").Value.ToString()),

                IsDespachado = rs.Fields.Item("U_CL_STSDES").Value.ToString().Equals("1"),
                DespachoDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECDES").Value, rs.Fields.Item("U_CL_HORDES").Value),
                UserIdDespacho = rs.Fields.Item("U_CL_USRDES").Value.ToString(),

                IsEntregado = rs.Fields.Item("U_CL_STSENT").Value.ToString().Equals("1"),
                EntregadoDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_CL_FECENT").Value, rs.Fields.Item("U_CL_HORENT").Value),
                UserIdEntregado = rs.Fields.Item("U_CL_USRENT").Value.ToString(),

                StatusId = int.Parse(rs.Fields.Item("LineStatus").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, DeliveryDetail obj) => table;
    }
}
