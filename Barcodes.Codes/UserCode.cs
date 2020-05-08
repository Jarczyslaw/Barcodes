using System;

namespace Barcodes.Codes
{
    public class UserCode : BaseCode
    {
        public UserCode(string code)
        {
            Parse(code);
        }

        public UserCode(int user)
        {
            CheckUser(user);
            User = user;
        }

        public UserCode(UserCode code)
        {
            User = code.User;
        }

        public int User { get; private set; }

        public override string Code => Prefix + User.ToString().PadLeft(BodyLength, '0');

        public override BarcodeType Type => BarcodeType.Code128;
        public override int Length => 7;
        public override string Prefix => "PR";

        private int BodyLength => Length - PrefixLength;

        private void CheckUser(int user)
        {
            if (user < 0 || user > MaxValue(BodyLength))
            {
                throw new ArgumentException("Invalid user number");
            }
        }

        public void Parse(string code)
        {
            CheckCode(code);
            var body = GetCodeBody(code);
            CheckCodeOnlyDigits(body);
            User = int.Parse(body);
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