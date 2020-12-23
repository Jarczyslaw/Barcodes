using Barcodes.Codes;
using Barcodes.Core.ViewModels;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Generator;
using System.Collections.ObjectModel;

namespace Barcodes.Core
{
    public static class BarcodeExamples
    {
        private static (GenerationData, GenerationData) GetGenerationData(BarcodeType type, string data, IAppSettingsService appSettingsService)
        {
            var exampleGenerationData = new GenerationData
            {
                DefaultSize = true,
                ValidateCodeText = false,
                Type = type,
                Data = data
            };

            var generationSettings = appSettingsService.GenerationSettings;
            var settingsGenerationData = new GenerationData
            {
                DefaultSize = generationSettings.DefaultSize,
                ValidateCodeText = generationSettings.ValidateCodeText,
                Width = generationSettings.Width,
                Height = generationSettings.Height,
                Type = type,
                Data = data
            };
            return (exampleGenerationData, settingsGenerationData);
        }

        public static ObservableCollection<ExampleBarcodeViewModel> CreateExamples(IGeneratorService generatorService, IAppSettingsService appSettingsService)
        {
            return new ObservableCollection<ExampleBarcodeViewModel>
            {
                CreateProductBarcode(generatorService, appSettingsService),
                CreateLocationBarcode(generatorService, appSettingsService),
                CreateOrderBarcode(generatorService, appSettingsService),
                CreateExternalReleaseBarcode(generatorService, appSettingsService),
                CreateContainerBarcode(generatorService, appSettingsService),
                CreateUserBarcode(generatorService, appSettingsService),
                CreateLongProductBarcode(generatorService, appSettingsService),
                CreateStationBarcode(generatorService, appSettingsService),
                CreatePaletteBarcode(generatorService, appSettingsService),
                CreateAlleyBarcode(generatorService, appSettingsService)
            };
        }

        private static ExampleBarcodeViewModel CreateAlleyBarcode(IGeneratorService generatorService, IAppSettingsService appSettingsService)
        {
            var (exampleData, settingsData) = GetGenerationData(BarcodeType.Code128, "AL0000012345KA", appSettingsService);
            return new ExampleBarcodeViewModel(settingsData)
            {
                Template = BarcodeTemplate.Alley,
                Title = "Alejka",
                Barcode = generatorService.CreateBarcode(exampleData),
                Description = "AL + adres lokalizacji (10 znaków) + alejka (minimum 2 znaki)"
            };
        }

        private static ExampleBarcodeViewModel CreateProductBarcode(IGeneratorService generatorService, IAppSettingsService appSettingsService)
        {
            var (exampleData, settingsData) = GetGenerationData(BarcodeType.Ean13, "5909991107123", appSettingsService);
            return new ExampleBarcodeViewModel(settingsData)
            {
                Template = BarcodeTemplate.Product,
                Title = "Kod produktu",
                Barcode = generatorService.CreateBarcode(exampleData),
                Description = "Apap - 53627"
            };
        }

        private static ExampleBarcodeViewModel CreateLocationBarcode(IGeneratorService generatorService, IAppSettingsService appSettingsService)
        {
            var (exampleData, settingsData) = GetGenerationData(BarcodeType.Code128, "LK1234567890", appSettingsService);
            return new ExampleBarcodeViewModel(settingsData)
            {
                Template = BarcodeTemplate.Location,
                Title = "Lokalizacja",
                Barcode = generatorService.CreateBarcode(exampleData),
                Description = "LK + 10 znaków"
            };
        }

        private static ExampleBarcodeViewModel CreateOrderBarcode(IGeneratorService generatorService, IAppSettingsService appSettingsService)
        {
            var (exampleData, settingsData) = GetGenerationData(BarcodeType.Code128, "DZ0000001230012718", appSettingsService);
            return new ExampleBarcodeViewModel(settingsData)
            {
                Template = BarcodeTemplate.Order,
                Title = "Zlecenie",
                Barcode = generatorService.CreateBarcode(exampleData),
                Description = "DZ + numer zlecenia"
            };
        }

        private static ExampleBarcodeViewModel CreateExternalReleaseBarcode(IGeneratorService generatorService, IAppSettingsService appSettingsService)
        {
            var (exampleData, settingsData) = GetGenerationData(BarcodeType.Code128, "WZ27922220010117", appSettingsService);
            return new ExampleBarcodeViewModel(settingsData)
            {
                Template = BarcodeTemplate.ReleaseDocument,
                Title = "WZka",
                Barcode = generatorService.CreateBarcode(exampleData),
                Description = "WZ + numer WZki"
            };
        }

        private static ExampleBarcodeViewModel CreateContainerBarcode(IGeneratorService generatorService, IAppSettingsService appSettingsService)
        {
            var (exampleData, settingsData) = GetGenerationData(BarcodeType.Code128, "PJ0002345", appSettingsService);
            return new ExampleBarcodeViewModel(settingsData)
            {
                Template = BarcodeTemplate.Container,
                Title = "Pojemnik",
                Barcode = generatorService.CreateBarcode(exampleData),
                Description = "PJ + numer pojemnika (7 cyfr)"
            };
        }

        private static ExampleBarcodeViewModel CreateUserBarcode(IGeneratorService generatorService, IAppSettingsService appSettingsService)
        {
            var (exampleData, settingsData) = GetGenerationData(BarcodeType.Code128, "PR09001", appSettingsService);
            return new ExampleBarcodeViewModel(settingsData)
            {
                Template = BarcodeTemplate.User,
                Title = "Użytkownik",
                Barcode = generatorService.CreateBarcode(exampleData),
                Description = "PR + numer użytkownika (5 cyfr)"
            };
        }

        private static ExampleBarcodeViewModel CreateLongProductBarcode(IGeneratorService generatorService, IAppSettingsService appSettingsService)
        {
            var (exampleData, settingsData) = GetGenerationData(BarcodeType.Ean128, "(02)05909991107123(17)200229(10)U1702011", appSettingsService);
            return new ExampleBarcodeViewModel(settingsData)
            {
                Template = BarcodeTemplate.LongProduct,
                Title = "Długi kod produktu",
                Barcode = generatorService.CreateBarcode(exampleData),
                Description = "(02) + 0 + EAN13 + (17) + data ważności (6 cyfr) + (10) + nr serii"
            };
        }

        private static ExampleBarcodeViewModel CreateStationBarcode(IGeneratorService generatorService, IAppSettingsService appSettingsService)
        {
            var (exampleData, settingsData) = GetGenerationData(BarcodeType.Code128, "ST0101", appSettingsService);
            return new ExampleBarcodeViewModel(settingsData)
            {
                Template = BarcodeTemplate.WarehouseAndStation,
                Title = "Stacja (z magazynem)",
                Barcode = generatorService.CreateBarcode(exampleData),
                Description = "ST + numer magazynu (2 cyfry) + numer stacji (2 cyfry, 00 - bez stacji)"
            };
        }

        private static ExampleBarcodeViewModel CreatePaletteBarcode(IGeneratorService generatorService, IAppSettingsService appSettingsService)
        {
            var (exampleData, settingsData) = GetGenerationData(BarcodeType.Code128, "PA1234", appSettingsService);
            return new ExampleBarcodeViewModel(settingsData)
            {
                Template = BarcodeTemplate.Palette,
                Title = "Paleta",
                Barcode = generatorService.CreateBarcode(exampleData),
                Description = "PA/PT + numer palety"
            };
        }
    }
}