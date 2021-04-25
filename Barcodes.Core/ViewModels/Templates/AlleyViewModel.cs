using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class AlleyViewModel : BaseTemplateViewModel
    {
        private string location = string.Empty;
        private string alley = string.Empty;

        public AlleyViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public string Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }

        public string Alley
        {
            get => alley;
            set => SetProperty(ref alley, value);
        }

        private AlleyCode GetCode => new AlleyCode(Location.Trim(), Alley.Trim());

        public override void LoadData(string data)
        {
            if (AlleyCode.TryParse(data, out AlleyCode alleyCode))
            {
                Location = alleyCode.LocationAddress;
                Alley = alleyCode.AlleyAddress;
            }
        }

        protected override TemplateResult GetResultData()
        {
            return new TemplateResult(GetCode);
        }

        protected override bool Validate()
        {
            try
            {
                var code = GetCode;
                return true;
            }
            catch (Exception exc)
            {
                dialogsService.ShowException(exc);
                return false;
            }
        }
    }
}