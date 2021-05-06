using JToolbox.WPF.Core.Awareness.Args;
using System.Windows;

namespace JToolbox.WPF.UI.DragAndDrop
{
    public class UiFileDropArgs : FileDropArgs
    {
        public FrameworkElement Element { get; set; }
        public object Target { get; set; }
    }
}