using System;
using System.Linq;

namespace Barcodes.Codes
{
    public abstract class BaseCode
    {
        public abstract string Code { get; }
        public abstract BarcodeType Type { get; }
        public virtual string Prefix => string.Empty;
        public int PrefixLength => Prefix.Length;
        public virtual int Length => 0;

        protected void CheckCodeOnlyDigits(string codeBody)
        {
            if (codeBody.Any(c => !char.IsDigit(c)))
            {
                throw new ArgumentException("Code must contain only digits");
            }
        }

        protected void CheckCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Code can not be empty");
            }

            if (!string.IsNullOrEmpty(Prefix) && !code.StartsWith(Prefix))
            {
                throw new ArgumentException("Invalid prefix");
            }

            if (Length != 0 && code.Length != Length)
            {
                throw new ArgumentException("Invalid code length");
            }
        }

        protected string GetCodeBody(string code)
        {
            return code.Substring(PrefixLength);
        }

        protected int MaxValue(int digits)
        {
            return (int)Math.Pow(10, digits) - 1;
        }

        protected void CheckValue(int value, int digits, string name)
        {
            var maxValue = MaxValue(digits);
            if (value < 0 || value > maxValue)
            {
                throw new ArgumentException($"Invalid value of {name} which is out of range [{0}, {maxValue}]");
            }
        }
    }
}