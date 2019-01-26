﻿using Barcodes.Services.AppSettings;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Services.System;
using Prism.Events;

namespace Barcodes.Core.Services
{
    public class ServicesContainer : IServicesContainer
    {
        public IEventAggregator EventAggregator { get; }
        public IGeneratorService BarcodesGenerator { get; }
        public IAppDialogsService DialogsService { get; }
        public IAppWindowsService AppWindowsService { get; }
        public ISystemService SystemService { get; }
        public IAppSettingsService AppSettingsService { get; }
        public IStorageService BarcodeStorageService { get; }
        public IDocExportService DocExportService { get; }

        public ServicesContainer(IEventAggregator eventAggregator, IGeneratorService barcodesGenerator, IAppDialogsService dialogsService,
            IAppWindowsService appWindowsService, ISystemService systemService, IAppSettingsService appSettingsService,
            IStorageService barcodeStorageService, IDocExportService docExportService)
        {
            EventAggregator = eventAggregator;
            BarcodesGenerator = barcodesGenerator;
            DialogsService = dialogsService;
            AppWindowsService = appWindowsService;
            SystemService = systemService;
            AppSettingsService = appSettingsService;
            BarcodeStorageService = barcodeStorageService;
            DocExportService = docExportService;
        }
    }
}
