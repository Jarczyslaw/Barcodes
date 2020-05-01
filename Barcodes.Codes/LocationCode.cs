namespace Barcodes.Codes
{
    public class LocationCode : BasePrefixCode
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

        protected override void Initialize()
        {
            Type = BarcodeType.Code128;
            Prefix = "LK";
            BodyLength = 10;
        }

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