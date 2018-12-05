﻿using Barcodes.Core.Events;
using Barcodes.Core.Services;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private BarcodesViewModel barcodes;
        public BarcodesViewModel Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }

        private readonly IDialogsService dialogsService;
        private readonly IAppSettingsService appSettingsService;
        private readonly IBarcodeStorageService barcodeStorageService;
        private readonly IAppWindowsService appWindowsService;

        public MenuViewModel(IDialogsService dialogsService, IEventAggregator eventAggregator, IAppSettingsService appSettingsService,
            IBarcodeStorageService barcodeStorageService, IAppWindowsService appWindowsService)
        {
            this.dialogsService = dialogsService;
            this.appSettingsService = appSettingsService;
            this.barcodeStorageService = barcodeStorageService;
            this.appWindowsService = appWindowsService;

            SaveToFileCommand = new DelegateCommand(SaveBarcodesToFile);
            LoadFromFileCommand = new DelegateCommand(LoadBarcodesFromFile);
            OpenStorageLocationCommand = new DelegateCommand(OpenStorageLocation);
            CloseCommand = new DelegateCommand(() => eventAggregator.GetEvent<CloseEvent>().Publish());
            ExportToPdfCommand = new DelegateCommand(ExportToPdf);
            ShowHelpCommand = new DelegateCommand(ShowHelp);
        }

        public DelegateCommand SaveToFileCommand { get; private set; }
        public DelegateCommand LoadFromFileCommand { get; private set; }
        public DelegateCommand OpenStorageLocationCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand ExportToPdfCommand { get; private set; }
        public DelegateCommand ShowHelpCommand { get; private set; }

        public void InitialBarcodesLoad()
        {
            LoadBarcodesFromFile(appSettingsService.StoragePath);
        }

        private void LoadBarcodesFromFile()
        {
            var directoryPath = Path.GetDirectoryName(appSettingsService.StoragePath);
            var filePath = dialogsService.OpenFile("Barcodes storage file", directoryPath, new DialogFilterPair { DisplayName = "json", ExtensionsList = "json" });
            if (string.IsNullOrEmpty(filePath))
                return;

            LoadBarcodesFromFile(filePath);
        }

        private void LoadBarcodesFromFile(string storagePath)
        {
            try
            {
                var barcodesFromStorage = barcodeStorageService.Load(storagePath, false);
                if (barcodesFromStorage == null)
                    return;

                foreach (var barcode in barcodesFromStorage)
                {
                    var barcodeData = new BarcodeData
                    {
                        Data = barcode.Data,
                        Type = barcode.Type
                    };
                    Barcodes.GenerateBarcode(barcodeData, barcode.Title);
                }
                appSettingsService.StoragePath = storagePath;
                Barcodes.StatusMessage = $"Successfully loaded barcodes from {Path.GetFileName(storagePath)}";
            }
            catch (Exception exc)
            {
                dialogsService.ShowException("Error when loading barcodes from file", exc);
            }
        }

        private void SaveBarcodesToFile()
        {
            if (Barcodes.BarcodesCount == 0)
            {
                dialogsService.ShowError("Generate barcodes before saving");
                return;
            }

            try
            {
                var fileName = Path.GetFileName(appSettingsService.StoragePath);
                var directoryPath = Path.GetDirectoryName(appSettingsService.StoragePath);
                var filePath = dialogsService.SaveFile("Barcodes storage file", directoryPath, fileName, new DialogFilterPair { DisplayName = "json", ExtensionsList = "json" });
                if (string.IsNullOrEmpty(filePath))
                    return;

                var barcodesToSave = Barcodes.Barcodes.Select(b => new BarcodeStorageEntry
                {
                    Data = b.Data,
                    Title = b.Title,
                    Type = b.Type
                }).ToList();

                barcodeStorageService.Save(filePath, barcodesToSave);
                appSettingsService.StoragePath = filePath;
                Barcodes.StatusMessage = $"Successfully saved barcodes to {fileName}";
            }
            catch (Exception exc)
            {
                dialogsService.ShowException("Error when saving barcodes to file", exc);
            }
        }

        private void ShowHelp()
        {
            appWindowsService.ShowHelpWindow();
        }

        private void ExportToPdf()
        {
            if (Barcodes.BarcodesCount == 0)
            {
                dialogsService.ShowError("Generate barcodes before export");
                return;
            }

            try
            {

            }
            catch (Exception exc)
            {
                dialogsService.ShowException("Error when generating a document", exc);
            }
        }

        private void OpenStorageLocation()
        {
            try
            {
                string argument = "/select, \"" + appSettingsService.StoragePath + "\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
            catch (Exception exc)
            {
                dialogsService.ShowException("Can not open storage file location", exc);
            }
        }
    }
}
