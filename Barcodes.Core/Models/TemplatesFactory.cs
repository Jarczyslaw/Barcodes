using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.ViewModels;
using Barcodes.Core.ViewModels.Templates;
using System.Collections.ObjectModel;

namespace Barcodes.Core.Models
{
    public class TemplatesFactory
    {
        public ObservableCollection<TemplateViewModel> GetAllTemplates(IAppWindowsService appWindowsService)
        {
            return new ObservableCollection<TemplateViewModel>
            {
                new TemplateViewModel
                {
                    Title = "Select barcode template"
                },
                new TemplateViewModel
                {
                    Template = BarcodeTemplate.Product,
                    Title = "Product code - EAN13",
                    Handler = appWindowsService.OpenTemplateWindow<ProductViewModel>
                },
                new TemplateViewModel
                {
                    Template = BarcodeTemplate.LongProduct,
                    Title = "Long product code - EAN128",
                    Handler = appWindowsService.OpenTemplateWindow<Ean128ProductViewModel>
                },
                new TemplateViewModel
                {
                    Template = BarcodeTemplate.NmvsProduct,
                    Title = "NMVS product code - DataMatrix",
                    Handler = appWindowsService.OpenTemplateWindow<NmvsProductViewModel>
                },
                new TemplateViewModel
                {
                    Template = BarcodeTemplate.Container,
                    Title = "Container - Code128",
                    Handler = appWindowsService.OpenTemplateWindow<ContainerViewModel>
                },
                new TemplateViewModel
                {
                    Template = BarcodeTemplate.Location,
                    Title = "Location - Code128 ",
                    Handler = appWindowsService.OpenTemplateWindow<LocationViewModel>
                },
                new TemplateViewModel
                {
                    Template = BarcodeTemplate.Order,
                    Title = "Order (DZ) - Code128",
                    Handler = appWindowsService.OpenTemplateWindow<OrderViewModel>
                },
                new TemplateViewModel
                {
                    Template = BarcodeTemplate.ReleaseDocument,
                    Title = "Release document (WZ) - Code128",
                    Handler = appWindowsService.OpenTemplateWindow<ReleaseDocumentViewModel>
                },
                new TemplateViewModel
                {
                    Template = BarcodeTemplate.User,
                    Title = "User - Code128",
                    Handler = appWindowsService.OpenTemplateWindow<UserViewModel>
                },
                new TemplateViewModel
                {
                    Template = BarcodeTemplate.WarehouseAndStation,
                    Title = "Warehouse and station - Code128",
                    Handler = appWindowsService.OpenTemplateWindow<WarehouseAndStationViewModel>
                },
            };
        }
    }
}