using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class ProductPriceMapper : ISapB1AutoMapper<ProductPrice>
    {
        public ProductPrice Mapper(IRecordset rs)
        {
            var productPrice = new ProductPrice
            {
                ProductId = rs.Fields.Item("PRODUCT_ID").Value.ToString(),
                BusinessPartnerId = rs.Fields.Item("CUSTOMER_ID").Value.ToString(),
                CurrencyId = rs.Fields.Item("CURRENCY_ID").Value.ToString(),
                Quantity = decimal.Parse(rs.Fields.Item("QUANTITY").Value.ToString()),
                Rate = decimal.Parse(rs.Fields.Item("RATE").Value.ToString()),
                BaseUnitPrice = decimal.Parse(rs.Fields.Item("PRECIO").Value.ToString()),
                CustomerDiscountXje = decimal.Parse(rs.Fields.Item("XJE_DESCUENTO_CLIENTE").Value.ToString()),
                ProductDiscountXje = decimal.Parse(rs.Fields.Item("XJE_DESCUENTO_PRODUCTO").Value.ToString()),
                MaxProductDiscountXje = decimal.Parse(rs.Fields.Item("XJE_DESCUENTO_PRODUCTO_MAX").Value.ToString()),
                CommissionXje = decimal.Parse(rs.Fields.Item("XJE_COMISION").Value.ToString())
            };

            productPrice.FinalUnitPrice = productPrice.BaseUnitPrice;
            productPrice.SapFinalUnitPrice = productPrice.BaseUnitPrice;

            productPrice.CustomerDiscount = decimal.Round(productPrice.FinalUnitPrice * productPrice.CustomerDiscountXje, 6);
            productPrice.Discount += productPrice.CustomerDiscount;
            productPrice.FinalUnitPrice -= productPrice.CustomerDiscount;
            productPrice.SapFinalUnitPrice -= productPrice.CustomerDiscount;

            var productDiscount = decimal.Round(productPrice.FinalUnitPrice * productPrice.ProductDiscountXje, 6);
            productPrice.Discount += productDiscount;
            productPrice.FinalUnitPrice -= productPrice.CustomerDiscount;

            return productPrice;
        }

        public IUserTable SetValuesToUserTable(IUserTable table, ProductPrice obj) => table;
    }
}
