using JToolbox.WPF.Core.Awareness.Args;
using System.Collections.Generic;
using System.Linq;
using JToolbox.Core.Extensions;

namespace Barcodes.Core.ViewModels
{
    public class AppDragDropViewModel
    {
        private AppViewModel app;

        public AppDragDropViewModel(AppViewModel app)
        {
            this.app = app;
        }

        public void OnDrag(DragDropArgs args)
        {
        }

        public void OnDrop(DragDropArgs args)
        {
            if (args.Source is WorkspaceViewModel sourceWorkspace)
            {
                if (args.Target == app)
                {
                    app.SetWorkspaceAsLast(sourceWorkspace);
                }
                else if (args.Target is WorkspaceViewModel targetWorkspace)
                {
                    MoveWorkspace(sourceWorkspace, targetWorkspace);
                }
            }
            else if (args.Source is BarcodeViewModel sourceBarcode)
            {
                if (args.Target is BarcodeViewModel targetBarcode)
                {
                    MoveBarcode(sourceBarcode, targetBarcode);
                }
                else if (args.Target is WorkspaceViewModel targetWorkspace)
                {
                    var sourceBarcodeWorkspace = GetWorkspaceByBarcode(sourceBarcode);
                    if (sourceBarcodeWorkspace == targetWorkspace)
                    {
                        sourceBarcodeWorkspace.Barcodes.SetAsLast(sourceBarcode);
                    }
                    else
                    {
                        app.MoveBarcodesToWorkspace(new List<BarcodeViewModel> { sourceBarcode }, sourceBarcodeWorkspace, targetWorkspace);
                    }
                }
            }
            args.Handled = true;
        }

        private void MoveWorkspace(WorkspaceViewModel source, WorkspaceViewModel target)
        {
            var workspaces = app.Workspaces;
            workspaces.Move(workspaces.IndexOf(source), workspaces.IndexOf(target));
            app.SelectedWorkspace = source;
        }

        private void MoveBarcode(BarcodeViewModel source, BarcodeViewModel target)
        {
            var workspace = GetWorkspaceByBarcode(source);
            var barcodes = workspace.Barcodes;
            barcodes.Move(barcodes.IndexOf(source), barcodes.IndexOf(target));
        }

        private WorkspaceViewModel GetWorkspaceByBarcode(BarcodeViewModel barcode)
        {
            return app.Workspaces.FirstOrDefault(w => w.Barcodes.Contains(barcode));
        }
    }
}