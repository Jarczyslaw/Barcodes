using Barcodes.Core.Services;
using Barcodes.Core.Services.StateSaver;
using Barcodes.Core.Services.StateSaver.States;
using Barcodes.Utils;
using System;

namespace Barcodes.Core.ViewModels.AdditionalInput
{
    public class Ean128ProductViewModel : BaseInputViewModel
    {
        private string productCode = string.Empty;
        private string serialNumber = string.Empty;
        private DateTime expireDate = DateTime.Now;

        public Ean128ProductViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public DateTime ExpireDate
        {
            get => expireDate;
            set => SetProperty(ref expireDate, value);
        }

        public string SerialNumber
        {
            get => serialNumber;
            set => SetProperty(ref serialNumber, value);
        }

        public string ProductCode
        {
            get => productCode;
            set => SetProperty(ref productCode, value);
        }

        public void LoadData(string ean128Data)
        {
            if (string.IsNullOrEmpty(ean128Data))
            {
                return;
            }

            if (Ean128Code.TryParse(ean128Data, out Ean128Code ean128Code))
            {
                ExpireDate = ean128Code.ExpireDate;
                SerialNumber = ean128Code.SerialNumber;
                ProductCode = ean128Code.ProductCode;
            }
        }

        protected override string GetResultData()
        {
            return new Ean128Code(ProductCode.Trim(), SerialNumber.Trim(), ExpireDate).ToString();
        }

        protected override bool Validate()
        {
            try
            {
                var ean128Code = new Ean128Code(ProductCode.Trim(), SerialNumber.Trim(), ExpireDate);
                return true;
            }
            catch (Exception exc)
            {
                dialogsService.ShowException(null, exc);
                return false;
            }
        }
    }
}
