using Barcodes.Services.AppSettings;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public delegate void DragDropModeChanged(DragDropMode mode);

    public class DragDropModesViewModel : BindableBase
    {
        private DragDropModeViewModel selectedItem;
        private ObservableCollection<DragDropModeViewModel> items;

        public event DragDropModeChanged DragDropModeChanged = delegate { };

        public DragDropModesViewModel()
        {
            Items = new ObservableCollection<DragDropModeViewModel>
            {
                new DragDropModeViewModel(DragDropMode.Arrangement),
                new DragDropModeViewModel(DragDropMode.ImportExport)
            };
        }

        public ObservableCollection<DragDropModeViewModel> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public DragDropModeViewModel SelectedItem
        {
            get => selectedItem;
            set
            {
                SetProperty(ref selectedItem, value);
                DragDropModeChanged(value.DragDropMode);
            }
        }

        public void Select(DragDropMode mode, bool riseEvent = false)
        {
            selectedItem = Items.First(d => d.DragDropMode == mode);
            RaisePropertyChanged(nameof(SelectedItem));
            if (riseEvent)
            {
                DragDropModeChanged(mode);
            }
        }
    }
}