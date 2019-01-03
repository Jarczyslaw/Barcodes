using Barcodes.Core.Services;
using Barcodes.Core.Services.ViewModelState;
using Barcodes.Utils;
using System;

namespace Barcodes.Core.ViewModels.AdditionalInput
{
    public class NmvsProductViewModel : BaseInputViewModel
    {
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

        public string BatchExpDate
        {
            get => selectedDateTime.ToExpireDate(EmptyDay);
        }

        private bool emptyDay;
        public bool EmptyDay
        {
            get => emptyDay;
            set => SetProperty(ref emptyDay, value);
        }

        private DateTime selectedDateTime = DateTime.Now;
        public DateTime SelectedDateTime
        {
            get => selectedDateTime;
            set => SetProperty(ref selectedDateTime, value);
        }

        private string serialNo;
        public string SerialNo
        {
            get => serialNo;
            set => SetProperty(ref serialNo, value);
        }

        public NmvsProductViewModel(IAppDialogsService dialogsService, IViewModelStateSaver viewModelStateSaver)
            : base(dialogsService, viewModelStateSaver)
        {
        }

        protected override string GetResultData()
        {
            var groupSeparator = "\u001D";
            return string.Format("01{0}17{1}21{2}{3}10{4}", ProductCode, BatchExpDate, SerialNo, groupSeparator, BatchId);
        }

        protected override bool Validate()
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

        protected override void SaveState()
        {
            throw new NotImplementedException();
        }
    }
}
