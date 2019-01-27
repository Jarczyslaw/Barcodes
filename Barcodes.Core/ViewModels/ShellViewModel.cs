using Barcodes.Core.Events;
using Barcodes.Core.Services;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Utils;
using Prism.Commands;
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
        private BarcodeResultViewModel selectedBarcode;
        private ObservableCollection<BarcodeResultViewModel> barcodes;
        private string busyMessage = string.Empty;

        private readonly IServicesContainer services;

        public ShellViewModel(IServicesContainer services)
        {
            this.services = services;

            Barcodes = new ObservableCollection<BarcodeResultViewModel>();

            EditBarcodeCommand = new DelegateCommand<BarcodeResultViewModel>(EditBarcode);
            MoveUpCommand = new DelegateCommand<BarcodeResultViewModel>(MoveUp);
            MoveDownCommand = new DelegateCommand<BarcodeResultViewModel>(MoveDown);
            OpenInNewWindowCommand = new DelegateCommand(OpenInNewWindow);
            CopyToClipboardCommand = new DelegateCommand<BarcodeResultViewModel>(CopyToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeResultViewModel>(Delete);

            AddNewBarcodeCommand = new DelegateCommand(AddNewBarcode);
            SaveToFileCommand = new DelegateCommand(SaveBarcodesToFile);
            LoadFromFileCommand = new DelegateCommand(LoadBarcodesFromFile);
            OpenStorageLocationCommand = new DelegateCommand(OpenStorageLocation);
            CloseCommand = new DelegateCommand(() => services.EventAggregator.GetEvent<ShellWindowClose>().Publish());
            ExportToPdfCommand = new DelegateCommand(ExportToPdf);
            ShowHelpCommand = new DelegateCommand(ShowHelp);

            services.AppSettingsService.Load();
            InitialBarcodesLoad();
        }

        public DelegateCommand AddNewBarcodeCommand { get; }
        public DelegateCommand SaveToFileCommand { get; }
        public DelegateCommand LoadFromFileCommand { get; }
        public DelegateCommand OpenStorageLocationCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand ExportToPdfCommand { get; }
        public DelegateCommand ShowHelpCommand { get; }

        public DelegateCommand<BarcodeResultViewModel> EditBarcodeCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> MoveUpCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> MoveDownCommand { get; }
        public DelegateCommand OpenInNewWindowCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> CopyToClipboardCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> DeleteCommand { get; }

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

        public int BarcodesCount
        {
            get => Barcodes.Count;
        }

        public BarcodeResultViewModel SelectedBarcode
        {
            get => selectedBarcode;
            set => SetProperty(ref selectedBarcode, value);
        }

        public ObservableCollection<BarcodeResultViewModel> Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
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

                int successfullyGenerated = 0;
                foreach (var barcode in barcodesFromStorage)
                {
                    if (!barcode.IsValid || !barcode.ValidSizes)
                    {
                        continue;
                    }

                    var barcodeData = new GenerationData
                    {
                        Data = barcode.Data,
                        Type = barcode.Type,
                        DefaultSize = barcode.DefaultSize,
                        ValidateCodeText = false,
                        Width = barcode.Width,
                        Height = barcode.Height
                    };

                    try
                    {
                        GenerateBarcode(barcodeData, barcode.Title);
                        successfullyGenerated++;
                    }
                    catch { }
                }

                services.AppSettingsService.StoragePath = storagePath;
                StatusMessage = $"Successfully loaded {successfullyGenerated} barcodes from {Path.GetFileName(storagePath)}";
                RaisePropertyChanged(nameof(BarcodesCount));
            }
            catch (Exception exc)
            {
                services.DialogsService.ShowException("Error when loading barcodes from file", exc);
            }
        }

        private void GenerateBarcode(GenerationData barcodeData, string title)
        {
            var barcodeBitmap = services.BarcodesGenerator.CreateBarcode(barcodeData);
            barcodeBitmap.Freeze();
            Barcodes.Insert(0, new BarcodeResultViewModel(barcodeData)
            {
                Barcode = barcodeBitmap,
                Title = title
            });
        }

        private void SaveBarcodesToFile()
        {
            if (BarcodesCount == 0)
            {
                services.DialogsService.ShowError("No barcodes to save");
                return;
            }

            try
            {
                var filePath = services.DialogsService.SaveStorageFile(services.AppSettingsService.StoragePath);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                var barcodesToSave = Barcodes.Select(b => new StorageEntry
                {
                    Data = b.GenerationData.Data,
                    Title = b.Title,
                    Type = b.GenerationData.Type,
                    Width = b.GenerationData.Width,
                    Height = b.GenerationData.Height,
                    DefaultSize = b.GenerationData.DefaultSize
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
                    Data = b.GenerationData.Data,
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

        private void AddNewBarcode()
        {
            var newBarcode = services.AppWindowsService.ShowGenerationWindow();
            if (newBarcode == null)
                return;

            Barcodes.Insert(0, newBarcode);
            StatusMessage = $"Barcode \"{newBarcode.Title}\" generated successfully!";
            RaisePropertyChanged(nameof(BarcodesCount));
        }

        private void EditBarcode(BarcodeResultViewModel barcode)
        {
            var newBarcode = services.AppWindowsService.ShowGenerationWindow(barcode);
            if (newBarcode == null)
                return;

            var barcodeIndex = Barcodes.IndexOf(barcode);
            Barcodes.Remove(barcode);
            Barcodes.Insert(barcodeIndex, newBarcode);
            StatusMessage = $"Barcode \"{newBarcode.Title}\" edited successfully!";
        }
    }
}
