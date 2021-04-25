using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class ReleaseDocumentViewModel : BaseOrderViewModel
    {
        public ReleaseDocumentViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        protected ReleaseDocumentCode GetCode() => new ReleaseDocumentCode(orderId, containerNumber, divisionNumber, year);

        public override void LoadData(string data)
        {
            if (ReleaseDocumentCode.TryParse(data, out ReleaseDocumentCode releaseDocumentCode))
            {
                OrderId = releaseDocumentCode.OrderId;
                ContainerNumber = releaseDocumentCode.ContainerNumber;
                DivisionNumber = releaseDocumentCode.DivisionNumber;
                Year = releaseDocumentCode.Year;
            }
        }

        protected override TemplateResult GetResultData()
        {
            return new TemplateResult(GetCode());
        }

        protected override bool Validate()
        {
            try
            {
                GetCode();
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