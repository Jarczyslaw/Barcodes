using Barcodes.Services.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels.AdditionalInput
{
    public abstract class BaseAdditionalInputViewModel : BindableBase
    {
        public Action CloseAction { get; set; }
        public string ResultData { get; private set; } = null;

        public DelegateCommand AcceptCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }

        protected readonly IDialogsService dialogsService;

        public BaseAdditionalInputViewModel(IDialogsService dialogsService)
        {
            this.dialogsService = dialogsService;

            AcceptCommand = new DelegateCommand(() =>
            {
                if (!Validate())
                    return;

                ResultData = GetResultData();
                CloseAction?.Invoke();
            });
            CloseCommand = new DelegateCommand(() => CloseAction?.Invoke());
        }

        protected abstract bool Validate();
        protected abstract string GetResultData();
    }
}
