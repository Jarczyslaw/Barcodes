using Barcodes.Core.Abstraction;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public abstract class BaseOrderViewModel : BaseTemplateViewModel
    {
        protected int orderId = 1;
        protected int containerNumber = 1;
        protected int divisionNumber = 1;
        protected int year = DateTime.Now.Year;

        protected BaseOrderViewModel(IAppDialogsService dialogsService)
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
    }
}