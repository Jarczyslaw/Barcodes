using Barcodes.Codes;
using Barcodes.Core.Services;
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

        private Ean128Code GetEan128Code()
        {
            return new Ean128Code(ProductCode.Trim(), SerialNumber.Trim(), ExpireDate);
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
                SerialNumber = ean128Code.BatchId;
                ProductCode = ean128Code.ProductCode;
            }
        }

        protected override string GetResultData()
        {
            return GetEan128Code().ToString();
        }

        protected override bool Validate()
        {
            try
            {
                var ean128Code = GetEan128Code();
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
