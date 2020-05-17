using Barcodes.Extensions;
using System;
using System.Text.RegularExpressions;

namespace Barcodes.Codes
{
    public class NmvsCode : BaseCode
    {
        public NmvsCode(string nmvsCode)
        {
            Parse(nmvsCode);
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

        public string ProductCode { get; private set; } = string.Empty;
        public string SerialNo { get; private set; } = string.Empty;
        public string BatchId { get; private set; } = string.Empty;
        public NmvsDate ExpireDate { get; private set; }

        public override string Code => $"01{ProductCode}17{ExpireDate}21{SerialNo}{GroupSeparator}10{BatchId}";

        public override BarcodeType Type => BarcodeType.DataMatrix;

        public void Parse(string codeString)
        {
            codeString = codeString.Replace(GroupSeparator, string.Empty);
            var match = Regex.Match(codeString, @"^01(\d{14})17(\d{6})21(.{1,20})10(.+)$");
            if (!match.Success)
            {
                throw new ArgumentException("Invalid code string format");
            }

            ProductCode = match.Groups[1].Value;
            ExpireDate = new NmvsDate(match.Groups[2].Value);
            SerialNo = match.Groups[3].Value;
            BatchId = match.Groups[4].Value;
        }

        public static bool TryParse(string codeString, out NmvsCode nmvsCode)
        {
            nmvsCode = null;
            try
            {
                nmvsCode = new NmvsCode(codeString);
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
    }
}
