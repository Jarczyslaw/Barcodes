﻿using System.Collections.Generic;

namespace Barcodes.Codes
{
    public class ReleaseDocumentCode : BaseOrderCode
    {
        public ReleaseDocumentCode()
        {
        }

        public ReleaseDocumentCode(string code)
            : base(code)
        {
        }

        public ReleaseDocumentCode(int orderId, int containerNumber, int divisionNumber, int year)
            : base(orderId, containerNumber, divisionNumber, year)
        {
        }

        public ReleaseDocumentCode(ReleaseDocumentCode orderCode)
            : base(orderCode)
        {
        }

        public override int OrderIdLength => 7;

        public override List<string> Prefixes => new List<string> { "WZ" };

        public static bool TryParse(string code, out ReleaseDocumentCode orderCode)
        {
            orderCode = null;
            try
            {
                orderCode = new ReleaseDocumentCode(code);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}