using Barcodes.Core.ViewModels;
using Prism.Events;

namespace Barcodes.Core.Events
{
    public class OnBarcodeGeneratedEvent : PubSubEvent<QuickGeneratorViewModel>
    {
    }
}