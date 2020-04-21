using System.Windows.Input;

namespace Barcodes.Core.Common
{
    public interface IOnKeyDownAware
    {
        bool OnKeyDown(KeyEventArgs keyEventArgs);
    }
}