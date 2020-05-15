﻿using System;

namespace Barcodes.Codes
{
    public class UserCode : BaseCode
    {
        public UserCode(string code)
        {
            Parse(code);
        }

        public UserCode(int number)
        {
            CheckUser(number);
            Number = number;
        }

        public UserCode(UserCode code)
        {
            Number = code.Number;
        }

        public int Number { get; private set; }

        public override string Code => Prefix + Number.ToString().PadLeft(BodyLength, '0');

        public override BarcodeType Type => BarcodeType.Code128;
        public override int Length => 7;
        public override string Prefix => "PR";

        private int BodyLength => Length - PrefixLength;

        private void CheckUser(int number)
        {
            if (number < 0 || number > MaxValue(BodyLength))
            {
                throw new ArgumentException("Invalid user number");
            }
        }

        public void Parse(string code)
        {
            CheckCode(code);
            var body = GetCodeBody(code);
            CheckCodeOnlyDigits(body);
            Number = int.Parse(body);
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