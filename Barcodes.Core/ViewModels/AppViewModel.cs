using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Extensions;
using Barcodes.Core.Models;
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

        public List<BarcodeViewModel> SelectedBarcodes
        {
            get => SelectedWorkspace?.SelectedBarcodes;
        }

        public bool AreBarcodesSelected => SelectedBarcodes?.Count > 0;

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
            };
            AddWorkspace(newWorkspace, servicesContainer.AppSettingsService.WorkspaceAddMode);
        }

        public bool CheckStorageChangesAndSave()
        {
            if (CheckStorageChanges())
            {
                var closingMode = servicesContainer.AppDialogsService.ShowSavingQuestion();
                if (closingMode == SavingMode.SaveChanges)
                {
                    return Save(false, true);
                }
                else if (closingMode == SavingMode.Cancel)
                {
                    return false;
                }
            }
            return true;
        }

        public void LoadStorageFromFile()
        {
            if (CheckStorageChangesAndSave())
            {
                var storagePath = servicesContainer.AppSettingsService.StoragePath;
                var filePath = servicesContainer.AppDialogsService.OpenStorageFile(storagePath, true);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                LoadFromFile(filePath, true);
            }
        }

        private List<WorkspaceViewModel> LoadStorage(string storagePath, bool storageValidation, bool clearDefaultFlag)
        {
            var storage = servicesContainer.StorageService.Load(storagePath, true);
            if (storage == null)
            {
                return null;
            }

            var newWorkspaces = new List<WorkspaceViewModel>();
            foreach (var storageWorkspace in storage.Content)
            {
                newWorkspaces.Add(storageWorkspace.ToWorkspace(servicesContainer.GeneratorService));
            }

            if (clearDefaultFlag)
            {
                newWorkspaces.ForEach(w => w.DefaultWorkspace = false);
            }

            if (storageValidation && newWorkspaces.Count == 0)
            {
                servicesContainer.AppDialogsService.ShowError("Selected file has no workspaces");
                return null;
            }

            return newWorkspaces;
        }

        private void LoadFromFile(string storagePath, bool storageValidation)
        {
            try
            {
                var newWorkspaces = LoadStorage(storagePath, storageValidation, false);
                if (newWorkspaces == null)
                {
                    return;
                }

                Workspaces.Clear();
                foreach (var newWorkspace in newWorkspaces)
                {
                    AddWorkspace(newWorkspace);
                }

                SetDefaultWorkspace();
                if (WorkspacesCount != 0 || BarcodesCount != 0)
                {
                    UpdateMessage($"Successfully loaded {WorkspacesCount} workspaces and {BarcodesCount} barcodes from {Path.GetFileName(storagePath)}");
                }
                else
                {
                    UpdateMessage($"File {Path.GetFileName(storagePath)} loaded");
                }

                SetStoragePath(storagePath);
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when loading storage from file", exc);
            }
        }

        private void SetDefaultWorkspace()
        {
            WorkspaceViewModel selected = null;
            foreach (var workspace in Workspaces)
            {
                if (selected == null && workspace.DefaultWorkspace)
                {
                    selected = workspace;
                }
                else
                {
                    workspace.DefaultWorkspace = false;
                }
            }
            SelectedWorkspace = selected;
        }

        private void SaveStorage(string filePath, Storage storage)
        {
            servicesContainer.StorageService.Save(filePath, storage);
            SetStoragePath(filePath);
        }

        private void SetStoragePath(string filePath)
        {
            servicesContainer.AppSettingsService.StoragePath = filePath;
            StoragePath = filePath;
        }

        public void CreateNewStorage()
        {
            if (CheckStorageChangesAndSave())
            {
                var filePath = servicesContainer.AppDialogsService.SaveStorageFile(servicesContainer.AppSettingsService.StoragePath);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                try
                {
                    SaveStorage(filePath, new Storage());
                    LoadFromFile(filePath, false);
                }
                catch (Exception exc)
                {
                    servicesContainer.AppDialogsService.ShowException("Error when creating new storage file", exc);
                }
            }
        }

        public bool Save(bool promptForPath, bool showContinueQuestion)
        {
            try
            {
                var filePath = servicesContainer.AppSettingsService.StoragePath;
                if (promptForPath)
                {
                    filePath = servicesContainer.AppDialogsService.SaveStorageFile(filePath);
                    if (string.IsNullOrEmpty(filePath))
                    {
                        return false;
                    }
                }

                SaveStorage(filePath, CreateCurrentStorage());
                StatusMessage = $"Successfully saved {Path.GetFileName(filePath)}";
                return true;
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when saving barcodes to file", exc);
                if (showContinueQuestion)
                {
                    return servicesContainer.AppDialogsService.ShowYesNoQuestion("An error occured. Do you want to continue?");
                }
                return false;
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
                AddNewBarcode(barcode, false, barcode.Template);
            }
        }

        public void ShowSettings()
        {
            var previousStoragePath = servicesContainer.AppSettingsService.StoragePath;
            var result = servicesContainer.AppWindowsService.ShowSettingsWindow();
            if (result != null)
            {
                BarcodesVisible = servicesContainer.AppSettingsService.BarcodesVisible;
                if (previousStoragePath != servicesContainer.AppSettingsService.StoragePath && !Save(false, false))
                {
                    SetStoragePath(previousStoragePath);
                }
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
                barcodesToExport.AddRange(SelectedWorkspace.Barcodes);
            }
            else if (mode == PdfExportMode.Selected)
            {
                if (!AreBarcodesSelected)
                {
                    servicesContainer.AppDialogsService.ShowError("No selected barcodes to export");
                    return;
                }
                barcodesToExport.AddRange(SelectedBarcodes);
            }
            else if (mode == PdfExportMode.All)
            {
                if (BarcodesCount == 0)
                {
                    servicesContainer.AppDialogsService.ShowError("No barcodes to export");
                    return;
                }
                foreach (var workspace in Workspaces)
                {
                    barcodesToExport.AddRange(workspace.Barcodes);
                }
            }

            if (barcodesToExport.Count > 0)
            {
                ExecuteExportToPdf(barcodesToExport);
            }
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

        public void AddNewBarcode(BarcodeViewModel barcode, bool edit, BarcodeTemplate? template = null)
        {
            var result = servicesContainer.AppWindowsService.ShowGenerationWindow(barcode, edit, template);
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

        public void DeleteBarcodes()
        {
            if (AreBarcodesSelected && servicesContainer.AppDialogsService.ShowYesNoQuestion("Do you really want to delete selected barcodes?"))
            {
                SelectedWorkspace?.RemoveBarcodes();
            }
        }

        public void OpenBarcodesInNewWindow()
        {
            if (AreBarcodesSelected)
            {
                foreach (var barcode in SelectedBarcodes)
                {
                    servicesContainer.AppWindowsService.OpenBarcodeWindow(barcode);
                }
            }
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

        public void MoveBarcodesDown()
        {
            SelectedWorkspace?.MoveDown();
        }

        public void MoveBarcodesUp()
        {
            SelectedWorkspace?.MoveUp();
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
                w.DefaultWorkspace = false;
            }
            workspace.DefaultWorkspace = true;
        }

        public void MoveWorkspaceLeft(WorkspaceViewModel workspace)
        {
            workspaces.ShiftLeft(workspace);
            SelectedWorkspace = workspace;
        }

        public void MoveWorkspaceRight(WorkspaceViewModel workspace)
        {
            workspaces.ShiftRight(workspace);
            SelectedWorkspace = workspace;
        }

        public void ChangeBarcodesWorkspace()
        {
            if (AreBarcodesSelected)
            {
                if (Workspaces.Count() == 1)
                {
                    servicesContainer.AppDialogsService.ShowInfo("There is no workspace to switch to");
                }
                else if (Workspaces.Count == 2)
                {
                    var targetWorkspace = GetWorkspacesWithout(SelectedWorkspace).Single();
                    MoveBarcodesToWorkspace(SelectedBarcodes, SelectedWorkspace, targetWorkspace);
                }
                else
                {
                    var availableWorkspaces = GetWorkspacesWithout(SelectedWorkspace);
                    var targetWorkspace = servicesContainer.AppWindowsService.SelectBarcodesWorkspace(availableWorkspaces);
                    if (targetWorkspace == null)
                    {
                        return;
                    }
                    MoveBarcodesToWorkspace(SelectedBarcodes, SelectedWorkspace, targetWorkspace);
                }
            }
        }

        private IEnumerable<WorkspaceViewModel> GetWorkspacesWithout(WorkspaceViewModel workspace)
        {
            return Workspaces.Where(w => w != workspace);
        }

        private void MoveBarcodesToWorkspace(List<BarcodeViewModel> barcodes, WorkspaceViewModel sourceWorkspace, WorkspaceViewModel targetWorkspace)
        {
            targetWorkspace.InsertNewBarcodes(barcodes, servicesContainer.AppSettingsService.BarcodeAddMode, false);
            sourceWorkspace.RemoveBarcodes(barcodes);
        }

        public void ClearWorkspace(WorkspaceViewModel workspace)
        {
            var dialogResult = servicesContainer.AppDialogsService.ShowYesNoQuestion("Do you really want to clear active workspace?");
            if (dialogResult)
            {
                workspace.Clear();
            }
        }

        public void Clear()
        {
            var dialogResult = servicesContainer.AppDialogsService.ShowYesNoQuestion("Do you really want to remove all workspaces and barcodes?");
            if (dialogResult)
            {
                Workspaces.Clear();
            }
        }

        public bool CheckStorageChanges()
        {
            var currentStorage = CreateCurrentStorage();
            return servicesContainer.StorageService.StorageChanged(currentStorage);
        }

        public void ImportBarcodes()
        {
            var barcodeFiles = servicesContainer.AppDialogsService.ImportBarcodesFiles();
            if (barcodeFiles?.Count > 0)
            {
                barcodeFiles.Reverse();
                var barcodes = new List<BarcodeViewModel>();
                foreach (var barcodeFile in barcodeFiles)
                {
                    try
                    {
                        foreach (var storageBarcode in servicesContainer.StorageService.ImportBarcodes(barcodeFile))
                        {
                            barcodes.Add(storageBarcode.ToBarcode(servicesContainer.GeneratorService));
                        }
                    }
                    catch (Exception exc)
                    {
                        servicesContainer.AppDialogsService.ShowException("Error when importing barcode", exc);
                        return;
                    }
                }

                ImportBarcodes(barcodes);
            }
        }

        public void ImportBarcodes(List<BarcodeViewModel> barcodes)
        {
            if (!TryAddInitialWorkspace())
            {
                return;
            }

            SelectedWorkspace.InsertNewBarcodes(barcodes, servicesContainer.AppSettingsService.BarcodeAddMode);
            UpdateMessage($"Successfully imported {barcodes.Count} barcodes");
        }

        public void ImportWorkspaces()
        {
            var workspaceFiles = servicesContainer.AppDialogsService.ImportWorkspaceFiles();
            if (workspaceFiles?.Count > 0)
            {
                var workspaces = new List<WorkspaceViewModel>();
                foreach (var workspaceFile in workspaceFiles)
                {
                    try
                    {
                        var storageWorkspace = servicesContainer.StorageService.ImportWorkspace(workspaceFile);
                        var workspace = storageWorkspace.ToWorkspace(servicesContainer.GeneratorService);
                        workspace.DefaultWorkspace = false;
                        workspaces.Add(workspace);
                    }
                    catch (Exception exc)
                    {
                        servicesContainer.AppDialogsService.ShowException("Error when importing workspace", exc);
                        return;
                    }
                }

                ImportWorkspaces(workspaces);
            }
        }

        public void ImportWorkspaces(List<WorkspaceViewModel> workspaces)
        {
            AddWorkspaces(workspaces, servicesContainer.AppSettingsService.WorkspaceAddMode);
            UpdateMessage($"Successfully imported {workspaces.Count} workspaces");
        }

        public void ImportStorage()
        {
            try
            {
                var filePath = servicesContainer.AppDialogsService.OpenStorageFile(null, true);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                var workspaces = LoadStorage(storagePath, true, true);
                if (workspaces == null)
                {
                    return;
                }

                servicesContainer.AppWindowsService.ShowStorageWindow(this, workspaces);
            }
            catch (Exception exc)
            {
                servicesContainer.AppDialogsService.ShowException("Error when importing storage", exc);
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

        public void ExportBarcodes()
        {
            if (AreBarcodesSelected)
            {
                var barcodeFile = servicesContainer.AppDialogsService.ExportBarcodesFile();
                if (string.IsNullOrEmpty(barcodeFile))
                {
                    return;
                }

                try
                {
                    var barcodesToExport = SelectedBarcodes.Select(s => s.ToStorage())
                        .ToList();
                    servicesContainer.StorageService.ExportBarcodes(barcodeFile, barcodesToExport);
                    StatusMessage = "Barcodes exported successfully";
                }
                catch (Exception exc)
                {
                    servicesContainer.AppDialogsService.ShowException("Error when exporting barcode", exc);
                }
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
            if (Workspaces.SetAsFirst(workspace))
            {
                SelectedWorkspace = workspace;
            }
        }

        public void SetWorkspaceAsLast(WorkspaceViewModel workspace)
        {
            if (Workspaces.SetAsLast(workspace))
            {
                SelectedWorkspace = workspace;
            }
        }

        public void SetBarcodesAsFirst()
        {
            SelectedWorkspace?.SetBarcodesAsFirst();
        }

        public void SetBarcodesAsLast()
        {
            SelectedWorkspace?.SetBarcodesAsLast();
        }

        public void CloseAllWindows()
        {
            if (servicesContainer.AppWindowsService.WindowsCount > 0)
            {
                var result = servicesContainer.AppDialogsService.ShowYesNoQuestion("Do you really want to close all barcodes and workspaces windows?");
                if (result)
                {
                    servicesContainer.AppWindowsService.CloseBarcodesAndWorkspacesWindows();
                }
            }
        }

        public void OpenWorkspaceInNewWindowCommand(WorkspaceViewModel workspaceViewModel)
        {
            servicesContainer.AppWindowsService.OpenWorkspaceWindow(workspaceViewModel);
        }
    }
}