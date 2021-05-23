using Barcodes.Core.Common;
using JToolbox.Core.Extensions;
using JToolbox.WPF.Core.Awareness.Args;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class AppDragDropViewModel
    {
        private readonly AppViewModel app;

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

        public void OnFileDrag(FileDragArgs args)
        {
            if (args.Source is BarcodeViewModel barcode)
            {
                var path = Path.Combine(Path.GetTempPath(), $"{barcode.Title}.{FileExtensions.Barcode}");
                args.Files = new List<string> { path };
                app.ExportBarcodes(path, new List<BarcodeViewModel> { barcode });
            }
            else if (args.Source is WorkspaceViewModel workspace)
            {
                var path = Path.Combine(Path.GetTempPath(), $"{workspace.Name}.{FileExtensions.Workspace}");
                args.Files = new List<string> { path };
                app.ExportWorkspace(path, workspace);
            }
        }

        public void OnFilesDrop(FileDropArgs args)
        {
            if (args.Files?.Count > 0)
            {
                var barcodeFiles = args.Files
                    .Where(f => CheckExtension(f, FileExtensions.Barcode))
                    .ToList();
                var workspaceFiles = args.Files
                    .Where(f => CheckExtension(f, FileExtensions.Workspace))
                    .ToList();

                if (barcodeFiles.Count > 0)
                {
                    app.ImportBarcodes(barcodeFiles);
                }

                if (workspaceFiles.Count > 0)
                {
                    app.ImportWorkspaces(workspaceFiles);
                }
            }
        }

        private bool CheckExtension(string file, string targetExtension)
        {
            var extension = Path.GetExtension(file);
            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }
            if (extension.StartsWith(".") && extension.Length > 1)
            {
                extension = extension.Substring(1, extension.Length - 1);
            }
            return extension.IgnoreCaseEquals(targetExtension);
        }
    }
}