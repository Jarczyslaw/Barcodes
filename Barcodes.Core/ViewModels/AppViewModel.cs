using Barcodes.Core.Services;
using Barcodes.Extensions;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class AppViewModel : BindableBase
    {
        private string statusMessage = string.Empty;
        private WorkspaceViewModel selectedWorkspace;
        private ObservableCollection<WorkspaceViewModel> workspaces;
        private string busyMessage = string.Empty;

        private readonly IServicesContainer servicesContainer;

        public AppViewModel(IServicesContainer servicesContainer)
        {
            this.servicesContainer = servicesContainer;

            workspaces = new ObservableCollection<WorkspaceViewModel>();
        }

        public bool IsBusy { get; set; }

        public bool IsEmpty
        {
            get => workspaces.Count == 0;
        }

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
            get => Workspaces.Sum(w => w.Barcodes.Count);
        }

        public WorkspaceViewModel SelectedWorkspace
        {
            get => selectedWorkspace;
            set => SetProperty(ref selectedWorkspace, value);
        }

        public ReadOnlyObservableCollection<WorkspaceViewModel> Workspaces
        {
            get => new ReadOnlyObservableCollection<WorkspaceViewModel>(workspaces);
        }

        private void UpdateMessage(string message)
        {
            StatusMessage = message;
        }

        private void UpdateBarcodesCount()
        {
            RaisePropertyChanged(nameof(BarcodesCount));
        }

        private void RemoveWorkspace(WorkspaceViewModel workspace)
        {
            workspace.OnMessageUpdate -= UpdateMessage;
            workspace.OnCounterUpdate -= UpdateBarcodesCount;

            workspaces.Remove(workspace);
            UpdateMessage($"Successfully removed {workspace.Name}");
            UpdateBarcodesCount();
        }

        private void AddWorkspace(WorkspaceViewModel workspace)
        {
            workspace.OnMessageUpdate += UpdateMessage;
            workspace.OnCounterUpdate += UpdateBarcodesCount;

            workspaces.Add(workspace);
            SelectedWorkspace = workspace;
        }

        private bool WorkspaceValidationRule(string workspaceName)
        {
            if (string.IsNullOrEmpty(workspaceName))
            {
                servicesContainer.AppDialogsService.ShowError("Workspace's name can not be empty");
                return false;
            }

            if (Workspaces.Any(w => w.Name == workspaceName))
            {
                servicesContainer.AppDialogsService.ShowError("The workspace with a given name already exists");
                return false;
            }

            return true;
        }

        public void AddNewWorkspace()
        {
            var workspaceName = servicesContainer.AppWindowsService.ShowWorkspaceNameWindow(string.Empty, WorkspaceValidationRule);
            if (string.IsNullOrEmpty(workspaceName))
            {
                return;
            }

            var newWorkspace = new WorkspaceViewModel
            {
                Name = workspaceName
            };
            AddWorkspace(newWorkspace);
        }

        public void LoadBarcodesFromFile()
        {
            var storagePath = servicesContainer.AppSettingsService.StoragePath;
            var filePath = servicesContainer.AppDialogsService.OpenStorageFile(storagePath);
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            LoadFromFile(filePath);
        }

        public void LoadFromFile(string storagePath)
        {
            try
            {
                var storageWorkspaces = servicesContainer.StorageService.Load(storagePath, false);
                if (storageWorkspaces == null)
                {
                    return;
                }

                workspaces.Clear();

                var invalidCodes = new List<string>();
                foreach (var storageWorkspace in storageWorkspaces)
                {
                    var workspace = new WorkspaceViewModel
                    {
                        Name = storageWorkspace.Title,
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

                    AddWorkspace(workspace);
                }

                SelectedWorkspace = Workspaces.SingleOrDefault(w => w.Default);

                servicesContainer.AppSettingsService.StoragePath = storagePath;

                var successfullyGenerated = storageWorkspaces.Sum(s => s.Barcodes.Count) - invalidCodes.Count;
                if (successfullyGenerated > 0)
                {
                    UpdateMessage($"Successfully loaded {successfullyGenerated} barcodes from {Path.GetFileName(storagePath)}");
                    UpdateBarcodesCount();
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

        public void SaveToFile()
        {
            if (BarcodesCount == 0)
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

                var storageWorkspaces = Workspaces.Select(w => new StorageWorkspace
                {
                    Title = w.Name,
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
                StatusMessage = $"Successfully saved {Path.GetFileName(filePath)}";
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when saving barcodes to file", exc);
            }
        }

        public void ShowHelp()
        {
            servicesContainer.AppWindowsService.ShowHelpWindow();
        }

        public void ExportToPdf()
        {
            var mode = servicesContainer.AppDialogsService.ShowPdfExportQuestion();
            if (mode == PdfExportMode.None)
            {
                return;
            }

            var barcodesToExport = new List<BarcodeResultViewModel>();
            if (mode == PdfExportMode.CurrentWorkspace)
            {
                if (SelectedWorkspace == null || SelectedWorkspace.Barcodes.Count == 0)
                {
                    servicesContainer.AppDialogsService.ShowError("No workspace found or there are no barcodes in active workspace");
                    return;
                }
                else
                {
                    barcodesToExport.AddRange(SelectedWorkspace.Barcodes);
                }
            }
            else if (mode == PdfExportMode.All)
            {
                if (BarcodesCount == 0)
                {
                    servicesContainer.AppDialogsService.ShowError("No barcodes to export");
                    return;
                }
                else
                {
                    foreach (var workspace in Workspaces)
                    {
                        barcodesToExport.AddRange(workspace.Barcodes);
                    }
                }
            }

            ExecuteExportToPdf(barcodesToExport);
        }

        private async void ExecuteExportToPdf(IEnumerable<BarcodeResultViewModel> barcodes)
        {
            try
            {
                var filePath = servicesContainer.AppDialogsService.SavePdfFile();
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                BusyMessage = "Generating document...";

                var barcodesToExport = barcodes.Select(b => new DocBarcodeData
                {
                    Title = b.Title,
                    Data = b.GenerationData.Data,
                    Barcode = b.Barcode
                }).ToList();
                await servicesContainer.DocExportService.ExportAsync(barcodesToExport, filePath)
                    .ConfigureAwait(false);
                await Task.Delay(2000)
                    .ConfigureAwait(false);

                StatusMessage = $"Successfully exported to {filePath}";
                BusyMessage = null;

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
                BusyMessage = null;
            }
        }

        public void OpenStorageLocation()
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

        public void AddNewBarcode()
        {
            var result = servicesContainer.AppWindowsService.ShowGenerationWindow();
            if (result == null)
            {
                return;
            }

            if (Workspaces.Count == 0 && !AddInitialWorkspace())
            {
                return;
            }

            SelectedWorkspace.InsertNewBarcode(result.Barcode);
        }

        private bool AddInitialWorkspace()
        {
            var workspaceName = servicesContainer.AppWindowsService.ShowWorkspaceNameWindow(null, WorkspaceValidationRule);
            if (string.IsNullOrEmpty(workspaceName))
            {
                return false;
            }

            var workspace = new WorkspaceViewModel
            {
                Default = true,
                Name = workspaceName
            };
            AddWorkspace(workspace);
            return true;
        }

        public void EditBarcode(BarcodeResultViewModel barcode)
        {
            var result = servicesContainer.AppWindowsService.ShowGenerationWindow(barcode);
            if (result == null)
            {
                return;
            }

            if (result.AddNew)
            {
                SelectedWorkspace.InsertNewBarcode(result.Barcode);
            }
            else
            {
                SelectedWorkspace.ReplaceBarcode(barcode, result.Barcode);
            }
        }

        public void DeleteBarcode(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
            {
                return;
            }

            if (!servicesContainer.AppDialogsService.ShowYesNoQuestion($"Do you really want to delete barcode {barcode.Title}?"))
            {
                return;
            }

            SelectedWorkspace.RemoveBarcode(barcode);
        }

        public void OpenInNewWindow()
        {
            if (SelectedWorkspace.SelectedBarcode == null)
            {
                return;
            }

            servicesContainer.AppWindowsService.OpenBarcodeWindow(SelectedWorkspace.SelectedBarcode);
        }

        public void SaveToImageFile(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
            {
                return;
            }

            try
            {
                var filePath = servicesContainer.AppDialogsService.SavePngFile(barcode.Title);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                barcode.Barcode.ToPng(filePath);
                StatusMessage = $"Barcode {barcode.Title} saved in {Path.GetFileName(filePath)}";
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when saving barcode to png file", exc);
            }
        }

        public void CopyToClipboard(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
            {
                return;
            }

            servicesContainer.SystemService.CopyToClipboard(barcode.Barcode);
            StatusMessage = $"Barcode {barcode.Title} copied to clipboard";
        }

        public void MoveBarcodeDown(BarcodeResultViewModel barcode)
        {
            SelectedWorkspace.MoveDown(barcode);
        }

        public void MoveBarcodeUp(BarcodeResultViewModel barcode)
        {
            SelectedWorkspace.MoveUp(barcode);
        }

        public void RenameWorkspace()
        {
            if (SelectedWorkspace == null)
            {
                return;
            }

            var workspaceName = servicesContainer.AppWindowsService.ShowWorkspaceNameWindow(SelectedWorkspace.Name, WorkspaceValidationRule);
            if (string.IsNullOrEmpty(workspaceName))
            {
                return;
            }

            SelectedWorkspace.Name = workspaceName;
        }

        public void DeleteWorkspace()
        {
            if (SelectedWorkspace == null)
            {
                return;
            }

            if (!servicesContainer.AppDialogsService.ShowYesNoQuestion($"Do you really want to delete workspace {SelectedWorkspace.Name}? This will delete all the codes of this workspace"))
            {
                return;
            }
            RemoveWorkspace(SelectedWorkspace);
        }

        public void SetAsDefaultWorkspace()
        {
            if (SelectedWorkspace == null)
            {
                return;
            }

            foreach (var workspace in Workspaces)
            {
                workspace.Default = false;
            }
            SelectedWorkspace.Default = true;
        }

        public void MoveWorkspaceLeft()
        {
            if (SelectedWorkspace == null)
            {
                return;
            }

            var index = workspaces.IndexOf(SelectedWorkspace);
            workspaces.ShiftLeft(index);
        }

        public void MoveWorkspaceRight()
        {
            if (SelectedWorkspace == null)
            {
                return;
            }

            var index = workspaces.IndexOf(SelectedWorkspace);
            workspaces.ShiftRight(index);
        }

        public void ChangeBarcodesWorkspace()
        {
            if (SelectedWorkspace == null || SelectedWorkspace.SelectedBarcode == null)
            {
                return;
            }

            if (Workspaces.Count() == 1)
            {
                servicesContainer.AppDialogsService.ShowInfo("There is no workspace to switch to");
            }
            else if (Workspaces.Count == 2)
            {
                var targetWorkspace = GetWorkspacesWithout(SelectedWorkspace).Single();
                MoveBarcodeToWorkspace(SelectedWorkspace.SelectedBarcode, SelectedWorkspace, targetWorkspace);
            }
            else
            {
                var availableWorkspaces = GetWorkspacesWithout(SelectedWorkspace);
                var targetWorkspace = servicesContainer.AppWindowsService.SelectBarcodesWorkspace(availableWorkspaces);
                if (targetWorkspace == null)
                {
                    return;
                }
                MoveBarcodeToWorkspace(SelectedWorkspace.SelectedBarcode, SelectedWorkspace, targetWorkspace);
            }
        }

        private IEnumerable<WorkspaceViewModel> GetWorkspacesWithout(WorkspaceViewModel workspace)
        {
            return Workspaces.Where(w => w != workspace);
        }

        private void MoveBarcodeToWorkspace(BarcodeResultViewModel barcode, WorkspaceViewModel sourceWorkspace, WorkspaceViewModel targetWorkspace)
        {
            sourceWorkspace.Barcodes.Remove(barcode);
            targetWorkspace.Barcodes.Insert(0, barcode);
        }

        public void ClearWorkspace()
        {
            if (SelectedWorkspace == null || SelectedWorkspace.Barcodes.Count == 0)
            {
                servicesContainer.AppDialogsService.ShowInfo("There is nothing to clear");
                return;
            }

            var dialogResult = servicesContainer.AppDialogsService.ShowYesNoQuestion("Do you really want to clear active workspace?");
            if (!dialogResult)
            {
                return;
            }

            SelectedWorkspace.Barcodes.Clear();
        }
    }
}
