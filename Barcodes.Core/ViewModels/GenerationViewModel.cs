using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.Models;
using Barcodes.Services.Generator;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class GenerationViewModel : BindableBase, ICloseSource
    {
        private string title = "Barcode Title";
        private bool edit;

        private GenerationDataViewModel generationData = new GenerationDataViewModel();
        private TemplateViewModel selectedTemplate;
        private ObservableCollection<TemplateViewModel> templates;

        private readonly IServicesContainer services;

        public GenerationViewModel(IServicesContainer services)
        {
            this.services = services;

            InitializeTemplates();
            LoadSettings();

            AddNewCommand = new DelegateCommand(() => GenerateBarcode(true));
            EditCommand = new DelegateCommand(() => GenerateBarcode(false), () => Edit);
            CancelCommand = new DelegateCommand(() => OnClose?.Invoke());
            UseTemplateCommand = new DelegateCommand(UseTemplate, () => TemplatesEnabled);
            DetectTemplateCommand = new DelegateCommand(DetectTemplate);
        }

        public DelegateCommand AddNewCommand { get; }
        public DelegateCommand EditCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand UseTemplateCommand { get; }
        public DelegateCommand DetectTemplateCommand { get; }

        public GenerationResult Result { get; private set; }

        public bool Edit
        {
            get => edit;
            set => SetProperty(ref edit, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public TemplateViewModel SelectedTemplate
        {
            get => selectedTemplate;
            set
            {
                SetProperty(ref selectedTemplate, value);
                TemplatesEnabled = value.Handler != null;
                UseTemplateCommand?.RaiseCanExecuteChanged();
            }
        }

        public bool TemplatesEnabled { get; set; }

        public ObservableCollection<TemplateViewModel> Templates
        {
            get => templates;
            set => SetProperty(ref templates, value);
        }

        public GenerationDataViewModel GenerationData
        {
            get => generationData;
            set => SetProperty(ref generationData, value);
        }

        public Action OnClose { get; set; }

        private void InitializeTemplates()
        {
            var factory = new TemplatesFactory();
            Templates = factory.GetAllTemplates(services.AppWindowsService);
            SelectedTemplate = Templates.First();
        }

        private bool GenerateValidation()
        {
            var title = Title.Trim();
            if (string.IsNullOrEmpty(title))
            {
                services.AppDialogsService.ShowError("Enter barcode's title");
                return false;
            }

            var data = GenerationData.Data.Trim();
            if (string.IsNullOrEmpty(data))
            {
                services.AppDialogsService.ShowError("Enter barcode's data");
                return false;
            }

            return true;
        }

        private void GenerateBarcode(bool addAsNew)
        {
            if (!GenerateValidation())
            {
                return;
            }

            try
            {
                Result = new GenerationResult
                {
                    Barcode = RunGenerator(GenerationData.ToData(), Title.Trim()),
                    AddNew = addAsNew
                };
                services.AppSettingsService.TryUpdateGenerationSettings(GenerationData.ToSettings());
                OnClose?.Invoke();
            }
            catch (Exception exc)
            {
                services.AppDialogsService.ShowException("Exception during barcode generation. Try disabling validation or adjust the barcode sizes", exc);
            }
        }

        private BarcodeViewModel RunGenerator(GenerationData barcodeData, string title)
        {
            var barcodeBitmap = services.GeneratorService.CreateBarcode(barcodeData);
            barcodeBitmap.Freeze();
            return new BarcodeViewModel(barcodeData)
            {
                Barcode = barcodeBitmap,
                Title = title
            };
        }

        private void UseTemplate()
        {
            if (SelectedTemplate == null || SelectedTemplate.Handler == null)
            {
                return;
            }

            var result = SelectedTemplate.Handler(GenerationData.Data);
            if (result != null)
            {
                GenerationData.Data = result.Data;
                if (result.BarcodeType.HasValue)
                {
                    GenerationData.SelectType(result.BarcodeType.Value);
                }
            }
        }

        public void Load(BarcodeViewModel barcode, bool edit, BarcodeTemplate? template)
        {
            if (barcode != null)
            {
                Title = barcode.Title;
                GenerationData.FromData(barcode.GenerationData);
            }

            Edit = edit;

            if (template.HasValue)
            {
                SelectTemplate(template.Value);
            }
        }

        private void SelectTemplate(BarcodeTemplate template)
        {
            var currentTemplate = Templates.SingleOrDefault(t => t.Template == template);
            if (currentTemplate != null)
            {
                SelectedTemplate = currentTemplate;
                UseTemplate();
            }
        }

        private void LoadSettings()
        {
            var generationSettings = services.AppSettingsService.AppSettings.GenerationSettings;
            GenerationData.FromSettings(generationSettings);
        }

        private void DetectTemplate()
        {
            var factory = new BarcodeTemplateFactory();
            var codePair = factory.GetCode(GenerationData.Data);
            if (codePair != null)
            {
                SelectTemplate(codePair.Template);
            }
            else
            {
                services.AppDialogsService.ShowError("No matching template found");
            }
        }
    }
}