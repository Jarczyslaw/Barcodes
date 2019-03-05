﻿using Barcodes.Core.Common;
using Barcodes.Core.Services;
using Barcodes.Services.AppSettings;
using Prism.Mvvm;
using System;
using Unity;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase, IShowDestination, ICloseSource, IClosingDestination
    {
        private readonly IUnityContainer unityContainer;
        private readonly IAppDialogsService appDialogsService;
        private readonly IAppSettingsService appSettingsService;

        public ShellViewModel(IUnityContainer unityContainer, IAppDialogsService appDialogsService, IAppSettingsService appSettingsService)
        {
            this.unityContainer = unityContainer;
            this.appDialogsService = appDialogsService;
            this.appSettingsService = appSettingsService;

            App = unityContainer.Resolve<AppViewModel>();
            Menu = new MenuViewModel(App);
            BarcodeMenu = new BarcodeMenuViewModel(App);
            WorkspaceMenu = new WorkspaceMenuViewModel(App);

            InitialSequence();
        }

        public AppViewModel App { get; }
        public MenuViewModel Menu { get; }
        public BarcodeMenuViewModel BarcodeMenu { get; }
        public WorkspaceMenuViewModel WorkspaceMenu { get; }

        public Action OnClose
        {
            get => Menu.OnClose;
            set => Menu.OnClose = value;
        }

        public void OnShow()
        {
            if (App.BarcodesCount == 0)
            {
                App.AddNewBarcode();
            }
        }

        public bool OnClosing()
        {
            if (App.CheckChanges())
            {
                var closingMode = appDialogsService.ShowClosingQuestion();
                if (closingMode == ClosingMode.SaveChanges)
                {
                    App.ExecuteSaveToFile();
                }
                else if (closingMode == ClosingMode.Cancel)
                {
                    return true;
                }
            }
            return false;
        }

        private void InitialSequence()
        {
            appSettingsService.Load();
            App.LoadFromFile(appSettingsService.StoragePath);
        }
    }
}
