using Barcodes.Core.Services;
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
            get => selectedDateTime.ToString("yyMMdd");
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

        public NmvsProductViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        protected override string GetResultData()
        {
            return $"01{ProductCode}17{BatchExpDate}21{SerialNo}GS10{BatchId}";
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
    }
}
