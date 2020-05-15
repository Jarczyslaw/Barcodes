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
        private string data = "Barcode Data";
        private string title = "Barcode Title";
        private bool defaultSize = true;
        private int width = 100;
        private int height = 100;
        private bool validateCodeText = true;
        private bool edit;

        private BarcodeTypeViewModel selectedBarcodeType;
        private ObservableCollection<BarcodeTypeViewModel> barcodeTypes;
        private TemplateViewModel selectedTemplate;
        private ObservableCollection<TemplateViewModel> templates;

        private readonly IServicesContainer services;

        public GenerationViewModel(IServicesContainer services)
        {
            this.services = services;

            InitializeBarcodeTypes();
            InitializeTemplates();
            LoadSettings();

            AddNewCommand = new DelegateCommand(() => GenerateBarcode(true));
            EditCommand = new DelegateCommand(() => GenerateBarcode(false), () => Edit);
            CancelCommand = new DelegateCommand(() => OnClose?.Invoke());
            UseTemplateCommand = new DelegateCommand(UseTemplate, () => TemplatesEnabled);
        }

        public DelegateCommand AddNewCommand { get; }
        public DelegateCommand EditCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand UseTemplateCommand { get; }

        public GenerationResult Result { get; private set; }

        public bool Edit
        {
            get => edit;
            set => SetProperty(ref edit, value);
        }

        public string Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public bool DefaultSize
        {
            get => defaultSize;
            set => SetProperty(ref defaultSize, value);
        }

        public int Width
        {
            get => width;
            set => SetProperty(ref width, value);
        }

        public int Height
        {
            get => height;
            set => SetProperty(ref height, value);
        }

        public bool ValidateCodeText
        {
            get => validateCodeText;
            set => SetProperty(ref validateCodeText, value);
        }

        public BarcodeTypeViewModel SelectedBarcodeType
        {
            get => selectedBarcodeType;
            set
            {
                SetProperty(ref selectedBarcodeType, value);
                UseTemplateCommand?.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<BarcodeTypeViewModel> BarcodeTypes
        {
            get => barcodeTypes;
            set => SetProperty(ref barcodeTypes, value);
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

        public Action OnClose { get; set; }

        private void InitializeTemplates()
        {
            var factory = new TemplatesFactory();
            Templates = factory.GetAllTemplates(services.AppWindowsService);
            SelectedTemplate = Templates.First();
        }

        private void InitializeBarcodeTypes()
        {
            BarcodeTypes = new ObservableCollection<BarcodeTypeViewModel>()
            {
                new BarcodeTypeViewModel(BarcodeType.Ean13),
                new BarcodeTypeViewModel(BarcodeType.Ean128),
                new BarcodeTypeViewModel(BarcodeType.Code128),
                new BarcodeTypeViewModel(BarcodeType.DataMatrix),
                new BarcodeTypeViewModel(BarcodeType.QRCode),
            };
        }

        private bool GenerateValidation()
        {
            var title = Title.Trim();
            if (string.IsNullOrEmpty(title))
            {
                services.AppDialogsService.ShowError("Enter barcode's title");
                return false;
            }

            var data = Data.Trim();
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

            var barcodeData = new GenerationData
            {
                Data = Data.Trim(),
                Type = SelectedBarcodeType.Type,
                DefaultSize = DefaultSize,
                Width = Width,
                Height = Height,
                ValidateCodeText = ValidateCodeText
            };

            try
            {
                Result = new GenerationResult
                {
                    Barcode = RunGenerator(barcodeData, Title.Trim()),
                    AddNew = addAsNew
                };
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

            var result = SelectedTemplate.Handler(Data);
            if (result != null)
            {
                Data = result.Data;
                if (result.BarcodeType.HasValue)
                {
                    SelectedBarcodeType = BarcodeTypes.FirstOrDefault(b => b.Type == result.BarcodeType.Value);
                }
            }
        }

        public void Load(BarcodeViewModel barcode, bool edit, Template? template)
        {
            if (barcode != null)
            {
                var generationData = barcode.GenerationData;
                Data = generationData.Data;
                SelectedBarcodeType = BarcodeTypes.FirstOrDefault(b => b.Type == generationData.Type);
                Title = barcode.Title;
                DefaultSize = generationData.DefaultSize;
                Width = generationData.Width;
                Height = generationData.Height;
                ValidateCodeText = generationData.ValidateCodeText;
            }

            Edit = edit;

            var initialTemplate = Templates.SingleOrDefault(t => t.Template == template);
            if (initialTemplate != null)
            {
                SelectedTemplate = initialTemplate;
                UseTemplate();
            }
        }

        private void LoadSettings()
        {
            var generationSettings = services.AppSettingsService.AppSettings.GenerationSettings;
            SelectedBarcodeType = BarcodeTypes.FirstOrDefault(b => b.Type == generationSettings.Type);
            DefaultSize = generationSettings.DefaultSize;
            Height = generationSettings.Height;
            Width = generationSettings.Width;
            ValidateCodeText = generationSettings.ValidateCodeText;
        }
    }
}