using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class UserViewModel : BaseTemplateViewModel
    {
        private int number = 1;

        public UserViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public int Number
        {
            get => number;
            set => SetProperty(ref number, value);
        }

        public override void LoadData(string data)
        {
            if (UserCode.TryParse(data, out UserCode userCode))
            {
                Number = userCode.Number;
            }
        }

        protected override TemplateResult GetResultData()
        {
            return new TemplateResult(new UserCode(Number));
        }

        protected override bool Validate()
        {
            try
            {
                new UserCode(Number);
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