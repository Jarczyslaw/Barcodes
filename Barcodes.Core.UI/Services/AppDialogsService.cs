using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.Models;
using JToolbox.Desktop.Dialogs;
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

        public string OpenStorageFile(string currentFilePath = null)
        {
            string directoryPath = null;
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                directoryPath = Path.GetDirectoryName(currentFilePath);
            }
            return OpenFile("Barcodes storage file", directoryPath, new List<DialogFilterPair> { storageFilter });
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
            return OpenFile("Workspace file", null, new List<DialogFilterPair> { workspaceFilter });
        }

        public List<string> ImportWorkspaceFiles()
        {
            return OpenFiles("Workspace file", null, new List<DialogFilterPair> { workspaceFilter });
        }

        public string ExportWorkspaceFile(string workspaceName)
        {
            var fileName = $"{workspaceName}.{workspaceFilter.DisplayName}";
            return SaveFile("Workspace file", null, fileName, workspaceFilter);
        }

        public string ImportBarcodesFile()
        {
            return OpenFile("Barcodes file", null, new List<DialogFilterPair> { barcodeFilter });
        }

        public List<string> ImportBarcodesFiles()
        {
            return OpenFiles("Barcodes files", null, new List<DialogFilterPair> { barcodeFilter });
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

        public ExportMode ShowExportQuestion(bool addAllOption = true)
        {
            var buttons = new List<CustomButtonData<ExportMode>>
            {
                new CustomButtonData<ExportMode>
                {
                    Name = "CurrentWorkspaceButton",
                    Caption = "Current workspace",
                    Value = ExportMode.CurrentWorkspace
                },
                new CustomButtonData<ExportMode>
                {
                    Name = "CurrentWorkspaceButton",
                    Caption = "Selected barcodes",
                    Value = ExportMode.Selected
                },
                new CustomButtonData<ExportMode>
                {
                    Name = "CancelButton",
                    Caption = "Cancel",
                    Value = ExportMode.None
                }
            };
            if (addAllOption)
            {
                buttons.Insert(0, new CustomButtonData<ExportMode>
                {
                    Name = "AllButton",
                    Caption = "All",
                    Value = ExportMode.All
                });
            }
            return ShowCustomButtonsQuestion("Select barcodes source", buttons);
        }

        public SavingMode ShowSavingQuestion(IntPtr mainWindowHandle)
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
            return ShowCustomButtonsQuestion("Do you want to save current storage's changes?", buttons, mainWindowHandle);
        }

        public void ShowBarcodeGenerationException(Exception exc)
        {
            ShowException(exc, "Exception during barcode generation. Try disabling validation and adjust the barcode sizes");
        }
    }
}