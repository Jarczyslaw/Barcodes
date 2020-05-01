namespace Barcodes.Codes
{
    public class BaseCode
    {
        public virtual string Code { get; }
        public BarcodeType Type { get; protected set; }
    }
}