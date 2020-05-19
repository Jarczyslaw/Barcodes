using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class ProductViewModel : BaseProductViewModel
    {
        public ProductViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        protected override TemplateResult GetResultData()
        {
            var code = new ProductCode(ProductCode.Trim());
            return new TemplateResult(code);
        }

        protected override void LoadProductData(ProductCodeData data)
        {
            var product = data.ProductCode;
            if (product.Length > 13)
            {
                product = product.Substring(1);
            }
            ProductCode = product;
        }

        protected override bool Validate()
        {
            try
            {
                new ProductCode(ProductCode.Trim());
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