using System.Collections.Generic;

namespace JToolbox.WPF.Core.Awareness.Args
{
    public class FileDropArgs
    {
        public bool Handled { get; set; }
        public List<string> Files { get; set; }
        public object Target { get; set; }
    }
}