using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barcodes.Services.DocExport
{
    public interface IDocExportService
    {
        void Export(List<DocBarcodeData> barcodes, string filePath);
        Task ExportAsync(List<DocBarcodeData> barcodes, string filePath);
        void Print(List<DocBarcodeData> barcodes);
        Task PrintAsync(List<DocBarcodeData> barcodes);
    }
}