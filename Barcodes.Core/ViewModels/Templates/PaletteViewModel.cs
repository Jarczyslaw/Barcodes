using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;
using System.Collections.Generic;

namespace Barcodes.Core.ViewModels.Templates
{
    public class PaletteViewModel : BaseTemplateViewModel
    {
        private string paletteNumberCode = string.Empty;
        private string selectedPrefix = string.Empty;

        public PaletteViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public string PaletteNumberCode
        {
            get => paletteNumberCode;
            set => SetProperty(ref paletteNumberCode, value);
        }

        public string SelectedPrefix
        {
            get => selectedPrefix;
            set => SetProperty(ref selectedPrefix, value);
        }

        public List<string> Prefixes
        {
            get => new PaletteCode().Prefixes;
        }

        public override void LoadData(string data)
        {
            SelectedPrefix = new PaletteCode().CurrentPrefix;
            if (PaletteCode.TryParse(data, out PaletteCode containerCode))
            {
                PaletteNumberCode = containerCode.PaletteNumberCode;
                SelectedPrefix = containerCode.CurrentPrefix;
            }
        }

        protected override TemplateResult GetResultData()
        {
            var code = new PaletteCode(PaletteNumberCode.Trim())
            {
                CurrentPrefix = SelectedPrefix
            };
            return new TemplateResult(code);
        }

        protected override bool Validate()
        {
            try
            {
                new PaletteCode(PaletteNumberCode.Trim());
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