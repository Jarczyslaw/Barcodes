using Barcodes.Core.Events;
using Barcodes.Core.Services;
using Barcodes.Services.AppSettings;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Services.System;
using Barcodes.Utils;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private string statusMessage = string.Empty;
        private string data = "Barcode Data";
        private string title = "Barcode Title";
        private bool defaultSize = true;
        private int width = 100;
        private int height = 100;
        private BarcodeTypeViewModel selectedBarcodeType;
        private BarcodeResultViewModel selectedBarcode;
        private ObservableCollection<BarcodeTypeViewModel> barcodeTypes;
        private ObservableCollection<BarcodeResultViewModel> barcodes;

        private string busyMessage = string.Empty;

        private readonly IServicesContainer services;

        public ShellViewModel(IServicesContainer services)
        {
            this.services = services;

            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
            AdditionalInputCommand = new DelegateCommand(AdditionalInput, () => AdditionalInputEnabled);

            MoveUpCommand = new DelegateCommand<BarcodeResultViewModel>(MoveUp);
            MoveDownCommand = new DelegateCommand<BarcodeResultViewModel>(MoveDown);
            OpenInNewWindowCommand = new DelegateCommand(OpenInNewWindow);
            CopyToClipboardCommand = new DelegateCommand<BarcodeResultViewModel>(CopyToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeResultViewModel>(Delete);

            SaveToFileCommand = new DelegateCommand(SaveBarcodesToFile);
            LoadFromFileCommand = new DelegateCommand(LoadBarcodesFromFile);
            OpenStorageLocationCommand = new DelegateCommand(OpenStorageLocation);
            CloseCommand = new DelegateCommand(() => services.EventAggregator.GetEvent<CloseEvent>().Publish());
            ExportToPdfCommand = new DelegateCommand(ExportToPdf);
            ShowHelpCommand = new DelegateCommand(ShowHelp);

            services.AppSettingsService.Load();
            InitializeBarcodes();
            InitialBarcodesLoad();
        }

        public DelegateCommand GenerateBarcodeCommand { get; }
        public DelegateCommand AdditionalInputCommand { get; }

        public DelegateCommand<BarcodeResultViewModel> MoveUpCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> MoveDownCommand { get; }
        public DelegateCommand OpenInNewWindowCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> CopyToClipboardCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> DeleteCommand { get; }

        public DelegateCommand SaveToFileCommand { get; }
        public DelegateCommand LoadFromFileCommand { get; }
        public DelegateCommand OpenStorageLocationCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand ExportToPdfCommand { get; }
        public DelegateCommand ShowHelpCommand { get; }

        public bool IsBusy { get; set; }

        public string BusyMessage
        {
            get => busyMessage;
            set
            {
                SetProperty(ref busyMessage, value);
                IsBusy = !string.IsNullOrEmpty(busyMessage);
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
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

        public int BarcodesCount
        {
            get => Barcodes.Count;
        }

        public bool AdditionalInputEnabled
        {
            get
            {
                if (SelectedBarcodeType == null)
                {
                    return false;
                }

                return SelectedBarcodeType.AdditionalInput != null;
            }
        }

        public BarcodeTypeViewModel SelectedBarcodeType
        {
            get => selectedBarcodeType;
            set
            {
                SetProperty(ref selectedBarcodeType, value);
                AdditionalInputCommand.RaiseCanExecuteChanged();
            }
        }

        public BarcodeResultViewModel SelectedBarcode
        {
            get => selectedBarcode;
            set => SetProperty(ref selectedBarcode, value);
        }

        public ObservableCollection<BarcodeTypeViewModel> BarcodeTypes
        {
            get => barcodeTypes;
            set => SetProperty(ref barcodeTypes, value);
        }

        public ObservableCollection<BarcodeResultViewModel> Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }

        private void InitializeBarcodes()
        {
            Barcodes = new ObservableCollection<BarcodeResultViewModel>();

            BarcodeTypes = new ObservableCollection<BarcodeTypeViewModel>()
            {
                new BarcodeTypeViewModel { Type = BarcodeType.Ean13 },
                new BarcodeTypeViewModel
                {
                    Type = BarcodeType.Ean128,
                    AdditionalInput = services.AppWindowsService.OpenEan128ProductWindow
                },
                new BarcodeTypeViewModel { Type = BarcodeType.Code128 },
                new BarcodeTypeViewModel
                {
                    Type = BarcodeType.DataMatrix,
                    AdditionalInput = services.AppWindowsService.OpenNmvsProductWindow
                },
                new BarcodeTypeViewModel { Type = BarcodeType.QRCode },
            };
            SelectedBarcodeType = BarcodeTypes.First(t => t.Type == BarcodeType.DataMatrix);
        }

        private bool GenerateValidation()
        {
            if (string.IsNullOrEmpty(Title))
            {
                services.DialogsService.ShowError("Enter barcode's title");
                return false;
            }

            if (string.IsNullOrEmpty(Data))
            {
                services.DialogsService.ShowError("Enter barcode's data");
                return false;
            }

            return true;
        }

        private void GenerateBarcode()
        {
            if (!GenerateValidation())
            {
                return;
            }

            var barcodeData = new BarcodeData
            {
                Data = Data,
                Type = SelectedBarcodeType.Type,
                DefaultSize = DefaultSize,
                Width = Width,
                Height = Height
            };

            try
            {
                GenerateBarcode(barcodeData, Title);
                StatusMessage = $"Barcode \"{Title}\" generate successfully!";
                RaisePropertyChanged(nameof(BarcodesCount));
            }
            catch (Exception exc)
            {
                services.DialogsService.ShowException("Exception during barcode generation", exc);
            }
        }

        private void GenerateBarcode(BarcodeData barcodeData, string title)
        {
            var barcode = services.BarcodesGenerator.CreateBarcode(barcodeData);
            barcode.Freeze();
            Barcodes.Insert(0, new BarcodeResultViewModel
            {
                Barcode = barcode,
                Data = barcodeData.Data,
                Title = title,
                Type = barcodeData.Type,
            });
            RaisePropertyChanged(nameof(BarcodesCount));
        }

        private void Delete(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
            {
                return;
            }

            if (!services.DialogsService.ShowYesNoQuestion($"Do you really want to delete barcode \"{barcode.Title}?\""))
            {
                return;
            }

            Barcodes.Remove(barcode);
            StatusMessage = $"Successfully removed \"{barcode.Title}\"";
            RaisePropertyChanged(nameof(BarcodesCount));
        }

        private void AdditionalInput()
        {
            var result = SelectedBarcodeType.AdditionalInput();
            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            Data = result;
        }

        private void OpenInNewWindow()
        {
            if (SelectedBarcode == null)
            {
                return;
            }

            services.AppWindowsService.OpenBarcodeWindow(SelectedBarcode);
        }

        private void CopyToClipboard(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
            {
                return;
            }

            services.SystemService.CopyToClipboard(barcode.Barcode);
            StatusMessage = $"Barcode \"{barcode.Title}\" copied to clipboard";
        }

        private void InitialBarcodesLoad()
        {
            LoadBarcodesFromFile(services.AppSettingsService.StoragePath);
        }

        private void LoadBarcodesFromFile()
        {
            var filePath = services.DialogsService.OpenStorageFile(services.AppSettingsService.StoragePath);
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            LoadBarcodesFromFile(filePath);
        }

        private void LoadBarcodesFromFile(string storagePath)
        {
            try
            {
                var barcodesFromStorage = services.BarcodeStorageService.Load(storagePath, false);
                if (barcodesFromStorage == null)
                {
                    return;
                }

                Barcodes.Clear();
                barcodesFromStorage.Reverse();
                foreach (var barcode in barcodesFromStorage)
                {
                    var barcodeData = new BarcodeData
                    {
                        Data = barcode.Data,
                        Type = barcode.Type
                    };
                    GenerateBarcode(barcodeData, barcode.Title);
                }
                services.AppSettingsService.StoragePath = storagePath;
                StatusMessage = $"Successfully loaded barcodes from {Path.GetFileName(storagePath)}";
            }
            catch (Exception exc)
            {
                services.DialogsService.ShowException("Error when loading barcodes from file", exc);
            }
        }

        private void SaveBarcodesToFile()
        {
            if (BarcodesCount == 0)
            {
                services.DialogsService.ShowError("Generate barcodes before saving");
                return;
            }

            try
            {
                var filePath = services.DialogsService.SaveStorageFile(services.AppSettingsService.StoragePath);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                var barcodesToSave = Barcodes.Select(b => new BarcodeStorageEntry
                {
                    Data = b.Data,
                    Title = b.Title,
                    Type = b.Type
                }).ToList();

                services.BarcodeStorageService.Save(filePath, barcodesToSave);
                services.AppSettingsService.StoragePath = filePath;
                StatusMessage = $"Successfully saved {Path.GetFileName(filePath)}";
            }
            catch (Exception exc)
            {
                services.DialogsService.ShowException("Error when saving barcodes to file", exc);
            }
        }

        private void ShowHelp()
        {
            services.AppWindowsService.ShowHelpWindow();
        }

        private async void ExportToPdf()
        {
            if (BarcodesCount == 0)
            {
                services.DialogsService.ShowError("Generate barcodes before export");
                return;
            }

            try
            {
                var filePath = services.DialogsService.SavePdfFile();
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                BusyMessage = "Generating document...";

                var barcodesToExport = Barcodes.Select(b => new DocBarcodeData
                {
                    Title = b.Title,
                    Data = b.Data,
                    Barcode = b.Barcode
                }).ToList();
                await services.DocExportService.ExportAsync(barcodesToExport, filePath)
                    .ConfigureAwait(false);
                await Task.Delay(2000)
                    .ConfigureAwait(false);

                StatusMessage = $"Successfully exported to {filePath}";
                BusyMessage = null;

                if (services.DialogsService.ShowYesNoQuestion($"Do you want to open the newly generated file?"))
                {
                    services.SystemService.StartProcess(filePath);
                }
            }
            catch (Exception exc)
            {
                services.DialogsService.ShowException("Error when generating a document", exc);
            }
            finally
            {
                BusyMessage = null;
            }
        }

        private void OpenStorageLocation()
        {
            const string openErrorMessage = "Can not open storage file location";
            try
            {
                if (!File.Exists(services.AppSettingsService.StoragePath))
                {
                    services.DialogsService.ShowError(openErrorMessage);
                    return;
                }

                services.SystemService.OpenLocation(services.AppSettingsService.StoragePath);
            }
            catch (Exception exc)
            {
                services.DialogsService.ShowException(openErrorMessage, exc);
            }
        }

        private void MoveDown(BarcodeResultViewModel barcode)
        {
            var index = Barcodes.IndexOf(barcode);
            Barcodes.ShiftRight(index);
        }

        private void MoveUp(BarcodeResultViewModel barcode)
        {
            var index = Barcodes.IndexOf(barcode);
            Barcodes.ShiftLeft(index);
        }
    }
}
