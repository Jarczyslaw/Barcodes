using Barcodes.Core.Abstraction;
using Prism.Commands;
using Prism.Mvvm;

namespace Barcodes.Core.ViewModels
{
    public class WorkspaceMenuViewModel : BindableBase
    {
        private readonly IServicesAggregator services;
        private readonly AppViewModel app;

        public WorkspaceMenuViewModel(AppViewModel app, IServicesAggregator services)
        {
            this.services = services;
            this.app = app;
        }

        public DelegateCommand<WorkspaceViewModel> RenameCommand => new DelegateCommand<WorkspaceViewModel>(w => app.RenameWorkspace(w));
        public DelegateCommand<WorkspaceViewModel> DeleteCommand => new DelegateCommand<WorkspaceViewModel>(app.DeleteWorkspace);
        public DelegateCommand<WorkspaceViewModel> MoveLeftCommand => new DelegateCommand<WorkspaceViewModel>(app.MoveWorkspaceLeft);
        public DelegateCommand<WorkspaceViewModel> MoveRightCommand => new DelegateCommand<WorkspaceViewModel>(app.MoveWorkspaceRight);
        public DelegateCommand<WorkspaceViewModel> ClearCommand => new DelegateCommand<WorkspaceViewModel>(app.ClearWorkspace);
        public DelegateCommand<WorkspaceViewModel> ExportCommand => new DelegateCommand<WorkspaceViewModel>(app.ExportWorkspace);
        public DelegateCommand<WorkspaceViewModel> SetAsFirstCommand => new DelegateCommand<WorkspaceViewModel>(app.SetWorkspaceAsFirst);
        public DelegateCommand<WorkspaceViewModel> SetAsLastCommand => new DelegateCommand<WorkspaceViewModel>(app.SetWorkspaceAsLast);
        public DelegateCommand<WorkspaceViewModel> OpenInNewWindowCommand => new DelegateCommand<WorkspaceViewModel>(app.OpenWorkspaceInNewWindowCommand);
    }
}