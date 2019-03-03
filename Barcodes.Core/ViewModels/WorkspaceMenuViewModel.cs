using Prism.Commands;
using Prism.Mvvm;

namespace Barcodes.Core.ViewModels
{
    public class WorkspaceMenuViewModel : BindableBase
    {
        private readonly AppViewModel app;

        public WorkspaceMenuViewModel(AppViewModel app)
        {
            this.app = app;

            RenameWorkspaceCommand = new DelegateCommand(app.RenameWorkspace);
            DeleteWorkspaceCommand = new DelegateCommand(app.DeleteWorkspace);
            SetAsDefaultWorkspaceCommand = new DelegateCommand(app.SetAsDefaultWorkspace);
            MoveLeftCommand = new DelegateCommand(app.MoveWorkspaceLeft);
            MoveRightCommand = new DelegateCommand(app.MoveWorkspaceRight);
            ExportWorkspaceCommand = new DelegateCommand(app.ExportWorkspace);
        }

        public DelegateCommand RenameWorkspaceCommand { get; }
        public DelegateCommand DeleteWorkspaceCommand { get; }
        public DelegateCommand SetAsDefaultWorkspaceCommand { get; }
        public DelegateCommand MoveLeftCommand { get; }
        public DelegateCommand MoveRightCommand { get; }
        public DelegateCommand ClearWorkspaceCommand { get; }
        public DelegateCommand ExportWorkspaceCommand { get; }
    }
}
