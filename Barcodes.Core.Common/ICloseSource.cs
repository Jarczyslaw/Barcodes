using System;

namespace Barcodes.Core.Common
{
    public interface ICloseSource
    {
        Action OnClose { get; set; }
    }
}
