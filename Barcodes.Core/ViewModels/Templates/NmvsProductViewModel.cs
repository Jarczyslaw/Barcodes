using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using Barcodes.Extensions;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class NmvsProductViewModel : BaseProductViewModel
    {
        private string batchId = string.Empty;
        private string batchExpDate = string.Empty;
        private string serialNo = string.Empty;

        public NmvsProductViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
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

        private NmvsProductCode GetCode()
        {
            return new NmvsProductCode(ProductCode.Trim(), SerialNo.Trim(), BatchId.Trim(), new NmvsDate(BatchExpDate.Trim()));
        }

        protected override TemplateResult GetResultData()
        {
            return new TemplateResult(GetCode());
        }

        protected override bool Validate()
        {
            try
            {
                GetCode();
                return true;
            }
            catch (Exception exc)
            {
                dialogsService.ShowException(null, exc);
                return false;
            }
        }

        protected override void LoadProductData(ProductCodeData data)
        {
            ProductCode = data.ProductCode.PadLeft(14, '0');
            BatchId = data.BatchId;
            if (data.ExpireDate.HasValue)
            {
                BatchExpDate = data.ExpireDate.Value.ToExpireDate(false);
            }
            SerialNo = data.SerialNo;
        }
    }
}