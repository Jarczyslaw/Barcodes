namespace Barcodes.Codes
{
    public class LocationCode : BasePrefixCode
    {
        public LocationCode(string code)
        {
            Initialize();
            Parse(code);
        }

        public LocationCode(LocationCode locationCode)
        {
            Initialize();
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
            CheckCode(code);
            var codeBody = GetCodeBody(code);
            CheckCodeOnlyDigits(codeBody);
            Address = codeBody;
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