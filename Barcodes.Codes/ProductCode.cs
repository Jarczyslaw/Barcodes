using System;
using System.Text.RegularExpressions;

namespace Barcodes.Codes
{
    public class ProductCode
    {
        public string Code { get; private set; }

        public ProductCode(string code)
        {
            Parse(code);
        }

        public ProductCode(ProductCode productCode)
        {
            Code = productCode.Code;
        }

        public void Parse(string code)
        {
            var match = Regex.Match(code, "^[0-9]{13}$");
            if (!match.Success)
            {
                throw new ArgumentException("Invalid code string format");
            }

            Code = code;
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

        public override string ToString()
        {
            return Code;
        }
    }
}