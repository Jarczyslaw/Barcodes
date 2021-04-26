using JToolbox.WPF.Core.Awareness.Args;
using System.Windows;

namespace JToolbox.WPF.UI.DragAndDrop
{
    public class UiFileDragArgs : FileDragArgs
    {
        public FrameworkElement Element { get; set; }
    }
}