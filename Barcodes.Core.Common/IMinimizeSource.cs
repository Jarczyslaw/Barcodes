using System;

namespace Barcodes.Core.Common
{
    public interface IMinimizeSource
    {
        Action Minimize { get; set; }
    }
}