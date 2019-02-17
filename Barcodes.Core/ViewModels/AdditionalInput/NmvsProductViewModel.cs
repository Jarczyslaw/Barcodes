using Barcodes.Core.Services;
using Barcodes.Core.Services.StateSaver;
using Barcodes.Core.Services.StateSaver.States;
using Barcodes.Utils;

namespace Barcodes.Core.ViewModels.AdditionalInput
{
    public class NmvsProductViewModel : BaseInputViewModel
    {
        private string productCode = string.Empty;
        private string batchId = string.Empty;
        private string batchExpDate = string.Empty;
        private string serialNo = string.Empty;

        public NmvsProductViewModel(IAppDialogsService dialogsService, IStateSaverService stateSaverService)
            : base(dialogsService, stateSaverService)
        {
            stateSaverService.Load<NmvsProductViewModel, NmvsProductViewModelState>(this);
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
            return string.Format("01{0}17{1}21{2}{3}10{4}", 
                ProductCode.Trim(), BatchExpDate.Trim(), SerialNo.Trim(), groupSeparator, BatchId.Trim());
        }

        protected override bool Validate()
        {
            var productCode = ProductCode.Trim();
            if (string.IsNullOrEmpty(productCode) || productCode.Length != 14)
            {
                dialogsService.ShowError("Invalid product code");
                return false;
            }

            var batchExpDate = BatchExpDate.Trim();
            if (!NmvsDate.TryParse(batchExpDate, out NmvsDate date))
            {
                dialogsService.ShowError("Invalid batch expiration date");
                return false;
            }

            var serialNo = SerialNo.Trim();
            if (string.IsNullOrEmpty(serialNo) || serialNo.Length > 20)
            {
                dialogsService.ShowError("Invalid serial number");
                return false;
            }

            var batchId = BatchId.Trim();
            if (string.IsNullOrEmpty(batchId))
            {
                dialogsService.ShowError("Invalid batch identifier");
                return false;
            }

            return true;
        }

        protected override void SaveState()
        {
            stateSaverService.Save<NmvsProductViewModel, NmvsProductViewModelState>(this);
        }
    }
}
