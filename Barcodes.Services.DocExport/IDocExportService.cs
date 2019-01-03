using System.Collections.Generic;

namespace Barcodes.Services.DocExport
{
    public interface IDocExportService
    {
        void Export(List<DocBarcodeData> barcodes, string filePath);
    }
}
