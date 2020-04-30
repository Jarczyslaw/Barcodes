﻿using Barcodes.Codes;
using Barcodes.Core.ViewModels;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Generator;
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

        public static ObservableCollection<BarcodeViewModel> CreateExamples(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            return new ObservableCollection<BarcodeViewModel>
            {
                CreateProductBarcode(generatorService, generationSettings),
                CreateLocationBarcode(generatorService, generationSettings),
                CreateOrderBarcode(generatorService, generationSettings),
                CreateExternalReleaseBarcode(generatorService, generationSettings),
                CreateContainerBarcode(generatorService, generationSettings),
                CreateUserBarcode(generatorService, generationSettings),
                CreateLongProductBarcode(generatorService, generationSettings),
                CreateStationBarcode(generatorService, generationSettings)
            };
        }

        private static BarcodeViewModel CreateProductBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "5909991107123";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Ean13;

            return new BarcodeViewModel(data)
            {
                Title = "Kod produktu",
                Barcode = generatorService.CreateBarcode(data),
                Description = "Apap - 53627"
            };
        }

        private static BarcodeViewModel CreateLocationBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "LK1234567890";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new BarcodeViewModel(data)
            {
                Title = "Lokalizacja",
                Barcode = generatorService.CreateBarcode(data),
                Description = "LK + 10 cyfr"
            };
        }

        private static BarcodeViewModel CreateOrderBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "DZ0000001230012718";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new BarcodeViewModel(data)
            {
                Title = "Zlecenie",
                Barcode = generatorService.CreateBarcode(data),
                Description = "DZ + numer zlecenia"
            };
        }

        private static BarcodeViewModel CreateExternalReleaseBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "WZ27922220010117";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new BarcodeViewModel(data)
            {
                Title = "WZka",
                Barcode = generatorService.CreateBarcode(data),
                Description = "WZ + numer WZki"
            };
        }

        private static BarcodeViewModel CreateContainerBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "PJ0002345";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new BarcodeViewModel(data)
            {
                Title = "Pojemnik",
                Barcode = generatorService.CreateBarcode(data),
                Description = "PJ + numer pojemnika (7 cyfr)"
            };
        }

        private static BarcodeViewModel CreateUserBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "PR09001";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new BarcodeViewModel(data)
            {
                Title = "Użytkownik",
                Barcode = generatorService.CreateBarcode(data),
                Description = "PR + numer użytkownika (5 cyfr)"
            };
        }

        private static BarcodeViewModel CreateLongProductBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "(02)05909991107123(17)200229(10)U1702011";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Ean128;

            return new BarcodeViewModel(data)
            {
                Title = "Długi kod produktu",
                Barcode = generatorService.CreateBarcode(data),
                Description = "(02) + 0 + EAN13 + (17) + data ważności (6 cyfr) + (10) + nr serii"
            };
        }

        private static BarcodeViewModel CreateStationBarcode(IGeneratorService generatorService, GenerationSettings generationSettings)
        {
            var data = GetBaseGenerationData(generationSettings);

            data.Data = "ST0101";
            data.ValidateCodeText = false;
            data.Type = BarcodeType.Code128;

            return new BarcodeViewModel(data)
            {
                Title = "Stacja (z magazynem)",
                Barcode = generatorService.CreateBarcode(data),
                Description = "ST + numer magazynu (2 cyfry) + numer stacji (2 cyfry, 00 - bez stacji)"
            };
        }
    }
}