using System;

namespace Barcodes.Codes
{
    public class ContainerCode : BaseCode
    {
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

        public int ContainerNumber { get; private set; }

        public override string Code => Prefix + ContainerNumber.ToString().PadLeft(BodyLength, '0');

        public override BarcodeType Type => BarcodeType.Code128;
        public override int Length => 7;
        public override string Prefix => "PJ";

        private int BodyLength => Length - PrefixLength;

        private void CheckContainer(int user)
        {
            if (user < 0 || user > MaxValue(BodyLength))
            {
                throw new ArgumentException("Invalid container number");
            }
        }

        public void Parse(string code)
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