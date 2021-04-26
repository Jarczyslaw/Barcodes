using JToolbox.WPF.Core.Awareness;
using JToolbox.WPF.Core.Awareness.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace JToolbox.WPF.UI.DragAndDrop
{

    public class UiDragDropArgs : DragDropArgs
    {
        public FrameworkElement SourceElement { get; set; }
        public FrameworkElement TargetElement { get; set; }
    }
}