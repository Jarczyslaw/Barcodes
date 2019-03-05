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

            RenameCommand = new DelegateCommand(app.RenameWorkspace);
            DeleteCommand = new DelegateCommand(app.DeleteWorkspace);
            SetAsDefaultCommand = new DelegateCommand(app.SetWorkspaceAsDefault);
            MoveLeftCommand = new DelegateCommand(app.MoveWorkspaceLeft);
            MoveRightCommand = new DelegateCommand(app.MoveWorkspaceRight);
            ExportCommand = new DelegateCommand(app.ExportWorkspace);
            ClearCommand = new DelegateCommand(app.ClearWorkspace);
        }

        public DelegateCommand RenameCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand SetAsDefaultCommand { get; }
        public DelegateCommand MoveLeftCommand { get; }
        public DelegateCommand MoveRightCommand { get; }
        public DelegateCommand ClearCommand { get; }
        public DelegateCommand ExportCommand { get; }
    }
}
