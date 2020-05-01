﻿using System;
using System.Linq;

namespace Barcodes.Codes
{
    public abstract class BasePrefixCode : BaseCode
    {
        public string Prefix { get; protected set; }
        public int PrefixLength => Prefix.Length;
        public int BodyLength { get; protected set; }

        protected abstract void Initialize();

        protected void CheckCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Code can not be empty");
            }

            if (!code.StartsWith(Prefix))
            {
                throw new ArgumentException("Invalid prefix");
            }

            if (BodyLength != 0 && code.Length != (BodyLength + Prefix.Length))
            {
                throw new ArgumentException("Invalid code length");
            }
        }

        protected string GetCodeBody(string code)
        {
            return code.Substring(Prefix.Length, BodyLength);
        }

        protected void CheckCodeOnlyDigits(string codeBody)
        {
            if (codeBody.Any(c => !char.IsDigit(c)))
            {
                throw new ArgumentException("Code must contain only digits");
            }
        }
    }
}