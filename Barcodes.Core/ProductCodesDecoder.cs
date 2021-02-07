using Barcodes.Codes;

namespace Barcodes.Core
{
    public class ProductCodesDecoder
    {
        public ProductCodeData Decode(BarcodeTemplate template, string code)
        {
            if (template == BarcodeTemplate.Product)
            {
                return DecodeProductCode(code);
            }
            else if (template == BarcodeTemplate.LongProduct)
            {
                return DecodeLongProductCode(code);
            }
            else if (template == BarcodeTemplate.NmvsProduct)
            {
                return DecodeNmvsProductCode(code);
            }
            return null;
        }

        public ProductCodeData Decode(string code)
        {
            var nmvsProduct = DecodeNmvsProductCode(code);
            if (nmvsProduct != null)
            {
                return nmvsProduct;
            }

            var longProduct = DecodeLongProductCode(code);
            if (longProduct != null)
            {
                return longProduct;
            }

            var product = DecodeProductCode(code);
            if (product != null)
            {
                return product;
            }
            return null;
        }

        public ProductCodeData DecodeNmvsProductCode(string code)
        {
            if (NmvsProductCode.TryParse(code, out NmvsProductCode nmvsProductCode))
            {
                return new ProductCodeData
                {
                    ProductCode = nmvsProductCode.ProductCode,
                    BatchId = nmvsProductCode.BatchId,
                    SerialNo = nmvsProductCode.SerialNo,
                    ExpireDate = nmvsProductCode.ExpireDate.ToDateTime()
                };
            }
            return null;
        }

        public ProductCodeData DecodeLongProductCode(string code)
        {
            if (LongProductCode.TryParse(code, out LongProductCode longProductCode))
            {
                return new ProductCodeData
                {
                    ProductCode = longProductCode.ProductCode,
                    BatchId = longProductCode.BatchId,
                    ExpireDate = longProductCode.ExpireDate
                };
            }
            return null;
        }

        public ProductCodeData DecodeProductCode(string code)
        {
            if (ProductCode.TryParse(code, out ProductCode productCode))
            {
                return new ProductCodeData
                {
                    ProductCode = productCode.Product
                };
            }
            return null;
        }
    }
}