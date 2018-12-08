using Barcodes.Utils;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;

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
                CreateDocument(barcodes, filePath);
            }
            finally
            {
                Directory.Delete(tempImagesPath, true);
            }
        }

        private void CreateDocument(List<DocBarcodeData> barcodes, string filePath)
        {
            var builder = new DocBuilder();
            builder.AddTitle()
                .AddPageNumber();
            foreach (var barcode in barcodes)
                builder.AddBarcode(barcode);
            var document = builder.Render();
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
