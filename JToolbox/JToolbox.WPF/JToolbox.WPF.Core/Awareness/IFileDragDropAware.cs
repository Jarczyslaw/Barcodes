using JToolbox.WPF.Core.Awareness.Args;

namespace JToolbox.WPF.Core.Awareness
{
    public interface IFileDragDropAware
    {
        void OnFileDrag(FileDragArgs args);

        void OnFilesDrop(FileDropArgs args);
    }
}