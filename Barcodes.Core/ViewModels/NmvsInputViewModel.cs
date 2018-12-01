using Barcodes.Services.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class NmvsInputViewModel : BindableBase
    {
        public Action CloseAction { get; set; }
        public string ResultData { get; private set; } = null;

        private string productCode;
        public string ProductCode
        {
            get => productCode;
            set => SetProperty(ref productCode, value);
        }

        private string batchId;
        public string BatchId
        {
            get => batchId;
            set => SetProperty(ref batchId, value);
        }

        private string batchExpDate;
        public string BatchExpDate
        {
            get => batchExpDate;
            set => SetProperty(ref batchExpDate, value);
        }

        private string serialNo;
        public string SerialNo
        {
            get => serialNo;
            set => SetProperty(ref serialNo, value);
        }

        public DelegateCommand AcceptCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }

        private readonly IDialogsService dialogsService;

        public NmvsInputViewModel(IDialogsService dialogsService)
        {
            this.dialogsService = dialogsService;

            AcceptCommand = new DelegateCommand(Accept);
            CloseCommand = new DelegateCommand(() => CloseAction?.Invoke());
        }

        private void Accept()
        {
            if (!Validate())
                return;

            ResultData = $"01{ProductCode}17{BatchExpDate}21{SerialNo}GS10{BatchId}";
            CloseAction?.Invoke();
        }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(ProductCode) || ProductCode.Length != 14)
            {
                dialogsService.ShowError("Invalid product code");
                return false;
            }

            if (string.IsNullOrEmpty(BatchExpDate) || BatchExpDate.Length != 6)
            {
                dialogsService.ShowError("Invalid batch expiration date");
                return false;
            }

            if (string.IsNullOrEmpty(SerialNo) || SerialNo.Length > 20)
            {
                dialogsService.ShowError("Invalid serial number");
                return false;
            }

            if (string.IsNullOrEmpty(BatchId))
            {
                dialogsService.ShowError("Invalid batch identifier");
                return false;
            }

            return true;
        }
    }
}
