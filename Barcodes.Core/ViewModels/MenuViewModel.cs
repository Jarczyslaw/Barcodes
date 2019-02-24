using Barcodes.Core.Services;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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

            SaveToFileCommand = new DelegateCommand(SaveBarcodesToFile);
            LoadFromFileCommand = new DelegateCommand(LoadBarcodesFromFile);
            OpenStorageLocationCommand = new DelegateCommand(OpenStorageLocation);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
            ExportToPdfCommand = new DelegateCommand(ExportToPdf);
            ShowHelpCommand = new DelegateCommand(ShowHelp);
            AddNewWorkspaceCommand = new DelegateCommand(AddNewWorkspace);
        }

        public DelegateCommand SaveToFileCommand { get; }
        public DelegateCommand LoadFromFileCommand { get; }
        public DelegateCommand OpenStorageLocationCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand ExportToPdfCommand { get; }
        public DelegateCommand ShowHelpCommand { get; }
        public DelegateCommand AddNewWorkspaceCommand { get; }

        public Action OnClose { get; set; }

        private void AddNewWorkspace()
        {
            throw new NotImplementedException();
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
                var storageWorkspaces = servicesContainer.StorageService.Load(storagePath, false);
                if (storageWorkspaces == null)
                {
                    return;
                }

                appState.Workspaces.Clear();

                var invalidCodes = new List<string>();
                foreach (var storageWorkspace in storageWorkspaces)
                {
                    var workspace = new WorkspaceViewModel
                    {
                        Title = storageWorkspace.Title,
                        Default = storageWorkspace.Default
                    };

                    foreach (var storageBarcode in storageWorkspace.Barcodes)
                    {
                        if (!storageBarcode.IsValid || !storageBarcode.ValidSizes)
                        {
                            invalidCodes.Add($"Barcode {storageBarcode.Title} can not be generated due to invalid sizes");
                            continue;
                        }

                        var barcodeData = new GenerationData
                        {
                            Data = storageBarcode.Data,
                            Type = storageBarcode.Type,
                            DefaultSize = storageBarcode.DefaultSize,
                            ValidateCodeText = false,
                            Width = storageBarcode.Width,
                            Height = storageBarcode.Height
                        };

                        try
                        {
                            var barcodeResult = new BarcodeResultViewModel(barcodeData)
                            {
                                Title = storageBarcode.Title,
                                Barcode = servicesContainer.GeneratorService.CreateBarcode(barcodeData)
                            };
                            workspace.Barcodes.Add(barcodeResult);
                        }
                        catch (Exception exc)
                        {
                            invalidCodes.Add($"Barcode {storageBarcode.Title} can not be generated due to generation error: {exc.Message}");
                        }
                    }

                    appState.AddWorkspace(workspace);
                }

                servicesContainer.AppSettingsService.StoragePath = storagePath;

                var successfullyGenerated = storageWorkspaces.Sum(s => s.Barcodes.Count) - invalidCodes.Count;
                if (successfullyGenerated > 0)
                {
                    appState.SetMessageAndCounter($"Successfully loaded {successfullyGenerated} barcodes from {Path.GetFileName(storagePath)}");
                }

                if (invalidCodes.Count != 0)
                {
                    const int invalidCodesDisplayCount = 10;
                    var detailsMessage = $"Total invalid barcodes: {invalidCodes.Count}. First {Math.Min(invalidCodesDisplayCount, invalidCodes.Count)} errors: {Environment.NewLine}";
                    detailsMessage += string.Join(Environment.NewLine, invalidCodes.Take(invalidCodesDisplayCount));
                    servicesContainer.AppDialogsService.ShowError("Some barcodes are not generated successfully. Check details for further informations", detailsMessage);
                }
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
                var filePath = servicesContainer.AppSettingsService.StoragePath;
                servicesContainer.AppDialogsService.SaveStorageFile(filePath);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                var storageWorkspaces = appState.Workspaces.Select(w => new StorageWorkspace
                {
                    Title = w.Title,
                    Default = w.Default,
                    Barcodes = w.Barcodes.Select(b => new StorageBarcode
                    {
                        Data = b.GenerationData.Data,
                        Title = b.Title,
                        Type = b.GenerationData.Type,
                        Width = b.GenerationData.Width,
                        Height = b.GenerationData.Height,
                        DefaultSize = b.GenerationData.DefaultSize
                    }).ToList()
                }).ToList();

                servicesContainer.StorageService.Save(filePath, storageWorkspaces);
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

                var barcodesToExport = appState.SelectedWorkspace.Barcodes.Select(b => new DocBarcodeData
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
