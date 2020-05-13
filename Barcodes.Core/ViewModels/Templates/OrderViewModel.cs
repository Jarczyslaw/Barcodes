using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class OrderViewModel : BaseTemplateViewModel
    {
        private int orderId;
        private int containerNumber;
        private int divisionNumber;
        private int year;

        protected OrderViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public int OrderId
        {
            get => orderId;
            set => SetProperty(ref orderId, value);
        }

        public int ContainerNumber
        {
            get => containerNumber;
            set => SetProperty(ref containerNumber, value);
        }

        public int DivisionNumber
        {
            get => divisionNumber;
            set => SetProperty(ref divisionNumber, value);
        }

        public int Year
        {
            get => year;
            set => SetProperty(ref year, value);
        }

        private OrderCode GetCode => new OrderCode(orderId, containerNumber, divisionNumber, year);

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
            return new TemplateResult(GetCode);
        }

        protected override bool Validate()
        {
            try
            {
                new OrderCode(GetCode);
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