namespace Barcodes.Codes
{
    public class LocationCode : BaseCode
    {
        public LocationCode(string codeOrAddress)
        {
            Parse(codeOrAddress);
        }

        public LocationCode(LocationCode locationCode)
        {
            Address = locationCode.Address;
        }

        public string Address { get; private set; }

        public override string Code => Prefix + Address;

        public override BarcodeType Type => BarcodeType.Code128;
        public override int Length => Prefix.Length + BodyLength;
        public override string Prefix => "LK";

        public int BodyLength => 10;

        public void Parse(string code)
        {
            code = PrepareCode(code);
            CheckCode(code);
            Address = GetCodeBody(code);
        }

        private string PrepareCode(string code)
        {
            if (code?.StartsWith(Prefix) == false)
            {
                code = Prefix + code;
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