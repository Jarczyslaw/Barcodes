using Barcodes.Core.Events;
using Barcodes.Core.Services;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Dialogs;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Utils;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private BarcodesViewModel barcodes;
        public BarcodesViewModel Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }

        private MenuViewModel menu;
        public MenuViewModel Menu
        {
            get => menu;
            set => SetProperty(ref menu, value);
        }

        public bool IsBusy { get; set; } = false;

        private string busyMessage = string.Empty;
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

        public ShellViewModel(IContainerExtension container)
        {
            Barcodes = container.Resolve<BarcodesViewModel>();
            Menu = container.Resolve<MenuViewModel>();
            Menu.Barcodes = Barcodes;
            Menu.Shell = this;
            Menu.InitialBarcodesLoad();
        }
    }
}
