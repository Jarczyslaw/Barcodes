using System;

namespace Barcodes.Core.Common
{
    public interface ICloseAware
    {
        Action OnClose { get; set; }
    }
}
