using Barcodes.Core.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public class BaseViewModel : BindableBase, ICloseSource, IOnShowAware
    {
        private bool isBusy;

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public Action OnClose { get; set; }

        public DelegateCommand CancelCommand => new DelegateCommand(CloseView);

        protected void CloseView()
        {
            if (OnClose == null)
            {
                throw new Exception("OnClose handler not specified");
            }
            OnClose();
        }

        public virtual void OnShow()
        {
        }
    }
}