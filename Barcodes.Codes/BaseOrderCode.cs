using System;
using System.Text.RegularExpressions;

namespace Barcodes.Codes
{
    public abstract class BaseOrderCode : BaseCode
    {
        protected BaseOrderCode(string code)
        {
            Parse(code);
        }

        protected BaseOrderCode(int orderId, int containerNumber, int divisionNumber, int year)
        {
            CheckMaxValue(orderId, OrderIdLength, "order number");
            CheckMaxValue(containerNumber, ContainerNumber, "container number");
            CheckMaxValue(divisionNumber, DivisionNumberLength, "division number");

            OrderId = orderId;
            ContainerNumber = containerNumber;
            DivisionNumber = divisionNumber;
            Year = year;
        }

        protected BaseOrderCode(BaseOrderCode orderCode)
        {
            OrderId = orderCode.OrderId;
            ContainerNumber = orderCode.ContainerNumber;
            DivisionNumber = orderCode.DivisionNumber;
            Year = orderCode.Year;
        }

        public int OrderId { get; private set; }
        public abstract int OrderIdLength { get; }

        public int ContainerNumber { get; private set; }
        public int ContainerNumberLength => 3;

        public int DivisionNumber { get; private set; }
        public int DivisionNumberLength => 2;

        public int Year { get; private set; }
        public int YearLength => 2;

        public override int Length => PrefixLength + OrderIdLength + ContainerNumberLength + DivisionNumberLength + YearLength;

        public override string Code => Prefix + Pad(OrderId, OrderIdLength)
            + Pad(ContainerNumber, ContainerNumberLength)
            + Pad(DivisionNumber, DivisionNumberLength)
            + Pad(Year % 100, YearLength);

        public override BarcodeType Type => BarcodeType.Code128;

        public void Parse(string code)
        {
            var match = Regex.Match(code, $@"{Prefix}(\d{OrderIdLength})(\d{ContainerNumberLength})(\d{DivisionNumberLength})(\d{YearLength})");
            if (!match.Success)
            {
                throw new ArgumentException("Invalid order code format");
            }

            OrderId = int.Parse(match.Groups[1].Value);
            ContainerNumber = int.Parse(match.Groups[2].Value);
            DivisionNumber = int.Parse(match.Groups[3].Value);
            Year = DateTime.Now.Year / 100 + int.Parse(match.Groups[4].Value);
        }

        private string Pad(int value, int length)
        {
            return value.ToString().PadLeft(length, '0');
        }
    }
}