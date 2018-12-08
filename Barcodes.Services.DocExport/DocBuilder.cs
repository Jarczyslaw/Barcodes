using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System;

namespace Barcodes.Services.DocExport
{
    public class DocBuilder
    {
        public Document Document { get; private set; }
        private Section section;

        public DocBuilder()
        {
            InitializeDocument();
        }

        public PdfDocument Render()
        {
            var renderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always)
            {
                Document = Document
            };
            renderer.RenderDocument();
            return renderer.PdfDocument;
        }

        public DocBuilder AddBarcode(DocBarcodeData barcodeData)
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.KeepTogether = true;
            paragraph.Format.SpaceAfter = Unit.FromCentimeter(0.75d);
            paragraph.AddText($"Title: {barcodeData.Title}");
            AddLineBreaks(paragraph);
            paragraph.AddText($"Data: {barcodeData.Data}");
            AddLineBreaks(paragraph, 2);
            paragraph.AddImage(barcodeData.TempFilePath);
            return this;
        }

        public DocBuilder AddTitle()
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Font.Size = 16;
            paragraph.Format.SpaceAfter = Unit.FromCentimeter(1d);
            paragraph.AddText($"Barcodes generated at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            return this;
        }

        public DocBuilder AddPageNumber()
        {
            var paragraph = new Paragraph();
            paragraph.AddText("Page ");
            paragraph.AddPageField();
            paragraph.AddText(" of ");
            paragraph.AddNumPagesField();
            section.Footers.Primary.Add(paragraph);
            return this;
        }

        private void AddLineBreaks(Paragraph paragraph, int count = 1)
        {
            for (int i = 0; i < count; i++)
                paragraph.AddLineBreak();
        }

        private void InitializeDocument()
        {
            Document = new Document();
            Document.Info.Title =
                Document.Info.Subject =
                Document.Info.Author = "Barcodes";

            var pageSetup = Document.DefaultPageSetup.Clone();
            var margin = 1d;
            pageSetup.TopMargin = Unit.FromCentimeter(margin);
            pageSetup.BottomMargin = Unit.FromCentimeter(margin);
            pageSetup.LeftMargin = Unit.FromCentimeter(margin);
            pageSetup.RightMargin = Unit.FromCentimeter(margin);

            section = Document.AddSection();
            section.PageSetup = pageSetup;
        }
    }
}
