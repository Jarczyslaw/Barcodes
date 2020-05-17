using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class ContainerViewModel : BaseTemplateViewModel
    {
        private int containerNumber = 1;

        public ContainerViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public int ContainerNumber
        {
            get => containerNumber;
            set => SetProperty(ref containerNumber, value);
        }

        public override void LoadData(string data)
        {
            if (ContainerCode.TryParse(data, out ContainerCode containerCode))
            {
                ContainerNumber = containerCode.ContainerNumber;
            }
        }

        protected override TemplateResult GetResultData()
        {
            var code = new ContainerCode(containerNumber);
            return new TemplateResult(code);
        }

        protected override bool Validate()
        {
            try
            {
                new ContainerCode(containerNumber);
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