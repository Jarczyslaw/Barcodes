using JToolbox.WPF.Core.Awareness.Args;

namespace JToolbox.WPF.Core.Awareness
{
    public interface IDragDropAware
    {
        void OnDrag(DragDropArgs args);
        void OnDrop(DragDropArgs args);
    }
}