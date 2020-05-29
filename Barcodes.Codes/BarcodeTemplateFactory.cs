using System.Collections.Generic;

namespace Barcodes.Codes
{
    public class BarcodeTemplateFactory
    {
        private readonly List<CodePair> codePairs = new List<CodePair>
        {
            new CodePair(new ContainerCode(), BarcodeTemplate.Container),
            new CodePair(new LongProductCode(), BarcodeTemplate.LongProduct),
            new CodePair(new LocationCode(), BarcodeTemplate.Location),
            new CodePair(new NmvsProductCode(), BarcodeTemplate.NmvsProduct),
            new CodePair(new OrderCode(), BarcodeTemplate.Order),
            new CodePair(new ProductCode(), BarcodeTemplate.Product),
            new CodePair(new ReleaseDocumentCode(), BarcodeTemplate.ReleaseDocument),
            new CodePair(new UserCode(), BarcodeTemplate.User),
            new CodePair(new WarehouseAndStationCode(), BarcodeTemplate.WarehouseAndStation),
            new CodePair(new PaletteCode(), BarcodeTemplate.Palette),
            new CodePair(new AlleyCode(), BarcodeTemplate.Alley)
        };

        public CodePair GetCode(string codeString)
        {
            foreach (var codePair in codePairs)
            {
                try
                {
                    codePair.Code.Parse(codeString);
                    return codePair;
                }
                catch { }
            }
            return null;
        }

        public class CodePair
        {
            public CodePair(BaseCode code, BarcodeTemplate template)
            {
                Code = code;
                Template = template;
            }

            public BaseCode Code { get; }
            public BarcodeTemplate Template { get; }
        }
    }
}