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
        {
            CheckNumber(number);
            PaletteNumber = number;
        }

        public override string Code => CurrentPrefix + PaletteNumber.ToString();

        public override BarcodeType Type => BarcodeType.Code128;

        public override List<string> Prefixes => new List<string> { "PA", "PT" };

        public int PaletteNumber { get; private set; } = 1;

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
            CheckCodeOnlyDigits(body);
            PaletteNumber = int.Parse(body);
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