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
                    Title = "EAN13 - Product code",
                    Handler = appWindowsService.OpenTemplateWindow<ProductViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.LongProduct,
                    Title = "EAN128 - Long product code",
                    Handler = appWindowsService.OpenTemplateWindow<Ean128ProductViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.NmvsProduct,
                    Title = "DataMatrix - NMVS product code",
                    Handler = appWindowsService.OpenTemplateWindow<NmvsProductViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.Container,
                    Title = "Code128 - Container",
                    Handler = appWindowsService.OpenTemplateWindow<ContainerViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.Location,
                    Title = "Code128 - Location",
                    Handler = appWindowsService.OpenTemplateWindow<LocationViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.Order,
                    Title = "Code128 - Order (DZ)",
                    Handler = appWindowsService.OpenTemplateWindow<OrderViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.ReleaseDocument,
                    Title = "Code128 - Release document (WZ)",
                    Handler = appWindowsService.OpenTemplateWindow<ReleaseDocumentViewModel>
                },
                new TemplateViewModel
                {
                    Template = Template.User,
                    Title = "Code128 - User",
                    Handler = appWindowsService.OpenTemplateWindow<UserViewModel>
                },
            };
        }
    }
}