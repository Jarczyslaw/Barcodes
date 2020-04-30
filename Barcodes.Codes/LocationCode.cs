using System;

namespace Barcodes.Codes
{
    public class LocationCode
    {
        public string Prefix => "LK";
        public int AddressLength => 10;
        public string Code { get; private set; }
        public string Address { get; private set; }

        public LocationCode(string code)
        {
            Parse(code);
        }

        public LocationCode(LocationCode locationCode)
        {
            Code = locationCode.Code;
            Address = locationCode.Address;
        }

        public void Parse(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Code can not be empty");
            }

            if (!code.StartsWith(Prefix))
            {
                throw new ArgumentException("Invalid prefix");
            }

            if (code.Length != (AddressLength + Prefix.Length))
            {
                throw new ArgumentException("Invalid code length");
            }

            Address = code.Substring(Prefix.Length, AddressLength);
            Code = code;
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

        public override string ToString()
        {
            return Code;
        }
    }
}