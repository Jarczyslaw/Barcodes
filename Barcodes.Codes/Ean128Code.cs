using Barcodes.Extensions;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Barcodes.Codes
{
    public class Ean128Code
    {
        public Ean128Code(string codeString) : this(Parse(codeString))
        {
        }

        public Ean128Code(string productCode, string serialNumber, DateTime expireDate)
        {
            Validate(productCode, serialNumber, expireDate);
            ProductCode = productCode;
            SerialNumber = serialNumber;
            ExpireDate = expireDate;
        }

        public Ean128Code(Ean128Code ean128Code)
        {
            ProductCode = ean128Code.ProductCode;
            SerialNumber = ean128Code.SerialNumber;
            ExpireDate = ean128Code.ExpireDate;
        }

        public string ProductCode { get; }
        public string SerialNumber { get; }
        public DateTime ExpireDate { get; }

        public static Ean128Code Parse(string codeString)
        {
            codeString = codeString.Trim();
            var match = Regex.Match(codeString, @"\(02\)(\d{14})\(17\)(\d{6})\(10\)(.+)");
            if (!match.Success)
            {
                throw new ArgumentException("Invalid code string format");
            }

            var productCode = match.Groups[1].Value;
            var expireDate = match.Groups[2].Value;
            var serialNumber = match.Groups[3].Value;

            if (DateTime.TryParseExact(expireDate, "yyMMdd", null, DateTimeStyles.None, out DateTime expireDateTime))
            {
                return new Ean128Code(productCode, serialNumber, expireDateTime);
            }
            else
            {
                throw new ArgumentException("Invalid date time value");
            }
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

        public void Validate(string productCode, string serialNumber, DateTime expireDate)
        {
            if (!ValidateProductCode(productCode))
            {
                throw new ArgumentException("Invalid product code");
            }

            if (!ValidateSerialNumber(serialNumber))
            {
                throw new ArgumentException("Invalid serial number");
            }

            if (!ValidateExpireDate(expireDate))
            {
                throw new ArgumentException("Invalid batch expiration date");
            }
        }

        public static bool ValidateProductCode(string productCode)
        {
            return !string.IsNullOrEmpty(productCode) && productCode.Length == 14 && productCode.ContainsOnlyDigits();
        }

        public static bool ValidateSerialNumber(string serialNumber)
        {
            return !string.IsNullOrEmpty(serialNumber);
        }

        public static bool ValidateExpireDate(DateTime expireDate)
        {
            return expireDate != default(DateTime);
        }

        public override string ToString()
        {
            return $"(02){ProductCode}(17){ExpireDate.ToExpireDate(false)}(10){SerialNumber}";
        }
    }
}
