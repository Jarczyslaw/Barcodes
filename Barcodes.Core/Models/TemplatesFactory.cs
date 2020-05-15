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
                    Template = Template.Product,
                    Title = "Product code - EAN13",
                    Handler = appWindowsService.OpenTemplateWindow<ProductViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.LongProduct,
                    Title = "Long product code - EAN128",
                    Handler = appWindowsService.OpenTemplateWindow<Ean128ProductViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.NmvsProduct,
                    Title = "NMVS product code - DataMatrix",
                    Handler = appWindowsService.OpenTemplateWindow<NmvsProductViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.Container,
                    Title = "Container - Code128",
                    Handler = appWindowsService.OpenTemplateWindow<ContainerViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.Location,
                    Title = "Location - Code128 ",
                    Handler = appWindowsService.OpenTemplateWindow<LocationViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.Order,
                    Title = "Order (DZ) - Code128",
                    Handler = appWindowsService.OpenTemplateWindow<OrderViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.ReleaseDocument,
                    Title = "Release document (WZ) - Code128",
                    Handler = appWindowsService.OpenTemplateWindow<ReleaseDocumentViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.User,
                    Title = "User - Code128",
                    Handler = appWindowsService.OpenTemplateWindow<UserViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.WarehouseAndStation,
                    Title = "Warehouse and station - Code128",
                    Handler = appWindowsService.OpenTemplateWindow<WarehouseAndStationViewModel>
                },
            };
        }
    }
}