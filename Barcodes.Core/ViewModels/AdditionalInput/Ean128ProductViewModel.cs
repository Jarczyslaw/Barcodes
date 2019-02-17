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
        private bool emptyDay;
        private DateTime selectedDateTime = DateTime.Now;
      
        public Ean128ProductViewModel(IAppDialogsService dialogsService, IStateSaverService stateSaverService)
            : base(dialogsService, stateSaverService)
        {
            stateSaverService.Load<Ean128ProductViewModel, Ean128ProductViewModelState>(this);
        }

        public DateTime SelectedDateTime
        {
            get => selectedDateTime;
            set => SetProperty(ref selectedDateTime, value);
        }

        public bool EmptyDay
        {
            get => emptyDay;
            set => SetProperty(ref emptyDay, value);
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

        public string ExpirationDate
        {
            get => selectedDateTime.ToExpireDate(EmptyDay);
        }

        protected override string GetResultData()
        {
            return $"(02)0{ProductCode.Trim()}(17){ExpirationDate}(10){SerialNumber.Trim()}";
        }

        protected override bool Validate()
        {
            var productCode = ProductCode.Trim();
            if (string.IsNullOrEmpty(productCode) || productCode.Length != 13)
            {
                dialogsService.ShowError("Invalid product code");
                return false;
            }

            if (string.IsNullOrEmpty(ExpirationDate) || ExpirationDate.Length != 6)
            {
                dialogsService.ShowError("Invalid batch expiration date");
                return false;
            }

            var serialNumber = SerialNumber.Trim();
            if (string.IsNullOrEmpty(serialNumber))
            {
                dialogsService.ShowError("Invalid serial number");
                return false;
            }

            return true;
        }

        protected override void SaveState()
        {
            stateSaverService.Save<Ean128ProductViewModel, Ean128ProductViewModelState>(this);
        }
    }
}
