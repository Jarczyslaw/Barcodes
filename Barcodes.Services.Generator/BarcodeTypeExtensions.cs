using Aspose.BarCode.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Services.Generator
{
    public static class BarcodeTypeExtensions
    {
        public static SymbologyEncodeType GetEncodeType(this BarcodeType barcodeType)
        {
            switch(barcodeType)
            {
                case BarcodeType.Code128:
                    return EncodeTypes.Code128;
                case BarcodeType.DataMatrix:
                    return EncodeTypes.DataMatrix;
                case BarcodeType.Ean128:
                    return EncodeTypes.GS1Code128;
                case BarcodeType.Ean13:
                    return EncodeTypes.EAN13;
                case BarcodeType.QRCode:
                    return EncodeTypes.QR;
                default:
                    throw new Exception("Invalid barcode type");
            }
        }
    }
}
