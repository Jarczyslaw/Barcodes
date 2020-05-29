using Barcodes.Codes;
using Barcodes.Core.ViewModels;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Generator;
using System;
using System.Collections.ObjectModel;

namespace Barcodes.Core
{
    public static class BarcodeExamples
    {
        private static GenerationData GetBaseGenerationData(GenerationSettings generationSettings)
        {
            return new GenerationData
            {
                Width = generationSettings.Width,
                DefaultSize = generationSettings.DefaultSize,
                Height = generationSettings.Height,
            };
        }

        public static ObservableCollection<ExampleBarcodeViewModel> CreateExamples(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            return new ObservableCollection<ExampleBarcodeViewModel>
            {
                CreateProductBarcode(generatorService, generationSettings),
                CreateLocationBarcode(generatorService, generationSettings),
                CreateOrderBarcode(generatorService, generationSettings),
                CreateExternalReleaseBarcode(generatorService, generationSettings),
                CreateContainerBarcode(generatorService, generationSettings),
                CreateUserBarcode(generatorService, generationSettings),
                CreateLongProductBarcode(generatorService, generationSettings),
                CreateStationBarcode(generatorService, generationSettings),
                CreatePaletteBarcode(generatorService, generationSettings),
                CreateAlleyBarcode(generatorService, generationSettings)
            };
        }

        private static ExampleBarcodeViewModel CreateAlleyBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "AL0000012345KA";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new ExampleBarcodeViewModel(data)
            {
                Template = BarcodeTemplate.Alley,
                Title = "Alejka",
                Barcode = generatorService.CreateBarcode(data),
                Description = "AL + adres lokalizacji (10 cyfr) + alejka (minimum 2 znaki)"
            };
        }

        private static ExampleBarcodeViewModel CreateProductBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "5909991107123";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Ean13;

            return new ExampleBarcodeViewModel(data)
            {
                Template = BarcodeTemplate.Product,
                Title = "Kod produktu",
                Barcode = generatorService.CreateBarcode(data),
                Description = "Apap - 53627"
            };
        }

        private static ExampleBarcodeViewModel CreateLocationBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "LK1234567890";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new ExampleBarcodeViewModel(data)
            {
                Template = BarcodeTemplate.Location,
                Title = "Lokalizacja",
                Barcode = generatorService.CreateBarcode(data),
                Description = "LK + 10 cyfr"
            };
        }

        private static ExampleBarcodeViewModel CreateOrderBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "DZ0000001230012718";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new ExampleBarcodeViewModel(data)
            {
                Template = BarcodeTemplate.Order,
                Title = "Zlecenie",
                Barcode = generatorService.CreateBarcode(data),
                Description = "DZ + numer zlecenia"
            };
        }

        private static ExampleBarcodeViewModel CreateExternalReleaseBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "WZ27922220010117";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new ExampleBarcodeViewModel(data)
            {
                Template = BarcodeTemplate.ReleaseDocument,
                Title = "WZka",
                Barcode = generatorService.CreateBarcode(data),
                Description = "WZ + numer WZki"
            };
        }

        private static ExampleBarcodeViewModel CreateContainerBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "PJ0002345";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new ExampleBarcodeViewModel(data)
            {
                Template = BarcodeTemplate.Container,
                Title = "Pojemnik",
                Barcode = generatorService.CreateBarcode(data),
                Description = "PJ + numer pojemnika (7 cyfr)"
            };
        }

        private static ExampleBarcodeViewModel CreateUserBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "PR09001";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new ExampleBarcodeViewModel(data)
            {
                Template = BarcodeTemplate.User,
                Title = "Użytkownik",
                Barcode = generatorService.CreateBarcode(data),
                Description = "PR + numer użytkownika (5 cyfr)"
            };
        }

        private static ExampleBarcodeViewModel CreateLongProductBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "(02)05909991107123(17)200229(10)U1702011";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Ean128;

            return new ExampleBarcodeViewModel(data)
            {
                Template = BarcodeTemplate.LongProduct,
                Title = "Długi kod produktu",
                Barcode = generatorService.CreateBarcode(data),
                Description = "(02) + 0 + EAN13 + (17) + data ważności (6 cyfr) + (10) + nr serii"
            };
        }

        private static ExampleBarcodeViewModel CreateStationBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "ST0101";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new ExampleBarcodeViewModel(data)
            {
                Template = BarcodeTemplate.WarehouseAndStation,
                Title = "Stacja (z magazynem)",
                Barcode = generatorService.CreateBarcode(data),
                Description = "ST + numer magazynu (2 cyfry) + numer stacji (2 cyfry, 00 - bez stacji)"
            };
        }

        private static ExampleBarcodeViewModel CreatePaletteBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "PA1234";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new ExampleBarcodeViewModel(data)
            {
                Template = BarcodeTemplate.Palette,
                Title = "Paleta",
                Barcode = generatorService.CreateBarcode(data),
                Description = "PA/PT + numer palety"
            };
        }
    }
}