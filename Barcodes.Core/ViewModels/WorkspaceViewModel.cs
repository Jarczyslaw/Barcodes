using Barcodes.Extensions;
using Barcodes.Services.Storage;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class WorkspaceViewModel : BindableBase
    {
        private bool defaultWorkspace;
        private string name;
        private BarcodeViewModel selectedBarcode;
        private ObservableCollection<BarcodeViewModel> barcodes;

        public WorkspaceViewModel()
        {
            Barcodes = new ObservableCollection<BarcodeViewModel>();
        }

        public Action<string> OnMessageUpdate { get; set; }

        public string DisplayName
        {
            get => Default ? $"(D) {Name}" : Name;
        }

        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public bool Default
        {
            get => defaultWorkspace;
            set
            {
                SetProperty(ref defaultWorkspace, value);
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public ObservableCollection<BarcodeViewModel> Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }

        public BarcodeViewModel SelectedBarcode
        {
            get => selectedBarcode;
            set => SetProperty(ref selectedBarcode, value);
        }

        public void InsertNewBarcode(BarcodeViewModel barcode)
        {
            Barcodes.Insert(0, barcode);
            OnMessageUpdate?.Invoke($"Barcode {barcode.Title} generated successfully!");
        }

        public void ReplaceBarcode(BarcodeViewModel barcode, BarcodeViewModel newBarcode)
        {
            var barcodeIndex = Barcodes.IndexOf(barcode);
            Barcodes.Remove(barcode);
            Barcodes.Insert(barcodeIndex, newBarcode);
            OnMessageUpdate?.Invoke($"Barcode {newBarcode.Title} edited successfully!");
        }

        public void RemoveBarcode(BarcodeViewModel barcode)
        {
            Barcodes.Remove(barcode);
            OnMessageUpdate?.Invoke($"Successfully removed {barcode.Title}");
        }

        public void MoveDown(BarcodeViewModel barcode)
        {
            var index = Barcodes.IndexOf(barcode);
            Barcodes.ShiftRight(index);
        }

        public void MoveUp(BarcodeViewModel barcode)
        {
            var index = Barcodes.IndexOf(barcode);
            Barcodes.ShiftLeft(index);
        }

        public StorageWorkspace ToStorage()
        {
            return new StorageWorkspace
            {
                Title = Name,
                Default = Default,
                Barcodes = Barcodes.Select(b => b.ToStorage()).ToList()
            };
        }
    }
}
