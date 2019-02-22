using Barcodes.Extensions;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Barcodes.Codes
{
    public class Ean128Code : IEquatable<Ean128Code>
    {
        public Ean128Code(string codeString)
            : this(Parse(codeString))
        {
        }

        public Ean128Code(string productCode, string batchId, string expireDate)
            : this(productCode, batchId, DateTime.ParseExact(expireDate, "yyMMdd", null))
        {
        }

        public Ean128Code(string productCode, string batchId, DateTime expireDate)
        {
            Validate(productCode, batchId);
            ProductCode = productCode;
            BatchId = batchId;
            ExpireDate = expireDate;
        }

        public Ean128Code(Ean128Code ean128Code)
        {
            ProductCode = ean128Code.ProductCode;
            BatchId = ean128Code.BatchId;
            ExpireDate = ean128Code.ExpireDate;
        }

        public string ProductCode { get; }
        public string BatchId { get; }
        public DateTime ExpireDate { get; }

        public static Ean128Code Parse(string codeString)
        {
            var match = Regex.Match(codeString, @"\(02\)(\d{14})\(17\)(\d{6})\(10\)(.+)");
            if (!match.Success)
            {
                throw new ArgumentException("Invalid code string format");
            }

            var productCode = match.Groups[1].Value;
            var expireDate = match.Groups[2].Value;
            var batchId = match.Groups[3].Value;

            var date = DateTime.ParseExact(expireDate, "yyMMdd", null, DateTimeStyles.None);
            return new Ean128Code(productCode, batchId, date);
        }

        public static bool TryParse(string codeString, out Ean128Code ean128Code)
        {
            ean128Code = null;
            try
            {
                ean128Code = Parse(codeString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Validate(string productCode, string batchId)
        {
            if (!ValidateProductCode(productCode))
            {
                throw new ArgumentException("Invalid product code");
            }

            if (!ValidateBatchId(batchId))
            {
                throw new ArgumentException("Invalid batch id");
            }
        }

        public static bool ValidateProductCode(string productCode)
        {
            return !string.IsNullOrEmpty(productCode) && productCode.Length == 14 && productCode.ContainsOnlyDigits();
        }

        public static bool ValidateBatchId(string batchId)
        {
            return !string.IsNullOrEmpty(batchId);
        }

        public override string ToString()
        {
            return $"(02){ProductCode}(17){ExpireDate.ToExpireDate(false)}(10){BatchId}";
        }

        public bool Equals(Ean128Code other)
        {
            return ProductCode == other.ProductCode && BatchId == other.BatchId
                && ExpireDate == other.ExpireDate;
        }
    }
}
