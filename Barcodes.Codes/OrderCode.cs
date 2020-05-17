namespace Barcodes.Codes
{
    public class OrderCode : BaseOrderCode
    {
        public OrderCode()
        {
        }

        public OrderCode(string code)
            : base(code)
        {
        }

        public OrderCode(int orderId, int containerNumber, int divisionNumber, int year)
            : base(orderId, containerNumber, divisionNumber, year)
        {
        }

        public OrderCode(OrderCode orderCode)
            : base(orderCode)
        {
        }

        public override int OrderIdLength => 9;

        public override string Prefix => "DZ";

        public static bool TryParse(string code, out OrderCode orderCode)
        {
            orderCode = null;
            try
            {
                orderCode = new OrderCode(code);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}