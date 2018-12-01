﻿using Aspose.BarCode.Generation;
using Barcodes.Core.Services;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Generator;
using Barcodes.Services.Windows;
using Barcodes.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private BitmapSource randomBarcode;
        public BitmapSource RandomBarcode
        {
            get => randomBarcode;
            set => SetProperty(ref randomBarcode, value);
        }

        private string data;
        public string Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public int BarcodesCount
        {
            get => Barcodes.Count;
        }

        private string statusMessage = string.Empty;
        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        public bool ExtraInputEnabled
        {
            get
            {
                if (SelectedBarcodeType == null)
                    return false;
                return SelectedBarcodeType.ExtraInput;
            }
        }

        private BarcodeTypeViewModel selectedBarcodeType;
        public BarcodeTypeViewModel SelectedBarcodeType
        {
            get => selectedBarcodeType;
            set
            {
                SetProperty(ref selectedBarcodeType, value);
                ExtraInputCommand.RaiseCanExecuteChanged();
            }
        }

        public BarcodeViewModel SelectedBarcode { get; set; }
        public SelectedActionViewModel CurrentSelectedAction { get; set; }

        public DelegateCommand GenerateRandomBarcodeCommand { get; private set; }
        public DelegateCommand GenerateBarcodeCommand { get; private set; }
        public DelegateCommand ExecuteSelectedActionCommand { get; private set; }
        public DelegateCommand OpenInNewWindowCommand { get; private set; }
        public DelegateCommand ExtraInputCommand { get; private set; }

        public ObservableCollection<BarcodeTypeViewModel> BarcodeTypes { get; private set; }
        public ObservableCollection<BarcodeViewModel> Barcodes { get; private set; } = new ObservableCollection<BarcodeViewModel>();
        public ObservableCollection<SelectedActionViewModel> SelectedActions { get; private set; }

        private readonly IBarcodesGeneratorService barcodesGenerator;
        private readonly IDialogsService dialogsService;
        private readonly IBarcodeWindowsService barcodeWindowsService;

        public ShellViewModel(IBarcodesGeneratorService barcodesGenerator, IDialogsService dialogsService, IBarcodeWindowsService barcodeWindowsService)
        {
            this.barcodesGenerator = barcodesGenerator;
            this.dialogsService = dialogsService;
            this.barcodeWindowsService = barcodeWindowsService;

            GenerateRandomBarcodeCommand = new DelegateCommand(GenerateRandomBarcode);
            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
            ExecuteSelectedActionCommand = new DelegateCommand(() => CurrentSelectedAction?.Action());
            OpenInNewWindowCommand = new DelegateCommand(OpenInNewWindow);
            ExtraInputCommand = new DelegateCommand(ExtraInput, () => ExtraInputEnabled);

            InitializeBarcodeTypes();
            InitializeSelectedActions();
            GenerateRandomBarcode();

            Data = "Test data";
            Title = "Test title";
        }

        private void ExtraInput()
        {
            var result = barcodeWindowsService.OpenNmvsInputWindow();
            if (string.IsNullOrEmpty(result))
                return;
            Data = result;
        }

        private void GenerateRandomBarcode()
        {
            var randomText = RandomTexts.Get();
            RandomBarcode = barcodesGenerator.CreateShellBarcode(400, randomText);
        }

        private void InitializeSelectedActions()
        {
            SelectedActions = new ObservableCollection<SelectedActionViewModel>
            {
                new SelectedActionViewModel
                {
                    Title = "Open in new window",
                    Action = OpenInNewWindow
                },
                new SelectedActionViewModel
                {
                    Title = "Copy to clipboard",
                    Action = CopySelectedToClipboard
                },
                new SelectedActionViewModel
                {
                    Title = "Delete",
                    Action = DeleteSelected
                }
            };
            CurrentSelectedAction = SelectedActions.First();
        }

        private void OpenInNewWindow()
        {
            if (SelectedBarcode == null)
                return;

            barcodeWindowsService.OpenBarcodeWindow(SelectedBarcode);
        }

        private void DeleteSelected()
        {
            if (SelectedBarcode == null)
                return;

            if (!dialogsService.ShowYesNoQuestion("Do you really want to delete selected barcode?"))
                return;

            Barcodes.Remove(SelectedBarcode);
            RaisePropertyChanged(nameof(BarcodesCount));
        }

        private void CopySelectedToClipboard()
        {
            if (SelectedBarcode == null)
                return;

            Clipboard.SetImage(SelectedBarcode.Barcode);
            StatusMessage = $"Barcode \"{SelectedBarcode.Title}\" copied to clipboard";
        }

        private void GenerateBarcode()
        {
            if (!GenerateValidation())
                return;

            var barcodeData = new BarcodeData
            {
                MinWidth = 300,
                Data = Data,
                ShowData = false,
                Type = SelectedBarcodeType.Type
            };

            try
            {
                var barcode = barcodesGenerator.CreateBarcode(barcodeData);
                Barcodes.Add(new BarcodeViewModel
                {
                    Barcode = barcode,
                    Data = barcodeData.Data,
                    Title = Title,
                    TypeTitle = SelectedBarcodeType.TypeTitle
                });
                RaisePropertyChanged(nameof(BarcodesCount));
            }
            catch (Exception exc)
            {
                dialogsService.ShowException("Exception during barcode generation", exc);
            }
        }

        private bool GenerateValidation()
        {
            if (string.IsNullOrEmpty(Title))
            {
                dialogsService.ShowError("Enter barcode's title");
                return false;
            }

            if (string.IsNullOrEmpty(Data))
            {
                dialogsService.ShowError("Enter barcode's data");
                return false;
            }

            return true;
        }

        private void InitializeBarcodeTypes()
        {
            BarcodeTypes = new ObservableCollection<BarcodeTypeViewModel>
            {
                new BarcodeTypeViewModel
                {
                    TypeTitle = "Ean13",
                    Type = EncodeTypes.EAN13
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "Ean128",
                    Type = EncodeTypes.GS1Code128
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "Code128",
                    Type = EncodeTypes.Code128
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "DataMatrix",
                    Type = EncodeTypes.DataMatrix,
                    ExtraInput = true
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "QR",
                    Type = EncodeTypes.QR
                },
            };
            SelectedBarcodeType = BarcodeTypes.First();
        }
    }
}
