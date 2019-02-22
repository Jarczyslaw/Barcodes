using Barcodes.Extensions;
using System;
using System.Text.RegularExpressions;

namespace Barcodes.Codes
{
    public class NmvsCode : IEquatable<NmvsCode>
    {
        public NmvsCode(string codeString)
            : this(Parse(codeString))
        {
        }

        public NmvsCode(string productCode, string serialNo, string batchId, string expireDate)
            : this(productCode, serialNo, batchId, NmvsDate.Parse(expireDate))
        {
        }

        public NmvsCode(string productCode, string serialNo, string batchId, NmvsDate expireDate)
        {
            Validate(productCode, serialNo, batchId);
            ProductCode = productCode;
            SerialNo = serialNo;
            BatchId = batchId;
            ExpireDate = expireDate;
        }

        public NmvsCode(NmvsCode nmvsCode)
        {
            ProductCode = nmvsCode.ProductCode;
            SerialNo = nmvsCode.SerialNo;
            BatchId = nmvsCode.BatchId;
            ExpireDate = nmvsCode.ExpireDate;
        }

        public static string GroupSeparator { get; } = "\u001D";

        public string ProductCode { get; }
        public string SerialNo { get; }
        public string BatchId { get; }
        public NmvsDate ExpireDate { get; }

        public static NmvsCode Parse(string codeString)
        {
            codeString = codeString.Replace(GroupSeparator, string.Empty);
            var match = Regex.Match(codeString, @"01(\d{14})17(\d{6})21(.{1,20})10(.+)");
            if (!match.Success)
            {
                throw new ArgumentException("Invalid code string format");
            }

            var productCode = match.Groups[1].Value;
            var expireDate = match.Groups[2].Value;
            var serialNo = match.Groups[3].Value;
            var batchId = match.Groups[4].Value;

            return new NmvsCode(productCode, serialNo, batchId, new NmvsDate(expireDate));
        }

        public static bool TryParse(string codeString, out NmvsCode nmvsCode)
        {
            nmvsCode = null;
            try
            {
                nmvsCode = Parse(codeString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Validate(string productCode, string serialNo, string batchId)
        {
            if (!ValidateProductCode(productCode))
            {
                throw new ArgumentException("Invalid product code");
            }

            if (!ValidateSerialNo(serialNo))
            {
                throw new ArgumentException("Invalid serial number");
            }

            if (!ValidateBatchId(batchId))
            {
                throw new ArgumentException("Invalid batch identifier");
            }
        }

        public static bool ValidateProductCode(string productCode)
        {
            return !string.IsNullOrEmpty(productCode) && productCode.Length == 14 && productCode.ContainsOnlyDigits();
        }

        public static bool ValidateSerialNo(string serialNo)
        {
            return !string.IsNullOrEmpty(serialNo) && serialNo.Length <= 20;
        }

        public static bool ValidateBatchId(string batchId)
        {
            return !string.IsNullOrEmpty(batchId);
        }

        public override string ToString()
        {
            return string.Format("01{0}17{1}21{2}{3}10{4}",
                ProductCode, ExpireDate.ToString(), SerialNo, GroupSeparator, BatchId);
        }

        public bool Equals(NmvsCode other)
        {
            return ProductCode == other.ProductCode && SerialNo == other.SerialNo 
                && BatchId == other.BatchId && ExpireDate.Equals(other.ExpireDate);
        }
    }
}
