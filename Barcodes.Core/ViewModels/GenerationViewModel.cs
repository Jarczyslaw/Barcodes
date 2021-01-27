using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Generator;
using Barcodes.Services.Sys;
using Prism.Commands;
using System;

namespace Barcodes.Core.ViewModels
{
    public class GenerationViewModel : BaseViewModel
    {
        private readonly IAppDialogsService appDialogsService;
        private readonly IAppSettingsService appSettingsService;

        private bool edit;
        private GenerationDataViewModel generationData;
        private BarcodeTemplate? initialTemplate;

        public GenerationViewModel(IAppDialogsService appDialogsService, IAppWindowsService appWindowsService, ISysService sysService,
            IAppSettingsService appSettingsService, IGeneratorService generatorService)
        {
            this.appDialogsService = appDialogsService;
            this.appSettingsService = appSettingsService;

            generationData = new GenerationDataViewModel(appDialogsService, appWindowsService, generatorService, sysService, appSettingsService)
            {
                ParentViewModel = this
            };
            LoadSettings();

            AddNewCommand = new DelegateCommand(() => GenerateBarcode(true));
            EditCommand = new DelegateCommand(() => GenerateBarcode(false), () => Edit);
        }

        public DelegateCommand AddNewCommand { get; }
        public DelegateCommand EditCommand { get; }

        public GenerationResult Result { get; private set; }

        public bool Edit
        {
            get => edit;
            set => SetProperty(ref edit, value);
        }

        public GenerationDataViewModel GenerationData
        {
            get => generationData;
            set => SetProperty(ref generationData, value);
        }

        private async void GenerateBarcode(bool addAsNew)
        {
            if (GenerationData.GenerateValidation())
            {
                try
                {
                    IsBusy = true;
                    Result = new GenerationResult
                    {
                        Barcode = await GenerationData.RunGenerator(),
                        AddNew = addAsNew
                    };
                    IsBusy = false;
                    appSettingsService.TryUpdateGenerationSettings(GenerationData.ToSettings());
                    OnClose?.Invoke();
                }
                catch (Exception exc)
                {
                    appDialogsService.ShowBarcodeGenerationException(exc);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public void Load(BarcodeViewModel barcode, bool edit, BarcodeTemplate? template)
        {
            if (barcode != null)
            {
                GenerationData.Title = barcode.Title;
                GenerationData.FromData(barcode.GenerationData);
            }

            Edit = edit;
            initialTemplate = template;
        }

        private void LoadSettings()
        {
            var generationSettings = appSettingsService.AppSettings.GenerationSettings;
            GenerationData.FromSettings(generationSettings);
        }

        public override void OnShow()
        {
            if (initialTemplate.HasValue)
            {
                GenerationData.SelectTemplate(initialTemplate.Value);
            }
        }
    }
}