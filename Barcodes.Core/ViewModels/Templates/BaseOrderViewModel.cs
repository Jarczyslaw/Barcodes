using Barcodes.Core.Abstraction;

namespace Barcodes.Core.ViewModels.Templates
{
    public abstract class BaseOrderViewModel : BaseTemplateViewModel
    {
        protected int orderId;
        protected int containerNumber;
        protected int divisionNumber;
        protected int year;

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