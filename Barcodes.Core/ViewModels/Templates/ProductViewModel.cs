using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class ProductViewModel : BaseTemplateViewModel
    {
        private string product = string.Empty;

        public ProductViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public string Product
        {
            get => product;
            set => SetProperty(ref product, value);
        }

        public override void LoadData(string data)
        {
            if (ProductCode.TryParse(data, out ProductCode productCode))
            {
                Product = productCode.Product;
            }
        }

        protected override TemplateResult GetResultData()
        {
            var code = new ProductCode(product.Trim());
            return new TemplateResult(code);
        }

        protected override bool Validate()
        {
            try
            {
                new ProductCode(Product.Trim());
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