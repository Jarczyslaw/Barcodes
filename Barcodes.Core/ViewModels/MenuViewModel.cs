﻿using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        public MenuViewModel(AppViewModel app)
        {
            SaveToFileCommand = new DelegateCommand(() => app.SaveToFile(true));
            LoadFromFileCommand = new DelegateCommand(app.LoadBarcodesFromFile);
            OpenStorageLocationCommand = new DelegateCommand(app.OpenStorageLocation);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
            ExportToPdfCommand = new DelegateCommand(app.ExportToPdf);
            ShowAboutCommand = new DelegateCommand(app.ShowAbout);
            ShowExamplesCommand = new DelegateCommand(app.ShowExamples);
            AddNewWorkspaceCommand = new DelegateCommand(app.AddNewWorkspace);
            AddNewBarcodeCommand = new DelegateCommand(app.AddNewBarcode);
            ImportBarcodeCommand = new DelegateCommand(app.ImportBarcode);
            ImportWorkspaceCommand = new DelegateCommand(app.ImportWorkspace);
        }

        public DelegateCommand SaveToFileCommand { get; }
        public DelegateCommand LoadFromFileCommand { get; }
        public DelegateCommand OpenStorageLocationCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand ExportToPdfCommand { get; }
        public DelegateCommand ShowAboutCommand { get; }
        public DelegateCommand ShowExamplesCommand { get; }
        public DelegateCommand AddNewWorkspaceCommand { get; }
        public DelegateCommand AddNewBarcodeCommand { get; }
        public DelegateCommand ImportBarcodeCommand { get; }
        public DelegateCommand ImportWorkspaceCommand { get; }

        public Action OnClose { get; set; }
    }
}
