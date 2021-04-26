using System;

namespace JToolbox.WPF.Core.Awareness
{
    public interface ICloseSource
    {
        Action OnClose { get; set; }
    }
}
