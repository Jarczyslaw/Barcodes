using System;
using System.Linq;

namespace Barcodes.Codes
{
    public abstract class BaseCode
    {
        protected BaseCode()
        {
            Initialize();
        }

        public virtual string Code { get; }
        public BarcodeType Type { get; protected set; }

        protected void CheckCodeOnlyDigits(string codeBody)
        {
            if (codeBody.Any(c => !char.IsDigit(c)))
            {
                throw new ArgumentException("Code must contain only digits");
            }
        }

        protected int MaxValue(int digits)
        {
            return (int)Math.Pow(10, digits) - 1;
        }

        protected abstract void Initialize();
    }
}