using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Extensions;
using Barcodes.Core.Models;
using Barcodes.Extensions;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Storage;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class AppViewModel : BaseViewModel
    {
        private string statusMessage = string.Empty;
        private WorkspaceViewModel selectedWorkspace;
        private ObservableCollection<WorkspaceViewModel> workspaces;

        private bool barcodesVisible;
        private string title = string.Empty;
        private string storagePath = string.Empty;

        private readonly IServicesAggregator services;

        public AppViewModel(IServicesAggregator servicesContainer)
        {
            this.services = servicesContainer;

            workspaces = new ObservableCollection<WorkspaceViewModel>();
            StoragePath = string.Empty;

            servicesContainer.AppEvents.SettingsChanged += AppEvents_SettingsChanged;
        }

        public bool BarcodesVisible
        {
            get => barcodesVisible;
            set
            {
                SetProperty(ref barcodesVisible, value);
                services.AppSettingsService.BarcodesVisible = value;
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

        public int WorkspacesCount
        {
            get => Workspaces.Count;
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
                services.AppDialogsService.ShowError("Workspace's name can not be empty");
                return false;
            }

            return true;
        }

        public void AddNewWorkspace()
        {
            var workspaceName = services.AppWindowsService.ShowWorkspaceNameWindow(string.Empty, WorkspaceNameValidation);
            if (string.IsNullOrEmpty(workspaceName))
            {
                return;
            }

            var newWorkspace = new WorkspaceViewModel
            {
                Name = workspaceName,
            };
            AddWorkspace(newWorkspace, services.AppSettingsService.WorkspaceAddMode);
        }

        public bool CheckStorageChangesAndSave()
        {
            if (CheckStorageChanges())
            {
                services.AppWindowsService.ShowMainAppWindow();
                var closingMode = services.AppDialogsService.ShowSavingQuestion(services.AppWindowsService.MainWindowHandle);
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

        public async Task LoadStorageFromFile()
        {
            if (CheckStorageChangesAndSave())
            {
                var storagePath = services.AppSettingsService.StoragePath;
                var filePath = services.AppDialogsService.OpenStorageFile(storagePath, true);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                await LoadFromFile(filePath, true);
            }
        }

        private async Task<List<WorkspaceViewModel>> LoadStorage(string storagePath, bool storageValidation, bool clearDefaultFlag)
        {
            var storage = services.StorageService.Load(storagePath, true);
            if (storage == null)
            {
                return null;
            }

            var newWorkspaces = await HeavyAction("Loading...", () =>
            {
                var temp = new List<WorkspaceViewModel>();
                foreach (var storageWorkspace in storage.Content)
                {
                    temp.Add(storageWorkspace.ToWorkspace(services.GeneratorService));
                }
                return temp;
            });

            if (clearDefaultFlag)
            {
                newWorkspaces.ForEach(w => w.DefaultWorkspace = false);
            }

            if (storageValidation && newWorkspaces.Count == 0)
            {
                services.AppDialogsService.ShowError("Selected file has no workspaces");
                return null;
            }

            return newWorkspaces;
        }

        private async Task LoadFromFile(string storagePath, bool storageValidation)
        {
            try
            {
                var newWorkspaces = await LoadStorage(storagePath, storageValidation, false);
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
                services.LogException("Error when loading storage from file", exc);
            }
        }

        private void SetDefaultWorkspace()
        {
            if (Workspaces?.Count > 1)
            {
                var defaultWorkspace = Workspaces.FirstOrDefault(w => w.DefaultWorkspace);
                if (defaultWorkspace != null)
                {
                    SelectedWorkspace = defaultWorkspace;
                }

                SelectedWorkspace = Workspaces.First();
            }
        }

        private void SaveStorage(string filePath, Storage storage)
        {
            services.StorageService.Save(filePath, storage);
            SetStoragePath(filePath);
        }

        private void SetStoragePath(string filePath)
        {
            services.AppSettingsService.StoragePath = filePath;
            StoragePath = filePath;
        }

        public async Task CreateNewStorage()
        {
            if (CheckStorageChangesAndSave())
            {
                var filePath = services.AppDialogsService.SaveStorageFile(services.AppSettingsService.StoragePath);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                try
                {
                    SaveStorage(filePath, new Storage());
                    await LoadFromFile(filePath, false);
                }
                catch (Exception exc)
                {
                    services.LogException("Error when creating new storage file", exc);
                }
            }
        }

        public bool Save(bool promptForPath, bool showContinueQuestion)
        {
            try
            {
                var filePath = services.AppSettingsService.StoragePath;
                if (promptForPath)
                {
                    filePath = services.AppDialogsService.SaveStorageFile(filePath);
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
                services.LogException("Error when saving barcodes to file", exc);
                if (showContinueQuestion)
                {
                    return services.AppDialogsService.ShowYesNoQuestion("An error occured. Do you want to continue?");
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

        public async Task ShowAbout()
        {
            AboutViewModel aboutViewModel = null;
            await HeavyAction("Please wait...", () =>
            {
                aboutViewModel = new AboutViewModel(services.GeneratorService);
                aboutViewModel.GenerateRandomBarcode();
            });
            services.AppWindowsService.ShowAboutWindow(null, aboutViewModel);
        }

        public async Task ShowExamples()
        {
            ExamplesViewModel examplesViewModel = null;
            await HeavyAction("Generating examples...", () =>
            {
                examplesViewModel = services.ContainerExtension.Resolve<ExamplesViewModel>();
                examplesViewModel.ParentViewModel = this;
                examplesViewModel.CreateExamples();
            });

            var barcode = services.AppWindowsService.ShowExamplesWindow(null, examplesViewModel);
            if (barcode != null)
            {
                AddNewBarcode(barcode);
            }
        }

        private void AppEvents_SettingsChanged(SettingsSaveResult settingsSaveResult)
        {
            BarcodesVisible = settingsSaveResult.BarcodesVisible;
            if (settingsSaveResult.StoragePathChanged && !Save(false, false))
            {
                SetStoragePath(settingsSaveResult.PreviusStoragePath);
            }
        }

        private List<BarcodeViewModel> GetBarcodesToExport()
        {
            var mode = services.AppDialogsService.ShowExportQuestion();
            if (mode == ExportMode.None)
            {
                return null;
            }

            var barcodesToExport = new List<BarcodeViewModel>();
            if (mode == ExportMode.CurrentWorkspace)
            {
                if (SelectedWorkspace == null || SelectedWorkspace.Barcodes.Count == 0)
                {
                    services.AppDialogsService.ShowError("No workspace found or there are no barcodes in active workspace");
                    return null;
                }
                barcodesToExport.AddRange(SelectedWorkspace.Barcodes);
            }
            else if (mode == ExportMode.Selected)
            {
                if (!AreBarcodesSelected)
                {
                    services.AppDialogsService.ShowError("No selected barcodes to export");
                    return null;
                }
                barcodesToExport.AddRange(SelectedBarcodes);
            }
            else if (mode == ExportMode.All)
            {
                if (BarcodesCount == 0)
                {
                    services.AppDialogsService.ShowError("No barcodes to export");
                    return null;
                }
                foreach (var workspace in Workspaces)
                {
                    barcodesToExport.AddRange(workspace.Barcodes);
                }
            }

            return barcodesToExport;
        }

        public void ExportToPdf()
        {
            var barcodesToExport = GetBarcodesToExport();
            if (barcodesToExport?.Count > 0)
            {
                ExecuteExportToPdf(barcodesToExport);
            }
        }

        public void Export()
        {
            var mode = services.AppDialogsService.ShowExportQuestion(false);
            if (mode == ExportMode.Selected)
            {
                ExportBarcodes();
            }
            else if (mode == ExportMode.CurrentWorkspace)
            {
                ExportWorkspace(SelectedWorkspace);
            }
        }

        public void Print()
        {
            var barcodesToPrint = GetBarcodesToExport();
            if (barcodesToPrint?.Count > 0)
            {
                ExecutePrint(barcodesToPrint);
            }
        }

        private async void ExecutePrint(IEnumerable<BarcodeViewModel> barcodes)
        {
            try
            {
                BusyMessage = "Printing barcodes...";

                var barcodesToExport = barcodes.Select(b => b.ToDocBarcodeData()).ToList();
                await services.DocExportService.PrintAsync(barcodesToExport)
                    .ConfigureAwait(false);

                StatusMessage = "Successfully printed";
                BusyMessage = null;
            }
            catch (Exception exc)
            {
                services.LogException("Error when printing barcodes", exc);
            }
            finally
            {
                BusyMessage = null;
            }
        }

        private async void ExecuteExportToPdf(IEnumerable<BarcodeViewModel> barcodes)
        {
            try
            {
                var filePath = services.AppDialogsService.SavePdfFile();
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                BusyMessage = "Generating document...";

                var barcodesToExport = barcodes.Select(b => b.ToDocBarcodeData()).ToList();
                await services.DocExportService.ExportAsync(barcodesToExport, filePath)
                    .ConfigureAwait(false);

                StatusMessage = $"Successfully exported to {filePath}";
                BusyMessage = null;

                services.OpenNewFile(filePath);
            }
            catch (Exception exc)
            {
                services.LogException("Error when generating a document", exc);
            }
            finally
            {
                BusyMessage = null;
            }
        }

        public void AddNewBarcode(BarcodeViewModel barcode, bool edit, BarcodeTemplate? template = null)
        {
            var result = services.AppWindowsService.ShowGenerationWindow(null, barcode, edit, template);
            if (result != null)
            {
                AddNewBarcode(result);
            }
        }

        public void AddNewBarcode(GenerationResult generationResult)
        {
            if (!TryAddInitialWorkspace())
            {
                return;
            }

            SelectedWorkspace.InsertNewBarcode(generationResult.Barcode, services.AppSettingsService.BarcodeAddMode);
        }

        private bool TryAddInitialWorkspace()
        {
            if (WorkspacesCount != 0)
            {
                return true;
            }

            var workspaceName = services.AppWindowsService.ShowWorkspaceNameWindow(null, WorkspaceNameValidation);
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
            var result = services.AppWindowsService.ShowGenerationWindow(null, barcode, true);
            if (result == null)
            {
                return;
            }

            if (result.AddNew)
            {
                SelectedWorkspace.InsertNewBarcode(result.Barcode, services.AppSettingsService.BarcodeAddMode);
            }
            else
            {
                SelectedWorkspace.ReplaceBarcode(barcode, result.Barcode);
            }
        }

        public void DeleteBarcodes()
        {
            if (AreBarcodesSelected && services.AppDialogsService.ShowYesNoQuestion("Do you really want to delete selected barcodes?"))
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
                    services.AppWindowsService.OpenBarcodeWindow(barcode);
                }
            }
        }

        public void SaveToImageFile(BarcodeViewModel barcode)
        {
            try
            {
                var filePath = services.AppDialogsService.SavePngFile(barcode.Title);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                barcode.Barcode.ToPng(filePath);
                StatusMessage = $"Barcode {barcode.Title} saved in {Path.GetFileName(filePath)}";
            }
            catch (Exception exc)
            {
                services.LogException("Error when saving barcode to png file", exc);
            }
        }

        public void CopyImageToClipboard(BarcodeViewModel barcode)
        {
            services.SysService.CopyToClipboard(barcode.Barcode);
            StatusMessage = $"Barcode image from {barcode.Title} copied to clipboard";
        }

        public void CopyDataToClipboard(BarcodeViewModel barcode)
        {
            services.SysService.CopyToClipboard(barcode.GenerationData.Data);
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
            var workspaceName = services.AppWindowsService.ShowWorkspaceNameWindow(workspace.Name, WorkspaceNameValidation);
            if (string.IsNullOrEmpty(workspaceName))
            {
                return false;
            }

            workspace.Name = workspaceName;
            return true;
        }

        public void DeleteWorkspace(WorkspaceViewModel workspace)
        {
            if (!services.AppDialogsService.ShowYesNoQuestion($"Do you really want to delete workspace {workspace.Name}? This will delete all the codes of this workspace"))
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
                    services.AppDialogsService.ShowInfo("There is no workspace to switch to");
                }
                else if (Workspaces.Count == 2)
                {
                    var targetWorkspace = GetWorkspacesWithout(SelectedWorkspace).Single();
                    MoveBarcodesToWorkspace(SelectedBarcodes, SelectedWorkspace, targetWorkspace);
                }
                else
                {
                    var availableWorkspaces = GetWorkspacesWithout(SelectedWorkspace);
                    var targetWorkspace = services.AppWindowsService.SelectBarcodesWorkspace(availableWorkspaces);
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
            targetWorkspace.InsertNewBarcodes(barcodes, services.AppSettingsService.BarcodeAddMode, false);
            sourceWorkspace.RemoveBarcodes(barcodes);
        }

        public void ClearWorkspace(WorkspaceViewModel workspace)
        {
            var dialogResult = services.AppDialogsService.ShowYesNoQuestion("Do you really want to clear active workspace?");
            if (dialogResult)
            {
                workspace.Clear();
            }
        }

        public void Clear()
        {
            var dialogResult = services.AppDialogsService.ShowYesNoQuestion("Do you really want to remove all workspaces and barcodes?");
            if (dialogResult)
            {
                Workspaces.Clear();
            }
        }

        public bool CheckStorageChanges()
        {
            var currentStorage = CreateCurrentStorage();
            return services.StorageService.StorageChanged(currentStorage);
        }

        public void ImportBarcodes()
        {
            try
            {
                var barcodeFiles = services.AppDialogsService.ImportBarcodesFiles();
                if (barcodeFiles?.Count > 0)
                {
                    barcodeFiles.Reverse();
                    var barcodes = new List<BarcodeViewModel>();
                    foreach (var barcodeFile in barcodeFiles)
                    {
                        try
                        {
                            foreach (var storageBarcode in services.StorageService.ImportBarcodes(barcodeFile))
                            {
                                barcodes.Add(storageBarcode.ToBarcode(services.GeneratorService));
                            }
                        }
                        catch (Exception exc)
                        {
                            services.LogException("Error when importing barcodes", exc);
                            return;
                        }
                    }

                    ImportBarcodes(barcodes);
                }
            }
            catch (Exception exc)
            {
                services.LogException("Error when importing barcodes", exc);
            }
        }

        public void ImportBarcodes(List<BarcodeViewModel> barcodes)
        {
            if (!TryAddInitialWorkspace())
            {
                return;
            }

            SelectedWorkspace.InsertNewBarcodes(barcodes, services.AppSettingsService.BarcodeAddMode);
            UpdateMessage($"Successfully imported {barcodes.Count} barcodes");
        }

        public void ImportWorkspaces()
        {
            var workspaceFiles = services.AppDialogsService.ImportWorkspaceFiles();
            if (workspaceFiles?.Count > 0)
            {
                var workspaces = new List<WorkspaceViewModel>();
                foreach (var workspaceFile in workspaceFiles)
                {
                    try
                    {
                        var storageWorkspace = services.StorageService.ImportWorkspace(workspaceFile);
                        var workspace = storageWorkspace.ToWorkspace(services.GeneratorService);
                        workspace.DefaultWorkspace = false;
                        workspaces.Add(workspace);
                    }
                    catch (Exception exc)
                    {
                        services.LogException("Error when importing workspace", exc);
                        return;
                    }
                }

                ImportWorkspaces(workspaces);
            }
        }

        public void ImportWorkspaces(List<WorkspaceViewModel> workspaces)
        {
            AddWorkspaces(workspaces, services.AppSettingsService.WorkspaceAddMode);
            UpdateMessage($"Successfully imported {workspaces.Count} workspaces");
        }

        public async Task ImportStorage()
        {
            try
            {
                var filePath = services.AppDialogsService.OpenStorageFile(null, true);
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                var workspaces = await LoadStorage(filePath, true, true);
                if (workspaces == null)
                {
                    return;
                }

                services.AppWindowsService.ShowStorageWindow(this, workspaces);
            }
            catch (Exception exc)
            {
                services.LogException("Error when importing storage", exc);
            }
        }

        public void ExportWorkspace(WorkspaceViewModel workspace)
        {
            if (SelectedWorkspace != null)
            {
                var workspaceFile = services.AppDialogsService.ExportWorkspaceFile(workspace.Name);
                if (string.IsNullOrEmpty(workspaceFile))
                {
                    return;
                }

                try
                {
                    services.StorageService.ExportWorkspace(workspaceFile, workspace.ToStorage());
                    StatusMessage = $"{workspace.Name} exported successfully";
                }
                catch (Exception exc)
                {
                    services.LogException("Error when exporting barcode", exc);
                }
            }
            else
            {
                services.AppDialogsService.ShowError("No workspace selected");
            }
        }

        public void ExportBarcodes()
        {
            if (AreBarcodesSelected)
            {
                var barcodeFile = services.AppDialogsService.ExportBarcodesFile();
                if (string.IsNullOrEmpty(barcodeFile))
                {
                    return;
                }

                try
                {
                    var barcodesToExport = SelectedBarcodes.Select(s => s.ToStorage())
                        .ToList();
                    services.StorageService.ExportBarcodes(barcodeFile, barcodesToExport);
                    StatusMessage = "Barcodes exported successfully";
                }
                catch (Exception exc)
                {
                    services.LogException("Error when exporting barcode", exc);
                }
            }
            else
            {
                services.AppDialogsService.ShowError("No barcodes seleced");
            }
        }

        public async Task InitialSequence()
        {
            try
            {
                services.AppSettingsService.Load(false);
                await ApplySettings(services.AppSettingsService.AppSettings);
            }
            catch (Exception exc)
            {
                services.LogException("Error while loading storage from file", exc);
            }
        }

        private async Task ApplySettings(AppSettings appSettings)
        {
            BarcodesVisible = appSettings.BarcodesVisible;
            StoragePath = appSettings.StoragePath;
            await LoadFromFile(appSettings.StoragePath, false);
        }

        public void SetWorkspaceAsFirst(WorkspaceViewModel workspace)
        {
            Workspaces.SetAsFirst(workspace);
            SelectedWorkspace = workspace;
        }

        public void SetWorkspaceAsLast(WorkspaceViewModel workspace)
        {
            Workspaces.SetAsLast(workspace);
            SelectedWorkspace = workspace;
        }

        public void SetBarcodesAsFirst()
        {
            SelectedWorkspace?.SetBarcodesAsFirst();
        }

        public void SetBarcodesAsLast()
        {
            SelectedWorkspace?.SetBarcodesAsLast();
        }

        public void OpenWorkspaceInNewWindowCommand(WorkspaceViewModel workspaceViewModel)
        {
            services.AppWindowsService.OpenWorkspaceWindow(workspaceViewModel);
        }
    }
}