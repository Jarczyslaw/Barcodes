using Barcodes.Core.Events;
using Barcodes.Core.Services;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Utils;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private BitmapSource randomBarcode;
        public BitmapSource RandomBarcode
        {
            get => randomBarcode;
            set => SetProperty(ref randomBarcode, value);
        }

        public BarcodeTypeViewModel SelectedBarcodeType
        {
            get => Barcodes.SelectedBarcodeType;
            set
            {
                Barcodes.SelectedBarcodeType = value;
                RaisePropertyChanged();
                ExtraInputCommand.RaiseCanExecuteChanged();
            }
        }

        private BarcodesViewModel barcodes;
        public BarcodesViewModel Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }

        private readonly IBarcodesGeneratorService barcodesGenerator;
        private readonly IDialogsService dialogsService;
        private readonly IBarcodeWindowsService barcodeWindowsService;
        private readonly IAppSettingsService appSettingsService;
        private readonly IBarcodeStorageService barcodeStorageService;

        public ShellViewModel(IBarcodesGeneratorService barcodesGenerator, IDialogsService dialogsService, 
            IBarcodeWindowsService barcodeWindowsService, IEventAggregator eventAggregator, 
            IAppSettingsService appSettingsService, IBarcodeStorageService barcodeStorageService)
        {
            this.barcodesGenerator = barcodesGenerator;
            this.dialogsService = dialogsService;
            this.barcodeWindowsService = barcodeWindowsService;
            this.appSettingsService = appSettingsService;
            this.barcodeStorageService = barcodeStorageService;

            Barcodes = new BarcodesViewModel(barcodesGenerator, dialogsService, barcodeWindowsService);

            GenerateRandomBarcodeCommand = new DelegateCommand(GenerateRandomBarcode);
            GenerateBarcodeCommand = new DelegateCommand(Barcodes.GenerateBarcode);
            ExtraInputCommand = new DelegateCommand(Barcodes.ExtraInput, () => Barcodes.ExtraInputEnabled);
            OpenInNewWindowCommand = new DelegateCommand(OpenInNewWindow);
            CopyToClipboardCommand = new DelegateCommand<BarcodeResultViewModel>(CopyToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeResultViewModel>(Barcodes.Delete);

            SaveToFileCommand = new DelegateCommand(SaveBarcodesToFile);
            LoadFromFileCommand = new DelegateCommand(LoadBarcodesFromFile);
            OpenStorageLocationCommand = new DelegateCommand(OpenStorageLocation);
            CloseCommand = new DelegateCommand(() => eventAggregator.GetEvent<CloseEvent>().Publish());
            ExportToPdfCommand = new DelegateCommand(ExportToPdf);
            ShowHelpCommand = new DelegateCommand(ShowHelp);

            GenerateRandomBarcode();

            LoadBarcodesFromFile(appSettingsService.StoragePath);
        }

        public DelegateCommand SaveToFileCommand { get; private set; }
        public DelegateCommand LoadFromFileCommand { get; private set; }
        public DelegateCommand OpenStorageLocationCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand ExportToPdfCommand { get; private set; }
        public DelegateCommand ShowHelpCommand { get; private set; }

        public DelegateCommand GenerateRandomBarcodeCommand { get; private set; }
        public DelegateCommand GenerateBarcodeCommand { get; private set; }
        public DelegateCommand ExtraInputCommand { get; private set; }
        public DelegateCommand OpenInNewWindowCommand { get; private set; }
        public DelegateCommand<BarcodeResultViewModel> CopyToClipboardCommand { get; private set; }
        public DelegateCommand<BarcodeResultViewModel> DeleteCommand { get; private set; }

        private void GenerateRandomBarcode()
        {
            var randomText = RandomTexts.Get();
            RandomBarcode = barcodesGenerator.CreateShellBarcode(400, randomText);
        }

        private void OpenInNewWindow()
        {
            if (Barcodes.SelectedBarcode == null)
                return;

            barcodeWindowsService.OpenBarcodeWindow(Barcodes.SelectedBarcode);
        }

        private void CopyToClipboard(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
                return;

            Clipboard.SetImage(barcode.Barcode);
            Barcodes.StatusMessage = $"Barcode \"{barcode.Title}\" copied to clipboard";
        }

        private void ShowHelp()
        {
            throw new NotImplementedException();
        }

        private void ExportToPdf()
        {
            throw new NotImplementedException();
        }

        private void OpenStorageLocation()
        {
            try
            {
                string argument = "/select, \"" + appSettingsService.StoragePath + "\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
            catch (Exception exc)
            {
                dialogsService.ShowException("Can not open storage file location", exc);
            }
        }

        private void LoadBarcodesFromFile()
        {
            var directoryPath = Path.GetDirectoryName(appSettingsService.StoragePath);
            var filePath = dialogsService.OpenFile("Barcodes storage file", directoryPath, new DialogFilterPair { DisplayName = "json", ExtensionsList = "json" });
            if (string.IsNullOrEmpty(filePath))
                return;

            LoadBarcodesFromFile(filePath);
        }

        private void LoadBarcodesFromFile(string storagePath)
        {
            try
            {
                var barcodes = barcodeStorageService.Load(storagePath, false);
                if (barcodes == null)
                    return;

                foreach (var barcode in barcodes)
                {
                    var barcodeData = new BarcodeData
                    {
                        Data = barcode.Data,
                        Type = barcode.Type
                    };
                    Barcodes.GenerateBarcode(barcodeData, barcode.Title);
                }
                appSettingsService.StoragePath = storagePath;
                Barcodes.StatusMessage = $"Successfully loaded barcodes from {Path.GetFileName(storagePath)}";
            }
            catch (Exception exc)
            {
                dialogsService.ShowException("Error during loading barcodes from file", exc);
            }
        }

        private void SaveBarcodesToFile()
        {
            if (!Barcodes.Barcodes.Any())
            {
                dialogsService.ShowError("Generate barcodes before saving");
                return;
            }

            try
            {
                var fileName = Path.GetFileName(appSettingsService.StoragePath);
                var directoryPath = Path.GetDirectoryName(appSettingsService.StoragePath);
                var filePath = dialogsService.SaveFile("Barcodes storage file", directoryPath, fileName, new DialogFilterPair { DisplayName = "json", ExtensionsList = "json" });
                if (string.IsNullOrEmpty(filePath))
                    return;

                var barcodesToSave = Barcodes.Barcodes.Select(b => new BarcodeStorageEntry
                {
                    Data = b.Data,
                    Title = b.Title,
                    Type = b.Type
                }).ToList();

                barcodeStorageService.Save(filePath, barcodesToSave);
                appSettingsService.StoragePath = filePath;
                Barcodes.StatusMessage = $"Successfully saved barcodes to {fileName}";
            }
            catch(Exception exc)
            {
                dialogsService.ShowException("Error during saving barcodes to file", exc);
            }
        }
    }
}
