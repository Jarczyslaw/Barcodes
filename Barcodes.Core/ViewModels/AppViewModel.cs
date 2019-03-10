using Barcodes.Core.Extensions;
using Barcodes.Core.Services;
using Barcodes.Extensions;
using Barcodes.Services.DocExport;
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
        private readonly ObservableCollection<WorkspaceViewModel> workspaces;
        private string busyMessage = string.Empty;

        private readonly IServicesContainer servicesContainer;

        public AppViewModel(IServicesContainer servicesContainer)
        {
            this.servicesContainer = servicesContainer;

            workspaces = new ObservableCollection<WorkspaceViewModel>();
        }

        public bool IsBusy { get; set; }

        public int WorkspacesCount
        {
            get => Workspaces.Count;
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

        public BarcodeViewModel SelectedBarcode
        {
            get => SelectedWorkspace?.SelectedBarcode;
        }

        public ReadOnlyObservableCollection<WorkspaceViewModel> Workspaces
        {
            get => new ReadOnlyObservableCollection<WorkspaceViewModel>(workspaces);
        }

        private void UpdateMessage(string message)
        {
            StatusMessage = message;
            RaisePropertyChanged(nameof(BarcodesCount));
        }

        private void RemoveWorkspace(WorkspaceViewModel workspace)
        {
            workspace.OnMessageUpdate -= UpdateMessage;
            workspaces.Remove(workspace);
            UpdateMessage($"Successfully removed {workspace.Name}");
        }

        private void AddWorkspace(WorkspaceViewModel workspace)
        {
            workspace.OnMessageUpdate += UpdateMessage;
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
                Name = workspaceName,
                Default = !Workspaces.Any(w => w.Default)
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

            LoadFromFile(filePath, true);
        }

        private void LoadFromFile(string storagePath, bool storageValidation)
        {
            try
            {
                var storage = servicesContainer.StorageService.Load(storagePath, true);
                if (storage == null)
                {
                    return;
                }

                if (storageValidation && storage.WorkspacesCount == 0)
                {
                    servicesContainer.AppDialogsService.ShowError("Selected file has no workspaces");
                    return;
                }

                var newWorkspaces = new List<WorkspaceViewModel>();
                foreach (var storageWorkspace in storage.Content)
                {
                    var newWorkspace = storageWorkspace.ToWorkspace(servicesContainer.GeneratorService);
                    newWorkspaces.Add(newWorkspace);
                }

                workspaces.Clear();
                foreach (var newWorkspace in newWorkspaces)
                {
                    AddWorkspace(newWorkspace);
                }

                SelectedWorkspace = Workspaces.SingleOrDefault(w => w.Default);
                if (WorkspacesCount != 0 || BarcodesCount != 0)
                {
                    UpdateMessage($"Successfully loaded {WorkspacesCount} workspaces and {BarcodesCount} barcodes from {Path.GetFileName(storagePath)}");
                }
                else
                {
                    UpdateMessage($"File {Path.GetFileName(storagePath)} loaded");
                }
                servicesContainer.AppSettingsService.StoragePath = storagePath;
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when loading storage from file", exc);
            }
        }

        public void SaveToFile(bool validateBarcodesCount)
        {
            if (validateBarcodesCount && BarcodesCount == 0)
            {
                var dialogResult = servicesContainer.AppDialogsService.ShowYesNoQuestion("You don't have any barcodes to save. Do you want to save file without barcodes?");
                if (!dialogResult)
                {
                    return;
                }
            }

            try
            {
                var filePath = servicesContainer.AppSettingsService.StoragePath;
                filePath = servicesContainer.AppDialogsService.SaveStorageFile(filePath);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                servicesContainer.StorageService.Save(filePath, CreateCurrentStorage());
                servicesContainer.AppSettingsService.StoragePath = filePath;
                StatusMessage = $"Successfully saved {Path.GetFileName(filePath)}";
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when saving barcodes to file", exc);
            }
        }

        private Storage CreateCurrentStorage()
        {
            return new Storage
            {
                Content = Workspaces.Select(w => w.ToStorage()).ToList()
            };
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

            var barcodesToExport = new List<BarcodeViewModel>();
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

        private async void ExecuteExportToPdf(IEnumerable<BarcodeViewModel> barcodes)
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

            if (!TryAddInitialWorkspace())
            {
                return;
            }

            SelectedWorkspace.InsertNewBarcode(result.Barcode);
        }

        private bool TryAddInitialWorkspace()
        {
            if (WorkspacesCount != 0)
            {
                return true;
            }

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

        public void EditBarcode(BarcodeViewModel barcode)
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

        public void DeleteBarcode(BarcodeViewModel barcode)
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
            if (SelectedBarcode == null)
            {
                return;
            }

            servicesContainer.AppWindowsService.OpenBarcodeWindow(SelectedBarcode);
        }

        public void SaveToImageFile(BarcodeViewModel barcode)
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

        public void CopyToClipboard(BarcodeViewModel barcode)
        {
            if (barcode == null)
            {
                return;
            }

            servicesContainer.SystemService.CopyToClipboard(barcode.Barcode);
            StatusMessage = $"Barcode {barcode.Title} copied to clipboard";
        }

        public void MoveBarcodeDown(BarcodeViewModel barcode)
        {
            SelectedWorkspace.MoveDown(barcode);
        }

        public void MoveBarcodeUp(BarcodeViewModel barcode)
        {
            SelectedWorkspace.MoveUp(barcode);
        }

        public void RenameWorkspace()
        {
            if (SelectedWorkspace == null)
            {
                return;
            }

            RenameWorkspace(SelectedWorkspace);
        }

        private bool RenameWorkspace(WorkspaceViewModel workspace)
        {
            var workspaceName = servicesContainer.AppWindowsService.ShowWorkspaceNameWindow(workspace.Name, WorkspaceValidationRule);
            if (string.IsNullOrEmpty(workspaceName))
            {
                return false;
            }

            workspace.Name = workspaceName;
            return true;
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

        public void SetWorkspaceAsDefault()
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

        public void ChangeBarcodesWorkspace(BarcodeViewModel barcode)
        {
            var barcodesWorkspace = Workspaces.Single(w => w.Barcodes.Contains(barcode));

            if (Workspaces.Count() == 1)
            {
                servicesContainer.AppDialogsService.ShowInfo("There is no workspace to switch to");
            }
            else if (Workspaces.Count == 2)
            {
                var targetWorkspace = GetWorkspacesWithout(barcodesWorkspace).Single();
                MoveBarcodeToWorkspace(barcode, barcodesWorkspace, targetWorkspace);
            }
            else
            {
                var availableWorkspaces = GetWorkspacesWithout(barcodesWorkspace);
                var targetWorkspace = servicesContainer.AppWindowsService.SelectBarcodesWorkspace(availableWorkspaces);
                if (targetWorkspace == null)
                {
                    return;
                }
                MoveBarcodeToWorkspace(barcode, barcodesWorkspace, targetWorkspace);
            }
        }

        private IEnumerable<WorkspaceViewModel> GetWorkspacesWithout(WorkspaceViewModel workspace)
        {
            return Workspaces.Where(w => w != workspace);
        }

        private void MoveBarcodeToWorkspace(BarcodeViewModel barcode, WorkspaceViewModel sourceWorkspace, WorkspaceViewModel targetWorkspace)
        {
            sourceWorkspace.Barcodes.Remove(barcode);
            targetWorkspace.Barcodes.Insert(0, barcode);
        }

        public void ClearWorkspace()
        {
            if (SelectedWorkspace == null || SelectedWorkspace.Barcodes.Count == 0)
            {
                return;
            }

            var dialogResult = servicesContainer.AppDialogsService.ShowYesNoQuestion("Do you really want to clear active workspace?");
            if (!dialogResult)
            {
                return;
            }

            SelectedWorkspace.Barcodes.Clear();
        }

        public bool CheckStorageChanges()
        {
            var currentStorage = CreateCurrentStorage();
            return servicesContainer.StorageService.StorageChanged(currentStorage);
        }

        public void ImportBarcode()
        {
            var barcodeFile = servicesContainer.AppDialogsService.ImportBarcodeFile();
            if (string.IsNullOrEmpty(barcodeFile))
            {
                return;
            }

            try
            {
                var storageBarcode = servicesContainer.StorageService.ImportBarcode(barcodeFile);
                var newBarcode = storageBarcode.ToBarcode(servicesContainer.GeneratorService);

                if (!TryAddInitialWorkspace())
                {
                    return;
                }

                SelectedWorkspace.InsertNewBarcode(newBarcode);
                UpdateMessage($"Successfully imported {newBarcode.Title}");
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when importing barcode", exc);
            }
        }

        public void ImportWorkspace()
        {
            var workspaceFile = servicesContainer.AppDialogsService.ImportWorkspaceFile();
            if (string.IsNullOrEmpty(workspaceFile))
            {
                return;
            }

            try
            {
                var storageWorkspace = servicesContainer.StorageService.ImportWorkspace(workspaceFile);
                var importedWorkspace = storageWorkspace.ToWorkspace(servicesContainer.GeneratorService);
                importedWorkspace.Default = !Workspaces.Any(w => w.Default);

                AddWorkspace(importedWorkspace);
                SelectedWorkspace = importedWorkspace;
                UpdateMessage($"Successfully imported {importedWorkspace.Name}");
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when importing workspace", exc);
            }
        }

        public void ExportWorkspace()
        {
            var workspaceFile = servicesContainer.AppDialogsService.ExportWorkspaceFile(SelectedWorkspace.Name);
            if (string.IsNullOrEmpty(workspaceFile))
            {
                return;
            }

            try
            {
                servicesContainer.StorageService.ExportWorkspace(workspaceFile, SelectedWorkspace.ToStorage());
                StatusMessage = $"{SelectedWorkspace.Name} exported successfully";
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when exporting barcode", exc);
            }
        }

        public void ExportBarcode(BarcodeViewModel barcode)
        {
            var barcodeFile = servicesContainer.AppDialogsService.ExportBarcodeFile(barcode.Title);
            if (string.IsNullOrEmpty(barcodeFile))
            {
                return;
            }

            try
            {
                servicesContainer.StorageService.ExportBarcode(barcodeFile, barcode.ToStorage());
                StatusMessage = $"{barcode.Title} exported successfully";
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when exporting barcode", exc);
            }
        }

        public void InitialSequence()
        {
            try
            {
                servicesContainer.AppSettingsService.Load(true);
                LoadFromFile(servicesContainer.AppSettingsService.StoragePath, false);
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error while loading storage from file", exc);
            }
        }
    }
}
