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
}