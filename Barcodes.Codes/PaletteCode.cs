using System;
using System.Collections.Generic;

namespace Barcodes.Codes
{
    public class PaletteCode : BaseCode
    {
        public PaletteCode()
        {
        }

        public PaletteCode(string code)
        {
            Parse(code);
        }

        public PaletteCode(int number)
            : this(number, 0)
        {
        }

        public PaletteCode(int number, int length)
        {
            CheckNumber(number);
            PaletteNumberCode = number.ToString().PadLeft(length, '0');
        }

        public override string Code => CurrentPrefix + PaletteNumberCode;

        public override BarcodeType Type => BarcodeType.Code128;

        public override List<string> Prefixes => new List<string> { "PA", "PT" };

        public int PaletteNumber => int.Parse(PaletteNumberCode);
        public string PaletteNumberCode { get; private set; } = "1";

        private void CheckNumber(int number)
        {
            if (number < 1)
            {
                throw new ArgumentException("Invalid palette number");
            }
        }

        public override void Parse(string code)
        {
            CheckCode(code);
            var body = GetCodeBody(code);
            var prefix = GetCodePrefix(code);
            CheckCodeOnlyDigits(body);

            PaletteNumberCode = body;
            CurrentPrefix = prefix;
        }

        public static bool TryParse(string code, out PaletteCode paletteCode)
        {
            paletteCode = null;
            try
            {
                paletteCode = new PaletteCode(code);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}