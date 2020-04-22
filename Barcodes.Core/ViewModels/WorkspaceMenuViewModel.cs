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

            RenameCommand = new DelegateCommand<WorkspaceViewModel>(w => app.RenameWorkspace(w));
            DeleteCommand = new DelegateCommand<WorkspaceViewModel>(app.DeleteWorkspace);
            SetAsDefaultCommand = new DelegateCommand<WorkspaceViewModel>(app.SetWorkspaceAsDefault);
            MoveLeftCommand = new DelegateCommand<WorkspaceViewModel>(app.MoveWorkspaceLeft);
            MoveRightCommand = new DelegateCommand<WorkspaceViewModel>(app.MoveWorkspaceRight);
            ExportCommand = new DelegateCommand<WorkspaceViewModel>(app.ExportWorkspace);
            ClearCommand = new DelegateCommand<WorkspaceViewModel>(app.ClearWorkspace);
            SetAsFirstCommand = new DelegateCommand<WorkspaceViewModel>(app.SetWorkspaceAsFirst);
            SetAsLastCommand = new DelegateCommand<WorkspaceViewModel>(app.SetWorkspaceAsLast);
        }

        public DelegateCommand<WorkspaceViewModel> RenameCommand { get; }
        public DelegateCommand<WorkspaceViewModel> DeleteCommand { get; }
        public DelegateCommand<WorkspaceViewModel> SetAsDefaultCommand { get; }
        public DelegateCommand<WorkspaceViewModel> MoveLeftCommand { get; }
        public DelegateCommand<WorkspaceViewModel> MoveRightCommand { get; }
        public DelegateCommand<WorkspaceViewModel> ClearCommand { get; }
        public DelegateCommand<WorkspaceViewModel> ExportCommand { get; }
        public DelegateCommand<WorkspaceViewModel> SetAsFirstCommand { get; }
        public DelegateCommand<WorkspaceViewModel> SetAsLastCommand { get; }
    }
}
