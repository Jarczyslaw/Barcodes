using Barcodes.Core.Services;
using Barcodes.Core.Services.ViewModelState;
using Barcodes.Utils;
using System;

namespace Barcodes.Core.ViewModels.AdditionalInput
{
    public class NmvsProductViewModel : BaseInputViewModel
    {
        private string productCode;
        private string batchId;
        private string batchExpDate;
        private string serialNo;

        public NmvsProductViewModel(IAppDialogsService dialogsService, IViewModelStateSaver viewModelStateSaver)
            : base(dialogsService, viewModelStateSaver)
        {
            viewModelStateSaver.LoadState(this);
        }

        public string ProductCode
        {
            get => productCode;
            set => SetProperty(ref productCode, value);
        }

        public string BatchId
        {
            get => batchId;
            set => SetProperty(ref batchId, value);
        }

        public string SerialNo
        {
            get => serialNo;
            set => SetProperty(ref serialNo, value);
        }

        public string BatchExpDate
        {
            get => batchExpDate;
            set => SetProperty(ref batchExpDate, value);
        }

        protected override string GetResultData()
        {
            const string groupSeparator = "\u001D";
            return string.Format("01{0}17{1}21{2}{3}10{4}", ProductCode, BatchExpDate, SerialNo, groupSeparator, BatchId);
        }

        protected override bool Validate()
        {
            if (string.IsNullOrEmpty(ProductCode) || ProductCode.Length != 14)
            {
                dialogsService.ShowError("Invalid product code");
                return false;
            }

            if (!NmvsDate.TryParse(BatchExpDate, out NmvsDate date))
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

        protected override void SaveState()
        {
            viewModelStateSaver.SaveState(this);
        }
    }
}
