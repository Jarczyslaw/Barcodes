using Barcodes.Core.Services;
using Barcodes.Core.Services.ViewModelState;
using Barcodes.Utils;
using System;

namespace Barcodes.Core.ViewModels.AdditionalInput
{
    public class Ean128ProductViewModel : BaseInputViewModel
    {
        private string productCode;
        public string ProductCode
        {
            get => productCode;
            set => SetProperty(ref productCode, value);
        }

        private string serialNumber;
        public string SerialNumber
        {
            get => serialNumber;
            set => SetProperty(ref serialNumber, value);
        }

        public string ExpirationDate
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

        public Ean128ProductViewModel(IAppDialogsService dialogsService, IViewModelStateSaver viewModelStateSaver)
            : base(dialogsService, viewModelStateSaver)
        {
            viewModelStateSaver.LoadState(this);
        }

        protected override string GetResultData()
        {
            return $"(02)0{ProductCode}(17){ExpirationDate}(10){SerialNumber}";
        }

        protected override bool Validate()
        {
            if (string.IsNullOrEmpty(ProductCode) || ProductCode.Length != 13)
            {
                dialogsService.ShowError("Invalid product code");
                return false;
            }

            if (string.IsNullOrEmpty(ExpirationDate) || ExpirationDate.Length != 6)
            {
                dialogsService.ShowError("Invalid batch expiration date");
                return false;
            }

            if (string.IsNullOrEmpty(SerialNumber))
            {
                dialogsService.ShowError("Invalid serial number");
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
