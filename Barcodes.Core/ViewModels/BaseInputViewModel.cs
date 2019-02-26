using Barcodes.Core.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public abstract class BaseInputViewModel : BindableBase, ICloseAware
    {
        private string title;
        private string contentHeader;
        private string label;

        protected BaseInputViewModel(string title, string contentHeader, string label)
        {
            this.title = title;
            this.contentHeader = contentHeader;
            this.label = label;

            AcceptCommand = new DelegateCommand(Accept);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
        }

        public DelegateCommand AcceptCommand { get; }
        public DelegateCommand CloseCommand { get; }

        public Action OnClose { get; set; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string ContentHeader
        {
            get => contentHeader;
            set => SetProperty(ref contentHeader, value);
        }

        public string Label
        {
            get => label;
            set => SetProperty(ref label, value);
        }

        protected abstract void Accept();
    }
}
