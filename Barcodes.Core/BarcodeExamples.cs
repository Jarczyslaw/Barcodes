using Barcodes.Core.ViewModels;
using Barcodes.Services.Generator;
using System.Collections.ObjectModel;

namespace Barcodes.Core
{
    public static class BarcodeExamples
    {
        public static ObservableCollection<BarcodeViewModel> CreateExamples(IGeneratorService generatorService)
        {
            return new ObservableCollection<BarcodeViewModel>
            {
                CreateProductBarcode(generatorService),
                CreateLocationBarcode(generatorService),
                CreateOrderBarcode(generatorService),
                CreateExternalReleaseBarcode(generatorService),
                CreateContainerBarcode(generatorService),
                CreateUserBarcode(generatorService),
                CreateLongProductBarcode(generatorService)
            };
        }

        private static BarcodeViewModel CreateProductBarcode(IGeneratorService generatorService)
        {
            var data = new GenerationData
            {
                Data = "5909991107123",
                ValidateCodeText = false,
                Type = BarcodeType.Ean13
            };

            return new BarcodeViewModel(data)
            {
                Title = "Kod produktu",
                Barcode = generatorService.CreateBarcode(data),
                Description = "Apap - 53627"
            };
        }

        private static BarcodeViewModel CreateLocationBarcode(IGeneratorService generatorService)
        {
            var data = new GenerationData
            {
                Data = "LK1234567890",
                ValidateCodeText = false,
                Type = BarcodeType.Code128
            };

            return new BarcodeViewModel(data)
            {
                Title = "Lokalizacja",
                Barcode = generatorService.CreateBarcode(data),
                Description = "LK + 10 cyfr"
            };
        }

        private static BarcodeViewModel CreateOrderBarcode(IGeneratorService generatorService)
        {
            var data = new GenerationData
            {
                Data = "DZ0000001230012718",
                ValidateCodeText = false,
                Type = BarcodeType.Code128
            };

            return new BarcodeViewModel(data)
            {
                Title = "Zlecenie",
                Barcode = generatorService.CreateBarcode(data),
                Description = "DZ + numer zlecenia"
            };
        }

        private static BarcodeViewModel CreateExternalReleaseBarcode(IGeneratorService generatorService)
        {
            var data = new GenerationData
            {
                Data = "WZ27922220010117",
                ValidateCodeText = false,
                Type = BarcodeType.Code128
            };

            return new BarcodeViewModel(data)
            {
                Title = "WZka",
                Barcode = generatorService.CreateBarcode(data),
                Description = "WZ + numer WZki"
            };
        }

        private static BarcodeViewModel CreateContainerBarcode(IGeneratorService generatorService)
        {
            var data = new GenerationData
            {
                Data = "PJ0002345",
                ValidateCodeText = false,
                Type = BarcodeType.Code128
            };

            return new BarcodeViewModel(data)
            {
                Title = "Pojemnik",
                Barcode = generatorService.CreateBarcode(data),
                Description = "PJ + numer pojemnika (7 cyfr)"
            };
        }

        private static BarcodeViewModel CreateUserBarcode(IGeneratorService generatorService)
        {
            var data = new GenerationData
            {
                Data = "PR09001",
                ValidateCodeText = false,
                Type = BarcodeType.Code128
            };

            return new BarcodeViewModel(data)
            {
                Title = "Użytkownik",
                Barcode = generatorService.CreateBarcode(data),
                Description = "PR + numer użytkownika (5 cyfr)"
            };
        }

        private static BarcodeViewModel CreateLongProductBarcode(IGeneratorService generatorService)
        {
            var data = new GenerationData
            {
                Data = "(02)05909991107123(17)200229(21)U1702011",
                ValidateCodeText = false,
                Type = BarcodeType.Ean128
            };

            return new BarcodeViewModel(data)
            {
                Title = "Długi kod produktu",
                Barcode = generatorService.CreateBarcode(data),
                Description = "(02) + 0 + EAN13 + (17) + data ważności (6 cyfr) + (21) + nr serii"
            };
        }
    }
}
