using Barcodes.Services.AppSettings;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class DragDropModesViewModel : BindableBase
    {
        private DragDropModeViewModel selectedItem;
        private ObservableCollection<DragDropModeViewModel> items;
        private readonly IAppSettingsService appSettingsService;


        public DragDropModesViewModel(IAppSettingsService appSettingsService)
        {
            this.appSettingsService = appSettingsService;
            Items = new ObservableCollection<DragDropModeViewModel>
            {
                new DragDropModeViewModel(DragDropMode.Arrangement),
                new DragDropModeViewModel(DragDropMode.ImportExport)
            };
            SelectedItem = Items[0];
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
                appSettingsService.DragDropMode = value.DragDropMode;
            }
        }

        public void Select(DragDropMode mode)
        {
            SelectedItem = Items.First(d => d.DragDropMode == mode);
        }
    }
}