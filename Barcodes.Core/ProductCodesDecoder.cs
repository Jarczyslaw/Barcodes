using Barcodes.Codes;
using System;

namespace Barcodes.Core
{
    public class ProductCodeData
    {
        public string ProductCode { get; set; } = string.Empty;
        public string BatchId { get; set; } = string.Empty;
        public string SerialNo { get; set; } = string.Empty;
        public DateTime? ExpireDate { get; set; }
    }

    public class ProductCodesDecoder
    {
        public ProductCodeData Decode(string code)
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
            else if (LongProductCode.TryParse(code, out LongProductCode longProductCode))
            {
                return new ProductCodeData
                {
                    ProductCode = longProductCode.ProductCode,
                    BatchId = longProductCode.BatchId,
                    ExpireDate = longProductCode.ExpireDate
                };
            }
            else if (ProductCode.TryParse(code, out ProductCode productCode))
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