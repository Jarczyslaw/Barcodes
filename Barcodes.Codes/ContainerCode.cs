using System;
using System.Collections.Generic;

namespace Barcodes.Codes
{
    public class ContainerCode : BaseCode
    {
        public ContainerCode()
        {
        }

        public ContainerCode(string code)
        {
            Parse(code);
        }

        public ContainerCode(int containerNumber)
        {
            CheckContainer(containerNumber);
            ContainerNumber = containerNumber;
        }

        public ContainerCode(ContainerCode code)
        {
            ContainerNumber = code.ContainerNumber;
        }

        public int ContainerNumber { get; private set; } = 1;

        public override string Code => CurrentPrefix + ContainerNumber.ToString().PadLeft(BodyLength, '0');

        public override BarcodeType Type => BarcodeType.Code128;
        public override int Length => 9;
        public override List<string> Prefixes => new List<string> { "PJ" };

        private int BodyLength => Length - PrefixLength;

        private void CheckContainer(int container)
        {
            if (container < 1 || container > MaxValue(BodyLength))
            {
                throw new ArgumentException("Invalid container number");
            }
        }

        public override void Parse(string code)
        {
            CheckCode(code);
            var body = GetCodeBody(code);
            CheckCodeOnlyDigits(body);
            ContainerNumber = int.Parse(body);
        }

        public static bool TryParse(string code, out ContainerCode containerCode)
        {
            containerCode = null;
            try
            {
                containerCode = new ContainerCode(code);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}