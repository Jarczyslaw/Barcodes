﻿using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.Models;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Generator;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class ExamplesViewModel : BindableBase, ICloseSource
    {
        private readonly IDialogsService dialogsService;
        private readonly IGeneratorService generatorService;
        private readonly IAppWindowsService appWindowsService;
        private readonly IAppSettingsService appSettingsService;
        private ObservableCollection<ExampleBarcodeViewModel> barcodes;
        private ExampleBarcodeViewModel selectedBarcode;

        public ExamplesViewModel(IGeneratorService generatorService, IDialogsService dialogsService,
            IAppWindowsService appWindowsService, IAppSettingsService appSettingsService)
        {
            this.dialogsService = dialogsService;
            this.generatorService = generatorService;
            this.appWindowsService = appWindowsService;
            this.appSettingsService = appSettingsService;
        }

        public DelegateCommand GenerateCommand => new DelegateCommand(() =>
        {
            if (SelectedBarcode == null)
            {
                dialogsService.ShowInfo("Select example barcode");
                return;
            }

            var result = appWindowsService.ShowGenerationWindow(SelectedBarcode, false, SelectedBarcode.Template);
            if (result != null)
            {
                GenerationResult = result;
                OnClose?.Invoke();
            }
        });

        public DelegateCommand CloseCommand => new DelegateCommand(() => OnClose?.Invoke());

        public GenerationResult GenerationResult { get; private set; }

        public ObservableCollection<ExampleBarcodeViewModel> Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }

        public ExampleBarcodeViewModel SelectedBarcode
        {
            get => selectedBarcode;
            set => SetProperty(ref selectedBarcode, value);
        }

        public Action OnClose { get; set; }

        public Task CreateExamples()
        {
            return Task.Run(() => Barcodes = BarcodeExamples.CreateExamples(generatorService, appSettingsService));
        }
    }
}