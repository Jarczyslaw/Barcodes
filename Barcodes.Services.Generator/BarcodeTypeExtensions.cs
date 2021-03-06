﻿using Aspose.BarCode.Generation;
using Barcodes.Codes;
using System;
using System.Collections.Generic;

namespace Barcodes.Services.Generator
{
    public static class BarcodeTypeExtensions
    {
        public static Dictionary<BarcodeType, SymbologyEncodeType> CodesMap { get; set; } = new Dictionary<BarcodeType, SymbologyEncodeType>
        {
            { BarcodeType.Code128, EncodeTypes.Code128 },
            { BarcodeType.DataMatrix, EncodeTypes.DataMatrix },
            { BarcodeType.EAN128, EncodeTypes.GS1Code128 },
            { BarcodeType.EAN13, EncodeTypes.EAN13 },
            { BarcodeType.ITF14, EncodeTypes.ITF14 },
            { BarcodeType.QRCode, EncodeTypes.QR },
        };

        public static SymbologyEncodeType GetEncodeType(this BarcodeType barcodeType)
        {
            if (!CodesMap.ContainsKey(barcodeType))
            {
                throw new Exception("Invalid barcode type");
            }

            return CodesMap[barcodeType];
        }
    }
}