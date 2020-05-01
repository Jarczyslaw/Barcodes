using System;
using System.Text.RegularExpressions;

namespace Barcodes.Codes
{
    public class ProductCode : BaseCode
    {
        public override string Code => Product;

        public string Product { get; private set; }

        public ProductCode(string code)
        {
            Parse(code);
        }

        public ProductCode(ProductCode productCode)
        {
            Type = BarcodeType.Ean13;
            Product = productCode.Product;
        }

        public void Parse(string code)
        {
            var match = Regex.Match(code, "^[0-9]{13}$");
            if (!match.Success)
            {
                throw new ArgumentException("Invalid product code");
            }

            Product = code;
        }

        public static bool TryParse(string code, out ProductCode productCode)
        {
            productCode = null;
            try
            {
                productCode = new ProductCode(code);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}