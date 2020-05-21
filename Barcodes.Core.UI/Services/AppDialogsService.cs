using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.Models;
using Barcodes.Services.Dialogs;
using System.Collections.Generic;
using System.IO;

namespace Barcodes.Core.UI.Services
{
    public class AppDialogsService : DialogsService, IAppDialogsService
    {
        private readonly DialogFilterPair storageFilter = new DialogFilterPair(FileExtensions.Storage);

        private readonly DialogFilterPair workspaceFilter = new DialogFilterPair(FileExtensions.Workspace);

        private readonly DialogFilterPair barcodeFilter = new DialogFilterPair(FileExtensions.Barcode);

        private readonly DialogFilterPair imageFilter = new DialogFilterPair("png");

        private readonly DialogFilterPair docFilter = new DialogFilterPair("pdf");

        public string OpenStorageFile(string currentFilePath, bool ensureFileExists)
        {
            var directoryPath = Path.GetDirectoryName(currentFilePath);
            return OpenFile("Barcodes storage file", directoryPath, ensureFileExists, storageFilter);
        }

        public string SaveStorageFile(string currentFilePath)
        {
            var fileName = Path.GetFileName(currentFilePath);
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"storage.{FileExtensions.Storage}";
            }

            var directoryPath = string.Empty;
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                directoryPath = Path.GetDirectoryName(currentFilePath);
            }
            return SaveFile("Barcodes storage file", directoryPath, fileName, storageFilter);
        }

        public string ImportWorkspaceFile()
        {
            return OpenFile("Workspace file", null, true, workspaceFilter);
        }

        public List<string> ImportWorkspaceFiles()
        {
            return OpenFiles("Workspace file", null, workspaceFilter);
        }

        public string ExportWorkspaceFile(string workspaceName)
        {
            var fileName = $"{workspaceName}.{workspaceFilter.DisplayName}";
            return SaveFile("Workspace file", null, fileName, workspaceFilter);
        }

        public string ImportBarcodeFile()
        {
            return OpenFile("Barcode file", null, true, barcodeFilter);
        }

        public List<string> ImportBarcodeFiles()
        {
            return OpenFiles("Barcode file", null, barcodeFilter);
        }

        public string ExportBarcodeFile(string barcodeTitle)
        {
            var fileName = $"{barcodeTitle}.{barcodeFilter.DisplayName}";
            return SaveFile("Barcode file", null, fileName, barcodeFilter);
        }

        public string SavePdfFile()
        {
            return SaveFile("Barcodes PDF file", null, $"barcodes.{docFilter.DisplayName}", docFilter);
        }

        public string SavePngFile(string defaultFileName)
        {
            return SaveFile("Barcodes image file", null, $"{defaultFileName}.{imageFilter.DisplayName}", imageFilter);
        }

        public PdfExportMode ShowPdfExportQuestion()
        {
            var buttons = new List<CustomButtonData<PdfExportMode>>
            {
                new CustomButtonData<PdfExportMode>
                {
                    Name = "AllButton",
                    Caption = "All",
                    Value = PdfExportMode.All
                },
                new CustomButtonData<PdfExportMode>
                {
                    Name = "CurrentWorkspaceButton",
                    Caption = "Current workspace",
                    Value = PdfExportMode.CurrentWorkspace
                },
                new CustomButtonData<PdfExportMode>
                {
                    Name = "CancelButton",
                    Caption = "Cancel",
                    Value = PdfExportMode.None
                }
            };
            return ShowCustomButtonsQuestion("Select barcodes source", buttons);
        }

        public SavingMode ShowSavingQuestion()
        {
            var buttons = new List<CustomButtonData<SavingMode>>
            {
                new CustomButtonData<SavingMode>
                {
                    Name = "SaveChangesButton",
                    Caption = "Save changes",
                    Value = SavingMode.SaveChanges
                },
                new CustomButtonData<SavingMode>
                {
                    Name = "DiscardChangesButton",
                    Caption = "Discard changes",
                    Value = SavingMode.DiscardChanges
                },
                new CustomButtonData<SavingMode>
                {
                    Name = "CancelButton",
                    Caption = "Cancel",
                    Value = SavingMode.Cancel
                }
            };
            return ShowCustomButtonsQuestion("Do you want to save current storage's changes?", buttons);
        }
    }
}
