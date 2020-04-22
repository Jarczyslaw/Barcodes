using Barcodes.Core.Extensions;
using Barcodes.Core.Services;
using Barcodes.Extensions;
using Barcodes.Services.AppSettings;
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
        private ObservableCollection<WorkspaceViewModel> workspaces;
        private string busyMessage = string.Empty;
        private bool barcodesVisible;
        private string title = string.Empty;
        private string storagePath = string.Empty;

        private readonly IServicesContainer servicesContainer;

        public AppViewModel(IServicesContainer servicesContainer)
        {
            this.servicesContainer = servicesContainer;

            workspaces = new ObservableCollection<WorkspaceViewModel>();
            StoragePath = string.Empty;
        }

        public bool BarcodesVisible
        {
            get => barcodesVisible;
            set
            {
                SetProperty(ref barcodesVisible, value);
                servicesContainer.AppSettingsService.BarcodesVisible = value;
            }
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string StoragePath
        {
            get => storagePath;
            set
            {
                SetProperty(ref storagePath, value);
                Title = "Barcodes";
                if (!string.IsNullOrEmpty(value))
                {
                    Title += $" [{value}]";
                }
            }
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

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get => workspaces;
            set => SetProperty(ref workspaces, value);
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

        private void AddWorkspace(WorkspaceViewModel workspace, AddMode addMode = AddMode.AsLast)
        {
            workspace.OnMessageUpdate += UpdateMessage;
            if (addMode == AddMode.AsFirst)
            {
                workspaces.Insert(0, workspace);
            }
            else
            {
                workspaces.Add(workspace);
            }
            SelectedWorkspace = workspace;
        }

        private void AddWorkspaces(List<WorkspaceViewModel> workspaces, AddMode addMode = AddMode.AsLast)
        {
            if (addMode == AddMode.AsFirst)
            {
                workspaces.Reverse();
            }
            foreach (var workspace in workspaces)
            {
                AddWorkspace(workspace, addMode);
            }
        }

        private bool WorkspaceNameValidation(string workspaceName)
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
            var workspaceName = servicesContainer.AppWindowsService.ShowWorkspaceNameWindow(string.Empty, WorkspaceNameValidation);
            if (string.IsNullOrEmpty(workspaceName))
            {
                return;
            }

            var newWorkspace = new WorkspaceViewModel
            {
                Name = workspaceName,
                Default = !Workspaces.Any(w => w.Default)
            };
            AddWorkspace(newWorkspace, servicesContainer.AppSettingsService.WorkspaceAddMode);
        }

        public bool CheckStorageSave()
        {
            if (CheckStorageChanges())
            {
                var closingMode = servicesContainer.AppDialogsService.ShowSavingQuestion();
                if (closingMode == SavingMode.SaveChanges)
                {
                    Save(false, false);
                }
                else if (closingMode == SavingMode.Cancel)
                {
                    return true;
                }
            }
            return false;
        }

        public void LoadBarcodesFromFile()
        {
            if (!CheckStorageSave())
            {
                var storagePath = servicesContainer.AppSettingsService.StoragePath;
                var filePath = servicesContainer.AppDialogsService.OpenStorageFile(storagePath);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                LoadFromFile(filePath, true);
            }
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
                StoragePath = storagePath;
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when loading storage from file", exc);
            }
        }

        public void Save(bool validateBarcodesCount, bool promptForPath)
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
                if (promptForPath || string.IsNullOrEmpty(filePath))
                {
                    filePath = servicesContainer.AppDialogsService.SaveStorageFile(filePath);
                    if (string.IsNullOrEmpty(filePath))
                    {
                        return;
                    }
                }

                servicesContainer.StorageService.Save(filePath, CreateCurrentStorage());
                servicesContainer.AppSettingsService.StoragePath = filePath;
                StoragePath = filePath;
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

        public void ShowAbout()
        {
            servicesContainer.AppWindowsService.ShowAboutWindow();
        }

        public void ShowExamples()
        {
            var barcode = servicesContainer.AppWindowsService.ShowExamplesWindow();
            if (barcode != null)
            {
                AddNewBarcode(barcode, false);
            }
        }

        public void ShowSettings()
        {
            if (servicesContainer.AppWindowsService.ShowSettingsWindow())
            {
                StoragePath = servicesContainer.AppSettingsService.StoragePath;
            }
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

        public void OpenAppLocation()
        {
            try
            {
                servicesContainer.SystemService.OpenAppLocation();
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Can not open app location", exc);
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
                servicesContainer.SystemService.OpenFileLocation(storagePath);
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException(openErrorMessage, exc);
            }
        }

        public void AddNewBarcode(BarcodeViewModel barcode, bool edit)
        {
            var result = servicesContainer.AppWindowsService.ShowGenerationWindow(barcode, edit);
            if (result == null)
            {
                return;
            }

            if (!TryAddInitialWorkspace())
            {
                return;
            }

            SelectedWorkspace.InsertNewBarcode(result.Barcode, servicesContainer.AppSettingsService.BarcodeAddMode);
        }

        private bool TryAddInitialWorkspace()
        {
            if (WorkspacesCount != 0)
            {
                return true;
            }

            var workspaceName = servicesContainer.AppWindowsService.ShowWorkspaceNameWindow(null, WorkspaceNameValidation);
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
            var result = servicesContainer.AppWindowsService.ShowGenerationWindow(barcode, true);
            if (result == null)
            {
                return;
            }

            if (result.AddNew)
            {
                SelectedWorkspace.InsertNewBarcode(result.Barcode, servicesContainer.AppSettingsService.BarcodeAddMode);
            }
            else
            {
                SelectedWorkspace.ReplaceBarcode(barcode, result.Barcode);
            }
        }

        public void TryDeleteSelectedBarcode()
        {
            if (SelectedBarcode != null)
            {
                DeleteBarcode(SelectedBarcode);
            }
        }

        public void DeleteBarcode(BarcodeViewModel barcode)
        {
            if (!servicesContainer.AppDialogsService.ShowYesNoQuestion($"Do you really want to delete barcode {barcode.Title}?"))
            {
                return;
            }

            SelectedWorkspace.RemoveBarcode(barcode);
        }

        public void OpenInNewWindow(BarcodeViewModel barcode)
        {
            servicesContainer.AppWindowsService.OpenBarcodeWindow(barcode);
        }

        public void SaveToImageFile(BarcodeViewModel barcode)
        {
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

        public void CopyImageToClipboard(BarcodeViewModel barcode)
        {
            servicesContainer.SystemService.CopyToClipboard(barcode.Barcode);
            StatusMessage = $"Barcode image from {barcode.Title} copied to clipboard";
        }

        public void CopyDataToClipboard(BarcodeViewModel barcode)
        {
            servicesContainer.SystemService.CopyToClipboard(barcode.GenerationData.Data);
            StatusMessage = $"Barcode data from {barcode.Title} copied to clipboard";
        }

        public void MoveBarcodeDown(BarcodeViewModel barcode)
        {
            SelectedWorkspace.MoveDown(barcode);
        }

        public void MoveBarcodeUp(BarcodeViewModel barcode)
        {
            SelectedWorkspace.MoveUp(barcode);
        }

        public bool RenameWorkspace(WorkspaceViewModel workspace)
        {
            var workspaceName = servicesContainer.AppWindowsService.ShowWorkspaceNameWindow(workspace.Name, WorkspaceNameValidation);
            if (string.IsNullOrEmpty(workspaceName))
            {
                return false;
            }

            workspace.Name = workspaceName;
            return true;
        }

        public void DeleteWorkspace(WorkspaceViewModel workspace)
        {
            if (!servicesContainer.AppDialogsService.ShowYesNoQuestion($"Do you really want to delete workspace {workspace.Name}? This will delete all the codes of this workspace"))
            {
                return;
            }
            RemoveWorkspace(workspace);
        }

        public void SetWorkspaceAsDefault(WorkspaceViewModel workspace)
        {
            foreach (var w in Workspaces)
            {
                w.Default = false;
            }
            workspace.Default = true;
        }

        public void MoveWorkspaceLeft(WorkspaceViewModel workspace)
        {
            var index = workspaces.IndexOf(workspace);
            workspaces.ShiftLeft(index);
            SelectedWorkspace = workspace;
        }

        public void MoveWorkspaceRight(WorkspaceViewModel workspace)
        {
            var index = workspaces.IndexOf(workspace);
            workspaces.ShiftRight(index);
            SelectedWorkspace = workspace;
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

        public void ClearWorkspace(WorkspaceViewModel workspace)
        {
            var dialogResult = servicesContainer.AppDialogsService.ShowYesNoQuestion("Do you really want to clear active workspace?");
            if (!dialogResult)
            {
                return;
            }

            workspace.Barcodes.Clear();
        }

        public bool CheckStorageChanges()
        {
            var currentStorage = CreateCurrentStorage();
            return servicesContainer.StorageService.StorageChanged(currentStorage);
        }

        public void ImportBarcodes()
        {
            var barcodeFiles = servicesContainer.AppDialogsService.ImportBarcodeFiles();
            if (barcodeFiles?.Count > 0)
            {
                barcodeFiles.Reverse();
                var barcodes = new List<BarcodeViewModel>();
                foreach (var barcodeFile in barcodeFiles)
                {
                    try
                    {
                        var storageBarcode = servicesContainer.StorageService.ImportBarcode(barcodeFile);
                        barcodes.Add(storageBarcode.ToBarcode(servicesContainer.GeneratorService));
                    }
                    catch (Exception exc)
                    {
                        servicesContainer.AppDialogsService.ShowException("Error when importing barcode", exc);
                        return;
                    }
                }

                if (!TryAddInitialWorkspace())
                {
                    return;
                }

                SelectedWorkspace.InsertNewBarcodes(barcodes, servicesContainer.AppSettingsService.BarcodeAddMode);
                UpdateMessage($"Successfully imported {barcodes.Count} barcodes");
            }
        }

        public void ImportWorkspaces()
        {
            var workspaceFiles = servicesContainer.AppDialogsService.ImportWorkspaceFiles();
            if (workspaceFiles?.Count > 0)
            {
                var workspaces = new List<WorkspaceViewModel>();
                foreach (var workspace in workspaceFiles)
                {
                    try
                    {
                        var storageWorkspace = servicesContainer.StorageService.ImportWorkspace(workspace);
                        workspaces.Add(storageWorkspace.ToWorkspace(servicesContainer.GeneratorService));
                    }
                    catch (Exception exc)
                    {
                        servicesContainer.AppDialogsService.ShowException("Error when importing workspace", exc);
                        return;
                    }
                }

                AddWorkspaces(workspaces, servicesContainer.AppSettingsService.WorkspaceAddMode);
                SelectedWorkspace = workspaces.Last();
                if (!Workspaces.Any(w => w.Default))
                {
                    workspaces.Last().Default = true;
                }
                UpdateMessage($"Successfully imported {workspaces.Count} workspaces");
            }
        }

        public void ExportWorkspace(WorkspaceViewModel workspace)
        {
            var workspaceFile = servicesContainer.AppDialogsService.ExportWorkspaceFile(workspace.Name);
            if (string.IsNullOrEmpty(workspaceFile))
            {
                return;
            }

            try
            {
                servicesContainer.StorageService.ExportWorkspace(workspaceFile, workspace.ToStorage());
                StatusMessage = $"{workspace.Name} exported successfully";
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
                servicesContainer.AppSettingsService.Load(false);
                ApplySettings(servicesContainer.AppSettingsService.AppSettings);
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error while loading storage from file", exc);
            }
        }

        private void ApplySettings(AppSettings appSettings)
        {
            BarcodesVisible = appSettings.BarcodesVisible;
            StoragePath = appSettings.StoragePath;
            LoadFromFile(appSettings.StoragePath, false);
        }

        public void SetWorkspaceAsFirst(WorkspaceViewModel workspace)
        {
            if (Workspaces.IndexOf(workspace) > 0)
            {
                Workspaces.Remove(workspace);
                Workspaces.Insert(0, workspace);
                SelectedWorkspace = workspace;
            }
        }

        public void SetWorkspaceAsLast(WorkspaceViewModel workspace)
        {
            if (Workspaces.IndexOf(workspace) < Workspaces.Count - 1)
            {
                Workspaces.Remove(workspace);
                Workspaces.Add(workspace);
                SelectedWorkspace = workspace;
            }
        }

        public void SetBarcodeAsFirst(BarcodeViewModel barcode)
        {
            SelectedWorkspace.SetBarcodeAsFirst(barcode);
        }

        public void SetBarcodeAsLast(BarcodeViewModel barcode)
        {
            SelectedWorkspace.SetBarcodeAsLast(barcode);
        }
    }
}