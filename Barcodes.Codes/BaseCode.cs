﻿using System;
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
        public int PrefixLength => CurrentPrefix.Length;

        public string PrefixesString => string.Join(", ", Prefixes.ToArray());
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
                    throw new ArgumentException($"Invalid prefix value (valid prefixes: {PrefixesString})");
                }
                currentPrefix = value;
            }
        }

        public bool HasPrefix => Prefixes.Count > 0;

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

        protected void CheckCodeEmpty(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Code can not be empty");
            }
        }

        protected void CheckCode(string code)
        {
            CheckCodeEmpty(code);

            if (!CheckPrefix(code))
            {
                throw new ArgumentException($"Invalid prefix (valid prefixes: {PrefixesString})");
            }

            if (HasPrefix && code.Length < PrefixLength)
            {
                throw new ArgumentException($"Invalid code length (valid length: {Length})");
            }

            if (Length != 0 && code.Length != Length)
            {
                throw new ArgumentException($"Invalid code length (valid length: {Length})");
            }
        }

        protected string GetCodeBody(string code)
        {
            return code.Substring(PrefixLength);
        }

        protected string GetCodePrefix(string code)
        {
            return code.Substring(0, PrefixLength);
        }

        protected int MaxValue(int digits)
        {
            return (int)Math.Pow(10, digits) - 1;
        }

        protected void CheckValue(int value, int digits, string name, int minValue = 1)
        {
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

        protected string PrepareCodeWithPrefix(string code)
        {
            CheckCodeEmpty(code);
            if (!CheckPrefix(code))
            {
                code = CurrentPrefix + code;
            }
            return code;
        }
    }
}