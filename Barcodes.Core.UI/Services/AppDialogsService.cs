using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.Models;
using Barcodes.Services.Dialogs;
using System;
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

        public string OpenStorageFile(string currentFilePath = null, bool ensureFileExists = true)
        {
            string directoryPath = null;
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                directoryPath = Path.GetDirectoryName(currentFilePath);
            }
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

        public string ImportBarcodesFile()
        {
            return OpenFile("Barcodes file", null, true, barcodeFilter);
        }

        public List<string> ImportBarcodesFiles()
        {
            return OpenFiles("Barcodes files", null, barcodeFilter);
        }

        public string ExportBarcodesFile()
        {
            var fileName = $"barcodes.{barcodeFilter.DisplayName}";
            return SaveFile("Barcodes file", null, fileName, barcodeFilter);
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
                    Name = "CurrentWorkspaceButton",
                    Caption = "Selected barcodes",
                    Value = PdfExportMode.Selected
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

        public void ShowBarcodeGenerationException(Exception exc)
        {
            ShowException("Exception during barcode generation. Try disabling validation and adjust the barcode sizes", exc);
        }
    }
}
