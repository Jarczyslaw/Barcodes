using Barcodes.Core.Services;
using Barcodes.Core.Services.ViewModelState;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels.AdditionalInput
{
    public abstract class BaseInputViewModel : BindableBase
    {
        public Action CloseAction { get; set; }
        public string ResultData { get; private set; }

        public DelegateCommand AcceptCommand { get; }
        public DelegateCommand CloseCommand { get; }

        protected readonly IAppDialogsService dialogsService;
        protected readonly IViewModelStateSaver viewModelStateSaver;

        protected BaseInputViewModel(IAppDialogsService dialogsService, IViewModelStateSaver viewModelStateSaver)
        {
            this.dialogsService = dialogsService;
            this.viewModelStateSaver = viewModelStateSaver;

            AcceptCommand = new DelegateCommand(() =>
            {
                if (!Validate())
                {
                    return;
                }

                ResultData = GetResultData();
                SaveState();
                CloseAction?.Invoke();
            });
            CloseCommand = new DelegateCommand(() => CloseAction?.Invoke());
        }

        protected abstract bool Validate();
        protected abstract void SaveState();
        protected abstract string GetResultData();
    }
}
