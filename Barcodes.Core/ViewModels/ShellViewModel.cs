using Barcodes.Core.Events;
using Barcodes.Core.Services;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Generator;
using Barcodes.Utils;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
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

        public ShellViewModel(IBarcodesGeneratorService barcodesGenerator, IDialogsService dialogsService, 
            IBarcodeWindowsService barcodeWindowsService, IEventAggregator eventAggregator)
        {
            this.barcodesGenerator = barcodesGenerator;
            this.dialogsService = dialogsService;
            this.barcodeWindowsService = barcodeWindowsService;

            Barcodes = new BarcodesViewModel(barcodesGenerator, dialogsService, barcodeWindowsService);

            GenerateRandomBarcodeCommand = new DelegateCommand(GenerateRandomBarcode);
            GenerateBarcodeCommand = new DelegateCommand(Barcodes.GenerateBarcode);
            ExtraInputCommand = new DelegateCommand(Barcodes.ExtraInput, () => Barcodes.ExtraInputEnabled);
            OpenInNewWindowCommand = new DelegateCommand(OpenInNewWindow);
            CopyToClipboardCommand = new DelegateCommand<BarcodeResultViewModel>(CopyToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeResultViewModel>(Barcodes.Delete);

            SaveToFileCommand = new DelegateCommand(SaveToFile);
            LoadFromFileCommand = new DelegateCommand(LoadFromFile);
            OpenStorageLocationCommand = new DelegateCommand(OpenStorageLocation);
            CloseCommand = new DelegateCommand(() => eventAggregator.GetEvent<CloseEvent>().Publish());
            ExportToPdfCommand = new DelegateCommand(ExportToPdf);
            ShowHelpCommand = new DelegateCommand(ShowHelp);


            GenerateRandomBarcode();
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
            throw new NotImplementedException();
        }

        private void LoadFromFile()
        {
            throw new NotImplementedException();
        }

        private void SaveToFile()
        {
            throw new NotImplementedException();
        }
    }
}
