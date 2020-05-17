using Barcodes.Extensions;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Storage;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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

        private void AddToBarcodes(BarcodeViewModel barcode, AddMode addMode)
        {
            if (addMode == AddMode.AsFirst)
            {
                Barcodes.Insert(0, barcode);
            }
            else
            {
                Barcodes.Add(barcode);
            }
        }

        public void InsertNewBarcode(BarcodeViewModel barcode, AddMode addMode = AddMode.AsFirst, bool updateMessage = true)
        {
            AddToBarcodes(barcode, addMode);
            if (updateMessage)
            {
                OnMessageUpdate?.Invoke($"Barcode {barcode.Title} generated successfully!");
            }
            SelectedBarcode = barcode;
        }

        public void InsertNewBarcodes(List<BarcodeViewModel> barcodes, AddMode addMode = AddMode.AsFirst, bool updateMessage = true)
        {
            if (addMode == AddMode.AsFirst)
            {
                barcodes.Reverse();
            }

            foreach (var barcode in barcodes)
            {
                AddToBarcodes(barcode, addMode);
            }

            if (updateMessage)
            {
                OnMessageUpdate?.Invoke($"{barcodes.Count} barcodes generated successfully!");
            }
        }

        public void ReplaceBarcode(BarcodeViewModel barcode, BarcodeViewModel newBarcode)
        {
            var barcodeIndex = Barcodes.IndexOf(barcode);
            Barcodes.Remove(barcode);
            Barcodes.Insert(barcodeIndex, newBarcode);
            OnMessageUpdate?.Invoke($"Barcode {newBarcode.Title} edited successfully!");
            SelectedBarcode = newBarcode;
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

        public void SetBarcodeAsFirst(BarcodeViewModel barcode)
        {
            if (Barcodes.IndexOf(barcode) > 0)
            {
                Barcodes.Remove(barcode);
                Barcodes.Insert(0, barcode);
                SelectedBarcode = barcode;
            }
        }

        public void SetBarcodeAsLast(BarcodeViewModel barcode)
        {
            if (Barcodes.IndexOf(barcode) < Barcodes.Count - 1)
            {
                Barcodes.Remove(barcode);
                Barcodes.Add(barcode);
                SelectedBarcode = barcode;
            }
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

        public void Clear()
        {
            Barcodes.Clear();
        }
    }
}