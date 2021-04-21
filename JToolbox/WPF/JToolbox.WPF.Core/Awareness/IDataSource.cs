using System;

namespace JToolbox.WPF.Core.Awareness
{
    public interface IDataSource
    {
        Action<object> OnData { get; set; }
    }
}