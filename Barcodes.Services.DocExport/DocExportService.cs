using Barcodes.Extensions;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering.Printing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barcodes.Services.DocExport
{
    public class DocExportService : IDocExportService
    {
        private readonly string tempImagesPath;

        public DocExportService()
        {
            tempImagesPath = Path.Combine(Path.GetTempPath(), "BarcodesTempImages");
        }

        public void Export(List<DocBarcodeData> barcodes, string filePath)
        {
            try
            {
                Directory.CreateDirectory(tempImagesPath);
                PrepareImages(barcodes);
                var pdfDocument = CreatePdfDocument(barcodes);
                SaveDocument(pdfDocument, filePath);
            }
            finally
            {
                Directory.Delete(tempImagesPath, true);
            }
        }

        public Task ExportAsync(List<DocBarcodeData> barcodes, string filePath)
        {
            return Task.Run(() => Export(barcodes, filePath));
        }

        public void Print(List<DocBarcodeData> barcodes)
        {
            var printerSettings = ShowPrintDialog();
            if (printerSettings != null)
            {
                StartPrinting(printerSettings, barcodes);
            }
        }

        public Task PrintAsync(List<DocBarcodeData> barcodes)
        {
            var printerSettings = ShowPrintDialog();
            if (printerSettings != null)
            {
                return Task.Run(() => StartPrinting(printerSettings, barcodes));
            }
            return Task.CompletedTask;
        }

        private void StartPrinting(PrinterSettings settings, List<DocBarcodeData> barcodes)
        {
            if (settings != null)
            {
                try
                {
                    Directory.CreateDirectory(tempImagesPath);
                    PrepareImages(barcodes);
                    var builder = PrepareDocument(barcodes);
                    PrintDocument(settings, builder.Document);
                }
                finally
                {
                    Directory.Delete(tempImagesPath, true);
                }
            }

        }

        private void PrintDocument(PrinterSettings settings, Document document)
        {
            var printDocument = new MigraDocPrintDocument(document);
            printDocument.PrinterSettings = settings;
            printDocument.Print();
        }

        private PrinterSettings ShowPrintDialog()
        {
            var printerSettings = new PrinterSettings();
            using (var dialog = new PrintDialog())
            {
                dialog.PrinterSettings = printerSettings;
                dialog.AllowSomePages = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return printerSettings;
                }
            }
            return null;
        }

        private DocBuilder PrepareDocument(List<DocBarcodeData> barcodes)
        {
            var builder = new DocBuilder();
            builder.AddTitle()
                .AddPageNumber();
            foreach (var barcode in barcodes)
            {
                builder.AddBarcode(barcode);
            }

            return builder;
        }

        private PdfDocument CreatePdfDocument(List<DocBarcodeData> barcodes)
        {
            var builder = PrepareDocument(barcodes);
            return builder.Render();
        }

        private void SaveDocument(PdfDocument document, string filePath)
        {
            document.Save(filePath);
            document.Close();
        }

        private void PrepareImages(List<DocBarcodeData> barcodes)
        {
            foreach (var barcode in barcodes)
            {
                var imagePath = Path.Combine(tempImagesPath, $"{Guid.NewGuid()}.bmp");
                barcode.Barcode.ToBmp(imagePath);
                barcode.TempFilePath = imagePath;
            }
        }
    }
}