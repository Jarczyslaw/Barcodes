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

        public void SetBusyMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Please wait...";
            }
            BusyMessage = message;
        }



        #region Heavy actions

        public async Task HeavyAction(string message, Action action)
        {
            try
            {
                SetBusyMessage(message);
                await Task.Run(action);
            }
            finally
            {
                BusyMessage = null;
            }
        }

        public async Task<T> HeavyAction<T>(string message, Func<T> action)
        {
            try
            {
                SetBusyMessage(message);
                return await Task.Run(action);
            }
            finally
            {
                BusyMessage = null;
            }
        }

        public async Task HeavyActionAsync(string message, Func<Task> action)
        {
            try
            {
                SetBusyMessage(message);
                await action();
            }
            finally
            {
                BusyMessage = null;
            }
        }

        public async Task<T> HeavyActionAsync<T>(string message, Func<Task<T>> action)
        {
            try
            {
                SetBusyMessage(message);
                return await action();
            }
            finally
            {
                BusyMessage = null;
            }
        }

        #endregion Heavy actions
    }
}