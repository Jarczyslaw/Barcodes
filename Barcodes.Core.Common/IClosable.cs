using System;

namespace Barcodes.Core.Common
{
    public interface IClosable
    {
        Action CloseAction { get; set; }
    }
}
