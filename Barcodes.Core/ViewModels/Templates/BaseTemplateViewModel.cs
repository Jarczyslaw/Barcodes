using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public abstract class BaseTemplateViewModel : BindableBase, ICloseSource
    {
        protected readonly IAppDialogsService dialogsService;

        protected BaseTemplateViewModel(IAppDialogsService dialogsService)
        {
            this.dialogsService = dialogsService;

            AcceptCommand = new DelegateCommand(() =>
            {
                if (!Validate())
                {
                    return;
                }

                ResultData = GetResultData();
                OnClose?.Invoke();
            });
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
        }

        public Action OnClose { get; set; }
        public TemplateResult ResultData { get; private set; }

        public DelegateCommand AcceptCommand { get; }
        public DelegateCommand CloseCommand { get; }

        protected abstract bool Validate();

        protected abstract TemplateResult GetResultData();

        public virtual void LoadData(string data)
        {
        }
    }
}