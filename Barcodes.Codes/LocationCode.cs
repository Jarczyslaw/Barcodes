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
        public override int Length => 12;
        public override string Prefix => "LK";

        public void Parse(string code)
        {
            if (code?.StartsWith(Prefix) == false)
            {
                code = Prefix + code;
            }
            CheckCode(code);
            Address = GetCodeBody(code);
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