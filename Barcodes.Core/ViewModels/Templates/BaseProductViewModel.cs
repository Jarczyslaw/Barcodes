using Barcodes.Core.Abstraction;

namespace Barcodes.Core.ViewModels.Templates
{
    public abstract class BaseProductViewModel : BaseTemplateViewModel
    {
        protected string productCode = string.Empty;
        protected ProductCodesDecoder productCodesDecoder = new ProductCodesDecoder();

        protected BaseProductViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public string ProductCode
        {
            get => productCode;
            set => SetProperty(ref productCode, value);
        }

        public override void LoadData(string nmvsData)
        {
            var data = productCodesDecoder.Decode(nmvsData);
            if (data != null)
            {
                LoadProductData(data);
            }
        }

        protected abstract void LoadProductData(ProductCodeData data);
    }
}