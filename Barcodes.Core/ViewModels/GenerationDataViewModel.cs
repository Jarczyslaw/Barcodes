using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Generator;
using Barcodes.Utils;
using JToolbox.Desktop.Core.Services;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class GenerationDataViewModel : GenerationBaseDataViewModel
    {
        private readonly IAppDialogsService appDialogsService;
        private readonly IAppWindowsService appWindowsService;
        private readonly IGeneratorService generatorService;
        private readonly ISystemService sysService;
        private readonly IAppSettingsService appSettingsService;

        private string title = "Barcode Title";
        private string data = "Barcode Data";
        private string description = string.Empty;

        private TemplateViewModel selectedTemplate;
        private ObservableCollection<TemplateViewModel> templates;

        public GenerationDataViewModel(IAppDialogsService appDialogsService, IAppWindowsService appWindowsService, IGeneratorService generatorService,
            ISystemService sysService, IAppSettingsService appSettingsService)
        {
            this.appDialogsService = appDialogsService;
            this.appWindowsService = appWindowsService;
            this.generatorService = generatorService;
            this.sysService = sysService;
            this.appSettingsService = appSettingsService;

            UseTemplateCommand = new DelegateCommand(UseTemplate, () => SelectedTemplate?.Handler != null);

            InitializeTemplates();
        }

        public DelegateCommand UseTemplateCommand { get; }
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

        public DelegateCommand RestoreSettingsCommand => new DelegateCommand(() => FromSettings(appSettingsService.AppSettings.GenerationSettings));

        public TemplateViewModel SelectedTemplate
        {
            get => selectedTemplate;
            set
            {
                SetProperty(ref selectedTemplate, value);
                UseTemplateCommand?.RaiseCanExecuteChanged();
            }
        }

        public object ParentViewModel { get; set; }

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

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public override GenerationData ToData()
        {
            var data = base.ToData();
            data.Data = Data.Trim();
            return data;
        }

        public override void FromData(GenerationData generationData)
        {
            Data = generationData.Data;
            base.FromData(generationData);
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

            var result = SelectedTemplate.Handler(ParentViewModel, Data);
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
                return new BarcodeViewModel(data)
                {
                    Barcode = barcodeBitmap,
                    Title = Title,
                    Description = Description
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