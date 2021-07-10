using System;
using System.Collections.Generic;

namespace Barcodes.Codes
{
    public class AlleyCode : BaseCode
    {
        public AlleyCode()
        {
        }

        public AlleyCode(string code)
        {
            Parse(code);
        }

        public AlleyCode(string locationAddress, string alleyAddress)
        {
            CheckLocationAndAddress(locationAddress, alleyAddress);
            LocationAddress = locationAddress;
            AlleyAddress = alleyAddress;
        }

        public string LocationAddress { get; private set; } = string.Empty;
        public string AlleyAddress { get; private set; } = string.Empty;

        public override string Code => CurrentPrefix + LocationAddress + AlleyAddress;

        public override BarcodeType Type => BarcodeType.Code128;
        public override List<string> Prefixes => new List<string> { "AL" };

        private void CheckLocationAndAddress(string locationAddress, string alleyAddress)
        {
            if (string.IsNullOrEmpty(locationAddress) || locationAddress.Length != 10)
            {
                throw new ArgumentException("Invalid location address");
            }

            if (string.IsNullOrEmpty(alleyAddress) || alleyAddress.Length < 2)
            {
                throw new ArgumentException("Invalid alley address");
            }
        }

        public override void Parse(string code)
        {
            CheckCode(code);
            var body = GetCodeBody(code);
            var length = 12;
            if (body.Length < length)
            {
                throw new ArgumentException($"Invalid code length (minimum {length} characters)");
            }
            LocationAddress = body.Substring(0, 10);
            AlleyAddress = body.Substring(10);
            CheckCodeOnlyDigits(LocationAddress);
        }

        public static bool TryParse(string code, out AlleyCode alleyCode)
        {
            alleyCode = null;
            try
            {
                alleyCode = new AlleyCode(code);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}