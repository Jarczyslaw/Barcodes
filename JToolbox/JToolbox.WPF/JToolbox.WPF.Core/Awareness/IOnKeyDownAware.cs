using System.Windows.Input;

namespace JToolbox.WPF.Core.Awareness
{
    public interface IOnKeyDownAware
    {
        void OnKeyDown(KeyEventArgs keyEventArgs);
    }
}