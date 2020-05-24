using System;
using System.Collections.Generic;
using System.Linq;

namespace Barcodes.Codes
{
    public abstract class BaseCode
    {
        protected string currentPrefix = string.Empty;

        public abstract string Code { get; }
        public abstract BarcodeType Type { get; }
        public virtual List<string> Prefixes => new List<string>();
        public int PrefixLength => 2;
        public virtual int Length => 0;

        public string CurrentPrefix
        {
            get
            {
                if (Prefixes.Count == 0)
                {
                    return string.Empty;
                }

                if (string.IsNullOrEmpty(currentPrefix))
                {
                    return Prefixes.First();
                }
                else
                {
                    return currentPrefix;
                }
            }
            set
            {
                if (!Prefixes.Contains(value))
                {
                    throw new ArgumentException("Invalid prefix value");
                }
                currentPrefix = value;
            }
        }

        public abstract void Parse(string code);

        protected void CheckCodeOnlyDigits(string codeBody)
        {
            if (codeBody.Any(c => !char.IsDigit(c)))
            {
                throw new ArgumentException("Code must contain only digits");
            }
        }

        protected bool CheckPrefix(string code)
        {
            if (Prefixes.Count > 0)
            {
                return Prefixes.Any(p => code?.StartsWith(p) == true);
            }
            return true;
        }

        protected void CheckCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Code can not be empty");
            }

            if (!CheckPrefix(code))
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
            var minValue = 1;
            var maxValue = MaxValue(digits);
            if (value < minValue || value > maxValue)
            {
                throw new ArgumentException($"Invalid value of {name} which is out of range [{minValue}, {maxValue}]");
            }
        }

        protected void CheckMinValue(int value, int min, string name)
        {
            if (value < min)
            {
                throw new ArgumentException($"Invalid value of {name} which is lower than {min}");
            }
        }
    }
}