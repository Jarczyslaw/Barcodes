namespace Barcodes.Codes
{
    public class UserCode : BasePrefixCode
    {
        public UserCode(string code)
        {
            Initialize();
            Parse(code);
        }

        public UserCode(UserCode code)
        {
            Initialize();
            User = code.User;
        }

        public override string Code => Prefix + User.ToString().PadLeft(5, '0');

        public int User { get; private set; }

        private void Parse(string code)
        {
            CheckCode(code);
            var body = GetCodeBody(code);
            CheckCodeOnlyDigits(body);
            User = int.Parse(body);
        }

        protected override void Initialize()
        {
            Type = BarcodeType.Code128;
            Prefix = "PR";
            BodyLength = 5;
        }

        public static bool TryParse(string code, out UserCode userCode)
        {
            userCode = null;
            try
            {
                userCode = new UserCode(code);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}