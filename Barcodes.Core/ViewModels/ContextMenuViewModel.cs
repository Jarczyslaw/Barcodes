using Barcodes.Core.Services;
using Barcodes.Extensions;
using Barcodes.Services.System;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;

namespace Barcodes.Core.ViewModels
{
    public class ContextMenuViewModel : BindableBase
    {
        private readonly IAppState appState;
        private readonly IAppWindowsService appWindowsService;
        private readonly IAppDialogsService appDialogsService;
        private readonly ISystemService systemService;

        public ContextMenuViewModel(IAppState appState, IAppWindowsService appWindowsService, IAppDialogsService appDialogsService,
            ISystemService systemService)
        {
            this.appState = appState;
            this.appWindowsService = appWindowsService;
            this.appDialogsService = appDialogsService;
            this.systemService = systemService;

            EditBarcodeCommand = new DelegateCommand<BarcodeResultViewModel>(EditBarcode);
            MoveUpCommand = new DelegateCommand<BarcodeResultViewModel>(MoveUp);
            MoveDownCommand = new DelegateCommand<BarcodeResultViewModel>(MoveDown);
            OpenInNewWindowCommand = new DelegateCommand<BarcodeResultViewModel>(OpenInNewWindow);
            SaveToImageFileCommand = new DelegateCommand<BarcodeResultViewModel>(SaveToImageFile);
            CopyToClipboardCommand = new DelegateCommand<BarcodeResultViewModel>(CopyToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeResultViewModel>(Delete);
        }

        public DelegateCommand<BarcodeResultViewModel> EditBarcodeCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> MoveUpCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> MoveDownCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> OpenInNewWindowCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> SaveToImageFileCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> CopyToClipboardCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> DeleteCommand { get; }

        private void EditBarcode(BarcodeResultViewModel barcode)
        {
            var result = appWindowsService.ShowGenerationWindow(barcode);
            if (result == null)
            {
                return;
            }

            if (result.AddAsNew)
            {
                appState.InsertNewBarcode(result.Barcode);
            }
            else
            {
                appState.ReplaceBarcode(barcode, result.Barcode);
            }
        }

        private void Delete(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
            {
                return;
            }

            if (!appDialogsService.ShowYesNoQuestion($"Do you really want to delete barcode \"{barcode.Title}?\""))
            {
                return;
            }

            appState.RemoveBarcode(barcode);
        }

        private void OpenInNewWindow(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
            {
                return;
            }

            appWindowsService.OpenBarcodeWindow(barcode);
        }

        private void SaveToImageFile(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
            {
                return;
            }

            try
            {
                var filePath = appDialogsService.SavePngFile(barcode.Title);
                barcode.Barcode.ToPng(filePath);
                appState.StatusMessage = $"Barcode \"{barcode.Title}\" saved in {Path.GetFileName(filePath)}";
            }
            catch (Exception exc)
            {
                appDialogsService.ShowException("Error when saving barcode to png file", exc);
            }
        }

        private void CopyToClipboard(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
            {
                return;
            }

            systemService.CopyToClipboard(barcode.Barcode);
            appState.StatusMessage = $"Barcode \"{barcode.Title}\" copied to clipboard";
        }

        private void MoveDown(BarcodeResultViewModel barcode)
        {
            appState.MoveDown(barcode);
        }

        private void MoveUp(BarcodeResultViewModel barcode)
        {
            appState.MoveUp(barcode);
        }
    }
}
