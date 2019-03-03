﻿using Barcodes.Services.Dialogs;
using System.Collections.Generic;
using System.IO;

namespace Barcodes.Core.Services
{
    public class AppDialogsService : DialogsService, IAppDialogsService
    {
        private readonly DialogFilterPair storageFilter = new DialogFilterPair("bcs");

        private readonly DialogFilterPair workspaceFilter = new DialogFilterPair("bcw");

        private readonly DialogFilterPair barcodeFilter = new DialogFilterPair("bcb");

        public string OpenStorageFile(string currentFilePath)
        {
            var directoryPath = Path.GetDirectoryName(currentFilePath);
            return OpenFile("Barcodes storage file", directoryPath, storageFilter);
        }

        public string SaveStorageFile(string currentFilePath)
        {
            var fileName = Path.GetFileName(currentFilePath);
            var directoryPath = Path.GetDirectoryName(currentFilePath);
            return SaveFile("Barcodes storage file", directoryPath, fileName, storageFilter);
        }

        public string ImportWorkspaceFile()
        {
            return OpenFile("Workspace file", null, workspaceFilter);
        }

        public string ExportWorkspaceFile(string workspaceName)
        {
            var fileName = $"{workspaceName}.{workspaceFilter.DisplayName}";
            return SaveFile("Workspace file", null, fileName, workspaceFilter);
        }

        public string ImportBarcodeFile()
        {
            return OpenFile("Barcode file", null, barcodeFilter);
        }

        public string ExportBarcodeFile(string barcodeTitle)
        {
            var fileName = $"{barcodeTitle}.{barcodeFilter.DisplayName}";
            return SaveFile("Barcode file", null, fileName, barcodeFilter);
        }

        public string SavePdfFile()
        {
            var filter = new DialogFilterPair("pdf");
            return SaveFile("Barcodes PDF file", null, "barcodes.pdf", filter);
        }

        public string SavePngFile(string defaultFileName)
        {
            var filter = new DialogFilterPair("png");
            return SaveFile("Barcodes image file", null, defaultFileName + ".png", filter);
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

        public ClosingMode ShowClosingQuestion()
        {
            var buttons = new List<CustomButtonData<ClosingMode>>
            {
                new CustomButtonData<ClosingMode>
                {
                    Name = "SaveChangesButton",
                    Caption = "Save changes",
                    Value = ClosingMode.SaveChanges
                },
                new CustomButtonData<ClosingMode>
                {
                    Name = "DiscardChangesButton",
                    Caption = "Discard changes",
                    Value = ClosingMode.DiscardChanges
                },
                new CustomButtonData<ClosingMode>
                {
                    Name = "CancelButton",
                    Caption = "Cancel",
                    Value = ClosingMode.Cancel
                }
            };
            return ShowCustomButtonsQuestion("Do you want to save current storage's changes?", buttons);
        }
    }
}
