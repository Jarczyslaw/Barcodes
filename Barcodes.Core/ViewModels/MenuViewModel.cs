using Barcodes.Core.Services;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private readonly IAppState appState;
        private readonly IServicesContainer servicesContainer;

        public MenuViewModel(IAppState appState, IServicesContainer servicesContainer)
        {
            this.appState = appState;
            this.servicesContainer = servicesContainer;

            AddNewBarcodeCommand = new DelegateCommand(AddNewBarcode);
            SaveToFileCommand = new DelegateCommand(SaveBarcodesToFile);
            LoadFromFileCommand = new DelegateCommand(LoadBarcodesFromFile);
            OpenStorageLocationCommand = new DelegateCommand(OpenStorageLocation);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
            ExportToPdfCommand = new DelegateCommand(ExportToPdf);
            ShowHelpCommand = new DelegateCommand(ShowHelp);
        }

        public DelegateCommand AddNewBarcodeCommand { get; }
        public DelegateCommand SaveToFileCommand { get; }
        public DelegateCommand LoadFromFileCommand { get; }
        public DelegateCommand OpenStorageLocationCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand ExportToPdfCommand { get; }
        public DelegateCommand ShowHelpCommand { get; }

        public Action OnClose { get; set; }

        public void AddNewBarcode()
        {
            var result = servicesContainer.AppWindowsService.ShowGenerationWindow();
            if (result == null)
            {
                return;
            }

            appState.InsertNewBarcode(result.Barcode);
        }

        private void LoadBarcodesFromFile()
        {
            var storagePath = servicesContainer.AppSettingsService.StoragePath;
            var filePath = servicesContainer.AppDialogsService.OpenStorageFile(storagePath);
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            LoadBarcodesFromFile(filePath);
        }

        public void LoadBarcodesFromFile(string storagePath)
        {
            try
            {
                var barcodesFromStorage = servicesContainer.StorageService.Load(storagePath, false);
                if (barcodesFromStorage == null)
                {
                    return;
                }

                appState.Barcodes.Clear();
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
                        appState.GenerateAndInsertBarcode(barcodeData, barcode.Title);
                        successfullyGenerated++;
                    }
                    catch { }
                }

                servicesContainer.AppSettingsService.StoragePath = storagePath;
                appState.SetMessageAndCounter($"Successfully loaded {successfullyGenerated} barcodes from {Path.GetFileName(storagePath)}");
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when loading barcodes from file", exc);
            }
        }


        private void SaveBarcodesToFile()
        {
            if (appState.BarcodesCount == 0)
            {
                var dialogResult = servicesContainer.AppDialogsService.ShowYesNoQuestion("You don't have any barcodes to save. Do you want to clear default storage file?");
                if (!dialogResult)
                {
                    return;
                }
            }

            try
            {
                var filePath = servicesContainer.AppDialogsService.SaveStorageFile(servicesContainer.AppSettingsService.StoragePath);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                var barcodesToSave = appState.Barcodes.Select(b => new StorageEntry
                {
                    Data = b.GenerationData.Data,
                    Title = b.Title,
                    Type = b.GenerationData.Type,
                    Width = b.GenerationData.Width,
                    Height = b.GenerationData.Height,
                    DefaultSize = b.GenerationData.DefaultSize
                }).ToList();

                servicesContainer.StorageService.Save(filePath, barcodesToSave);
                servicesContainer.AppSettingsService.StoragePath = filePath;
                appState.StatusMessage = $"Successfully saved {Path.GetFileName(filePath)}";
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when saving barcodes to file", exc);
            }
        }

        private void ShowHelp()
        {
            servicesContainer.AppWindowsService.ShowHelpWindow();
        }

        private async void ExportToPdf()
        {
            if (appState.BarcodesCount == 0)
            {
                servicesContainer.AppDialogsService.ShowError("Generate barcodes before export");
                return;
            }

            try
            {
                var filePath = servicesContainer.AppDialogsService.SavePdfFile();
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                appState.BusyMessage = "Generating document...";

                var barcodesToExport = appState.Barcodes.Select(b => new DocBarcodeData
                {
                    Title = b.Title,
                    Data = b.GenerationData.Data,
                    Barcode = b.Barcode
                }).ToList();
                await servicesContainer.DocExportService.ExportAsync(barcodesToExport, filePath)
                    .ConfigureAwait(false);
                await Task.Delay(2000)
                    .ConfigureAwait(false);

                appState.StatusMessage = $"Successfully exported to {filePath}";
                appState.BusyMessage = null;

                if (servicesContainer.AppDialogsService.ShowYesNoQuestion($"Do you want to open the newly generated file?"))
                {
                    servicesContainer.SystemService.StartProcess(filePath);
                }
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when generating a document", exc);
            }
            finally
            {
                appState.BusyMessage = null;
            }
        }

        private void OpenStorageLocation()
        {
            const string openErrorMessage = "Can not open storage file location";
            try
            {
                if (!File.Exists(servicesContainer.AppSettingsService.StoragePath))
                {
                    servicesContainer.AppDialogsService.ShowError(openErrorMessage);
                    return;
                }

                var storagePath = servicesContainer.AppSettingsService.StoragePath;
                servicesContainer.SystemService.OpenLocation(storagePath);
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException(openErrorMessage, exc);
            }
        }
    }
}
