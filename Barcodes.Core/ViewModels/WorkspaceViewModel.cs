using Barcodes.Services.AppSettings;
using Barcodes.Services.Storage;
using JToolbox.Core.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class WorkspaceViewModel : BindableBase
    {
        private string name;
        private BarcodeViewModel selectedBarcode;
        private List<BarcodeViewModel> selectedBarcodes;
        private ObservableCollection<BarcodeViewModel> barcodes;

        public WorkspaceViewModel()
        {
            Barcodes = new ObservableCollection<BarcodeViewModel>();
        }

        public WorkspaceViewModel(WorkspaceViewModel workspaceViewModel)
            : this()
        {
            Name = workspaceViewModel.Name;
            foreach (var barcode in workspaceViewModel.Barcodes)
            {
                Barcodes.Add(new BarcodeViewModel(barcode));
            }
        }

        public Action<string> OnMessageUpdate { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
            
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

        public List<BarcodeViewModel> SelectedBarcodes
        {
            get => selectedBarcodes;
            set
            {
                selectedBarcodes = value.OrderBy(b => Barcodes.IndexOf(b))
                    .ToList();
            }
        }

        public bool AreBarcodesSelected => SelectedBarcodes?.Count > 0;

        public bool IsChecked
        {
            set
            {
                foreach (var barcode in Barcodes)
                {
                    barcode.IsChecked = value;
                }
            }
        }

        public bool ContainsCheckedBarcodes => Barcodes.Any(b => b.IsChecked);

        public List<BarcodeViewModel> CheckedBarcodes => Barcodes.Where(b => b.IsChecked).ToList();

        private void SetSelectedBarcode(BarcodeViewModel barcode)
        {
            SelectedBarcode = barcode;
            SelectedBarcodes = new List<BarcodeViewModel>
            {
                barcode
            };
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
                OnMessageUpdate?.Invoke($"Barcode {barcode.Title} added successfully!");
            }
            SetSelectedBarcode(barcode);
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
                OnMessageUpdate?.Invoke($"{barcodes.Count} barcodes added successfully!");
            }
        }

        public void ReplaceBarcode(BarcodeViewModel barcode, BarcodeViewModel newBarcode)
        {
            var barcodeIndex = Barcodes.IndexOf(barcode);
            Barcodes.Remove(barcode);
            Barcodes.Insert(barcodeIndex, newBarcode);
            OnMessageUpdate?.Invoke($"Barcode {newBarcode.Title} edited successfully!");
            SetSelectedBarcode(newBarcode);
        }

        public void RemoveBarcodes()
        {
            if (AreBarcodesSelected)
            {
                var count = SelectedBarcodes.Count;
                RemoveBarcodes(SelectedBarcodes);
                OnMessageUpdate?.Invoke($"Successfully removed {count} barcodes");
            }
        }

        public void RemoveBarcodes(List<BarcodeViewModel> barcodes)
        {
            foreach (var barcode in barcodes)
            {
                Barcodes.Remove(barcode);
            }
        }

        public void MoveDown()
        {
            if (AreBarcodesSelected)
            {
                Barcodes.ShiftRight(SelectedBarcodes);
            }
        }

        public void MoveUp()
        {
            if (AreBarcodesSelected)
            {
                Barcodes.ShiftLeft(SelectedBarcodes);
            }
        }

        public void SetBarcodesAsFirst()
        {
            if (AreBarcodesSelected)
            {
                Barcodes.SetAsFirst(SelectedBarcodes);
            }
        }

        public void SetBarcodesAsLast()
        {
            if (AreBarcodesSelected)
            {
                Barcodes.SetAsLast(SelectedBarcodes);
            }
        }

        public StorageWorkspace ToStorage()
        {
            return new StorageWorkspace
            {
                Title = Name,
                Barcodes = Barcodes.Select(b => b.ToStorage()).ToList()
            };
        }

        public void Clear()
        {
            Barcodes.Clear();
        }
    }
}