using Barcodes.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace Barcodes.Core.ViewModels
{
    public class WorkspaceViewModel : BindableBase
    {
        private string name;
        private BarcodeResultViewModel selectedBarcode;
        private ObservableCollection<BarcodeResultViewModel> barcodes;

        public WorkspaceViewModel()
        {
            Barcodes = new ObservableCollection<BarcodeResultViewModel>();
        }

        public Action<string> OnMessageUpdate { get; set; }

        public Action OnCounterUpdate { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public bool Default { get; set; }

        public ObservableCollection<BarcodeResultViewModel> Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }

        public BarcodeResultViewModel SelectedBarcode
        {
            get => selectedBarcode;
            set => SetProperty(ref selectedBarcode, value);
        }

        public void InsertNewBarcode(BarcodeResultViewModel barcode)
        {
            Barcodes.Insert(0, barcode);
            OnMessageUpdate?.Invoke($"Barcode {barcode.Title} generated successfully!");
            OnCounterUpdate?.Invoke();
        }

        public void ReplaceBarcode(BarcodeResultViewModel barcode, BarcodeResultViewModel newBarcode)
        {
            var barcodeIndex = Barcodes.IndexOf(barcode);
            Barcodes.Remove(barcode);
            Barcodes.Insert(barcodeIndex, newBarcode);
            OnMessageUpdate?.Invoke($"Barcode {newBarcode.Title} edited successfully!");
        }

        public void RemoveBarcode(BarcodeResultViewModel barcode)
        {
            Barcodes.Remove(barcode);
            OnMessageUpdate?.Invoke($"Successfully removed {barcode.Title}");
            OnCounterUpdate?.Invoke();
        }

        public void MoveDown(BarcodeResultViewModel barcode)
        {
            var index = Barcodes.IndexOf(barcode);
            Barcodes.ShiftRight(index);
        }

        public void MoveUp(BarcodeResultViewModel barcode)
        {
            var index = Barcodes.IndexOf(barcode);
            Barcodes.ShiftLeft(index);
        }
    }
}
