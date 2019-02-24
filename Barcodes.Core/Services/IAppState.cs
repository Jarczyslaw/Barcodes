using Barcodes.Core.ViewModels;
using System.Collections.ObjectModel;

namespace Barcodes.Core.Services
{
    public interface IAppState
    {
        bool IsBusy { get; set; }
        string BusyMessage { get; set; }
        string StatusMessage { get; set; }
        int BarcodesCount { get; }
        bool IsEmpty { get; }

        WorkspaceViewModel SelectedWorkspace { get; set; }
        ObservableCollection<WorkspaceViewModel> Workspaces { get; set; }
        void AddWorkspace(WorkspaceViewModel workspace);

        void SetMessageAndCounter(string message);
    }
}
