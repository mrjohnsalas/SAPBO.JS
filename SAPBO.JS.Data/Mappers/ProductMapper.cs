using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductMapper : ISapB1AutoMapper<Product>
    {
        public Product Mapper(IRecordset rs)
        {
            var product = new Product();

            product.Id = rs.Fields.Item("ItemCode").Value.ToString();
            product.Name = rs.Fields.Item("ItemName").Value.ToString();
            product.MedidaId = rs.Fields.Item("SalUnitMsr").Value.ToString();
            product.Remark = rs.Fields.Item("UserText").Value.ToString();

            product.ProductGroupId = rs.Fields.Item("U_CL_GRPART").Value.ToString();
            product.ProductSuperGroupId = rs.Fields.Item("U_CL_SUPGRP").Value.ToString();
            product.ProductClassId = rs.Fields.Item("U_CL_CODCLA").Value.ToString();

            product.UnidadesxPaquete = decimal.Parse(rs.Fields.Item("U_CL_UNDPQT").Value.ToString());
            product.AnchoPaquete = decimal.Parse(rs.Fields.Item("U_CL_ANCPQT").Value.ToString());
            product.LargoPaquete = decimal.Parse(rs.Fields.Item("U_CL_LARPQT").Value.ToString());
            product.AltoPaquete = decimal.Parse(rs.Fields.Item("U_CL_ALTPQT").Value.ToString());
            product.PaquetesxCaja = decimal.Parse(rs.Fields.Item("U_CL_PQTCAJ").Value.ToString());
            product.AnchoCaja = decimal.Parse(rs.Fields.Item("U_CL_ANCCAJ").Value.ToString());
            product.LargoCaja = decimal.Parse(rs.Fields.Item("U_CL_LARCAJ").Value.ToString());
            product.AltoCaja = decimal.Parse(rs.Fields.Item("U_CL_ALTCAJ").Value.ToString());
            product.CantidadMinimaVenta = decimal.Parse(rs.Fields.Item("U_CL_CANMIV").Value.ToString());
            product.MultiploCantidad = decimal.Parse(rs.Fields.Item("U_CL_MULCAN").Value.ToString());
            product.BaseUnitPrice = decimal.Parse(rs.Fields.Item("U_CL_PREUSD").Value.ToString());

            product.DefaultWarehouseId = rs.Fields.Item("DEFALM").Value.ToString();
            product.SalesWarehouseId = rs.Fields.Item("SALALM").Value.ToString();

            product.Stock = decimal.Parse(rs.Fields.Item("OnHand").Value.ToString());
            product.CommittedStock = decimal.Parse(rs.Fields.Item("IsCommited").Value.ToString());
            product.AvailableStock = decimal.Parse(rs.Fields.Item("Available").Value.ToString());
            product.MaxCustomerQuantity = decimal.Parse(rs.Fields.Item("MaxCustomerQuantity").Value.ToString());

            return product;
        }

        public IUserTable SetValuesToUserTable(IUserTable table, Product obj) => table;
    }
}
