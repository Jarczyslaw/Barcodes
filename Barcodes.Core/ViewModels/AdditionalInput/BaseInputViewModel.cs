using Barcodes.Core.Common;
using Barcodes.Core.Services;
using Barcodes.Core.Services.StateSaver;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels.AdditionalInput
{
    public abstract class BaseInputViewModel : BindableBase, IClosable
    {
        protected readonly IAppDialogsService dialogsService;

        protected BaseInputViewModel(IAppDialogsService dialogsService)
        {
            this.dialogsService = dialogsService;

            AcceptCommand = new DelegateCommand(() =>
            {
                if (!Validate())
                {
                    return;
                }

                ResultData = GetResultData();
                CloseAction?.Invoke();
            });
            CloseCommand = new DelegateCommand(() => CloseAction?.Invoke());
        }

        public Action CloseAction { get; set; }
        public string ResultData { get; private set; }

        public DelegateCommand AcceptCommand { get; }
        public DelegateCommand CloseCommand { get; }

        protected abstract bool Validate();
        protected abstract string GetResultData();
    }
}
