using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class LongProductViewModel : BaseProductViewModel
    {
        private string batchId = string.Empty;
        private DateTime expireDate = DateTime.Now;

        public LongProductViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public DateTime ExpireDate
        {
            get => expireDate;
            set => SetProperty(ref expireDate, value);
        }

        public string BatchId
        {
            get => batchId;
            set => SetProperty(ref batchId, value);
        }

        private LongProductCode GetCode()
        {
            return new LongProductCode(ProductCode.Trim(), BatchId.Trim(), ExpireDate);
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
            ProductCode = data.ProductCode;
            BatchId = data.BatchId;
            if (data.ExpireDate.HasValue)
            {
                ExpireDate = data.ExpireDate.Value;
            }
        }
    }
}