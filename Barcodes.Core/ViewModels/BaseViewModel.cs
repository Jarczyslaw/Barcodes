using Barcodes.Core.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class BaseViewModel : BindableBase, ICloseSource, IOnShowAware
    {
        private bool isBusy;
        private string busyMessage = string.Empty;

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public string BusyMessage
        {
            get => busyMessage;
            set
            {
                SetProperty(ref busyMessage, value);
                IsBusy = !string.IsNullOrEmpty(busyMessage);
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        public Action OnClose { get; set; }

        public DelegateCommand CloseCommand => new DelegateCommand(CloseView);

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

        public async Task HeavyAction(string message, Func<Task> action)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                {
                    message = "Please wait...";
                }
                BusyMessage = message;
                await action();
            }
            finally
            {
                BusyMessage = null;
            }
        }
    }
}