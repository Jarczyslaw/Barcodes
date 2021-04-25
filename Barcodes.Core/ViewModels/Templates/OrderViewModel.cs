using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class OrderViewModel : BaseOrderViewModel
    {
        public OrderViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        protected OrderCode GetCode() => new OrderCode(orderId, containerNumber, divisionNumber, year);

        public override void LoadData(string data)
        {
            if (OrderCode.TryParse(data, out OrderCode orderCode))
            {
                OrderId = orderCode.OrderId;
                ContainerNumber = orderCode.ContainerNumber;
                DivisionNumber = orderCode.DivisionNumber;
                Year = orderCode.Year;
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