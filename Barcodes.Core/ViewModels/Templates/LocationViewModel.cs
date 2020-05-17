using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class LocationViewModel : BaseTemplateViewModel
    {
        private string location = string.Empty;

        public LocationViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public string Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }

        public override void LoadData(string data)
        {
            if (LocationCode.TryParse(data, out LocationCode locationCode))
            {
                location = locationCode.Address;
            }
        }

        protected override TemplateResult GetResultData()
        {
            var code = new LocationCode(Location.Trim());
            return new TemplateResult(code);
        }

        protected override bool Validate()
        {
            try
            {
                new LocationCode(Location.Trim());
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