using System.Collections.Generic;

namespace Barcodes.Codes
{
    public class LocationCode : BaseCode
    {
        public LocationCode()
        {
        }

        public LocationCode(string codeOrAddress)
        {
            Parse(codeOrAddress);
        }

        public LocationCode(LocationCode locationCode)
        {
            Address = locationCode.Address;
        }

        public string Address { get; private set; } = string.Empty;

        public override string Code => CurrentPrefix + Address;

        public override BarcodeType Type => BarcodeType.Code128;
        public override int Length => CurrentPrefix.Length + BodyLength;
        public override List<string> Prefixes => new List<string> { "LK" };

        public int BodyLength => 10;

        public override void Parse(string code)
        {
            code = PrepareCode(code);
            CheckCode(code);
            Address = GetCodeBody(code);
        }

        private string PrepareCode(string code)
        {
            if (!CheckPrefix(code))
            {
                code = CurrentPrefix + code;
            }
            return code;
        }

        public static bool TryParse(string code, out LocationCode locationCode)
        {
            locationCode = null;
            try
            {
                locationCode = new LocationCode(code);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}