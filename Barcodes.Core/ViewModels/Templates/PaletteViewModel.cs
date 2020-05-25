using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;
using System.Collections.Generic;

namespace Barcodes.Core.ViewModels.Templates
{
    public class PaletteViewModel : BaseTemplateViewModel
    {
        private int paletteNumber = 1;
        private string selectedPrefix = string.Empty;

        public PaletteViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public int PaletteNumber
        {
            get => paletteNumber;
            set => SetProperty(ref paletteNumber, value);
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
                PaletteNumber = containerCode.PaletteNumber;
                SelectedPrefix = containerCode.CurrentPrefix;
            }
        }

        protected override TemplateResult GetResultData()
        {
            var code = new PaletteCode(PaletteNumber);
            code.CurrentPrefix = SelectedPrefix;
            return new TemplateResult(code);
        }

        protected override bool Validate()
        {
            try
            {
                new PaletteCode(PaletteNumber);
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