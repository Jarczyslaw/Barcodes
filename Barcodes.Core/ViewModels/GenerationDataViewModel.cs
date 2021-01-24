using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Generator;
using Barcodes.Services.Sys;
using Barcodes.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class GenerationDataViewModel : BindableBase
    {
        private readonly IAppDialogsService appDialogsService;
        private readonly IAppWindowsService appWindowsService;
        private readonly IGeneratorService generatorService;
        private readonly ISysService sysService;
        private readonly IAppSettingsService appSettingsService;

        private string title = "Barcode Title";
        private string data = "Barcode Data";
        private bool defaultSize = true;
        private int width = 100;
        private int height = 100;
        private bool validateCodeText = true;

        private BarcodeTypeViewModel selectedType;
        private ObservableCollection<BarcodeTypeViewModel> types;
        private TemplateViewModel selectedTemplate;
        private ObservableCollection<TemplateViewModel> templates;

        public GenerationDataViewModel(IAppDialogsService appDialogsService, IAppWindowsService appWindowsService, IGeneratorService generatorService,
            ISysService sysService, IAppSettingsService appSettingsService)
        {
            this.appDialogsService = appDialogsService;
            this.appWindowsService = appWindowsService;
            this.generatorService = generatorService;
            this.sysService = sysService;
            this.appSettingsService = appSettingsService;

            InitializeBarcodeTypes();
            InitializeTemplates();
        }

        public DelegateCommand UseTemplateCommand => new DelegateCommand(UseTemplate, () => TemplatesEnabled);
        public DelegateCommand DetectTemplateCommand => new DelegateCommand(DetectTemplate);

        public DelegateCommand CopySettingsToClipboardCommand => new DelegateCommand(() => sysService.CopyToClipboard(Serializer.ToString(ToData())));

        public DelegateCommand PasteSettingsFromClipboardCommand => new DelegateCommand(() =>
        {
            try
            {
                var rawData = sysService.GetTextFromClipboard();
                FromData(Serializer.FromString<GenerationData>(rawData));
            }
            catch
            {
                appDialogsService.ShowError("Clipboard does not contain valid generation data");
            }
        });

        public DelegateCommand RestoreSettingsCommand => new DelegateCommand(() =>
        {
            var settings = appSettingsService.AppSettings.GenerationSettings;
            FromSettings(settings);
        });

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

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Data
        {
            get => data;
            set => SetProperty(ref data, value);
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

        public BarcodeTypeViewModel SelectedType
        {
            get => selectedType;
            set => SetProperty(ref selectedType, value);
        }

        public ObservableCollection<BarcodeTypeViewModel> Types
        {
            get => types;
            set => SetProperty(ref types, value);
        }

        private void InitializeBarcodeTypes()
        {
            Types = new ObservableCollection<BarcodeTypeViewModel>()
            {
                new BarcodeTypeViewModel(BarcodeType.Ean13),
                new BarcodeTypeViewModel(BarcodeType.Ean128),
                new BarcodeTypeViewModel(BarcodeType.Code128),
                new BarcodeTypeViewModel(BarcodeType.DataMatrix),
                new BarcodeTypeViewModel(BarcodeType.QRCode),
            };
        }

        public void SelectType(BarcodeType barcodeType)
        {
            SelectedType = Types.FirstOrDefault(b => b.Type == barcodeType);
        }

        public GenerationData ToData()
        {
            return new GenerationData
            {
                Data = Data.Trim(),
                Type = SelectedType.Type,
                DefaultSize = DefaultSize,
                Width = Width,
                Height = Height,
                ValidateCodeText = ValidateCodeText
            };
        }

        public void FromData(GenerationData generationData)
        {
            Data = generationData.Data;
            SelectType(generationData.Type);
            DefaultSize = generationData.DefaultSize;
            Width = generationData.Width;
            Height = generationData.Height;
            ValidateCodeText = generationData.ValidateCodeText;
        }

        public void FromSettings(GenerationSettings generationSettings)
        {
            SelectType(generationSettings.Type);
            DefaultSize = generationSettings.DefaultSize;
            Height = generationSettings.Height;
            Width = generationSettings.Width;
            ValidateCodeText = generationSettings.ValidateCodeText;
        }

        public GenerationSettings ToSettings()
        {
            return new GenerationSettings
            {
                Width = Width,
                DefaultSize = DefaultSize,
                Height = Height,
                Type = SelectedType.Type,
                ValidateCodeText = ValidateCodeText
            };
        }

        private void InitializeTemplates()
        {
            var factory = new TemplatesFactory();
            Templates = factory.GetAllTemplates(appWindowsService);
            SelectedTemplate = Templates.First();
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
                    SelectType(result.BarcodeType.Value);
                }
            }
        }

        public void SelectTemplate(BarcodeTemplate template)
        {
            var currentTemplate = Templates.SingleOrDefault(t => t.Template == template);
            if (currentTemplate != null)
            {
                SelectedTemplate = currentTemplate;
                UseTemplate();
            }
        }

        private void DetectTemplate()
        {
            var factory = new BarcodeTemplateFactory();
            var codePair = factory.GetCode(Data);
            if (codePair != null)
            {
                SelectTemplate(codePair.Template);
            }
            else
            {
                appDialogsService.ShowError("No matching template found");
            }
        }

        public async Task<BarcodeViewModel> RunGenerator()
        {
            return await Task.Run(() =>
            {
                var data = ToData();
                var barcodeBitmap = generatorService.CreateBarcode(data);
                barcodeBitmap.Freeze();
                return new BarcodeViewModel(data)
                {
                    Barcode = barcodeBitmap,
                    Title = Title
                };
            });
        }

        public bool GenerateValidation(bool validateTitle = true)
        {
            if (validateTitle)
            {
                var title = Title.Trim();
                if (string.IsNullOrEmpty(title))
                {
                    appDialogsService.ShowError("Enter barcode's title");
                    return false;
                }
            }

            var data = Data.Trim();
            if (string.IsNullOrEmpty(data))
            {
                appDialogsService.ShowError("Enter barcode's data");
                return false;
            }

            return true;
        }
    }
}