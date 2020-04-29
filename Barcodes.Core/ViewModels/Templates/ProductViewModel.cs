using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using Barcodes.Services.Generator;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class ProductViewModel : BaseTemplateViewModel
    {
        private string productCode = string.Empty;

        public ProductViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public string ProductCode
        {
            get => productCode;
            set => SetProperty(ref productCode, value);
        }

        protected override TemplateResult GetResultData()
        {
            return new TemplateResult
            {
                BarcodeType = BarcodeType.Ean13,
                Data = ProductCode.Trim()
            };
        }

        protected override bool Validate()
        {
            try
            {
                new ProductCode(ProductCode);
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