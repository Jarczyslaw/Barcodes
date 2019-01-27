using Barcodes.Core.Services;
using Barcodes.Core.Services.StateSaver;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels.AdditionalInput
{
    public abstract class BaseInputViewModel : BindableBase
    {
        protected readonly IAppDialogsService dialogsService;
        protected readonly IStateSaverService stateSaverService;

        protected BaseInputViewModel(IAppDialogsService dialogsService, IStateSaverService stateSaverService)
        {
            this.dialogsService = dialogsService;
            this.stateSaverService = stateSaverService;

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

        public Action CloseAction { get; set; }
        public string ResultData { get; private set; }

        public DelegateCommand AcceptCommand { get; }
        public DelegateCommand CloseCommand { get; }

        protected abstract bool Validate();
        protected abstract void SaveState();
        protected abstract string GetResultData();
    }
}
