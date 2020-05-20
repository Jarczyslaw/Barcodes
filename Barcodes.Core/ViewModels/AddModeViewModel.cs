using Barcodes.Services.AppSettings;

namespace Barcodes.Core.ViewModels
{
    public class AddModeViewModel
    {
        public AddModeViewModel(AddMode addMode)
        {
            AddMode = addMode;
        }

        public AddMode AddMode { get; set; }
        public string Title => AddMode == AddMode.AsFirst ? "As first" : "As last";
    }
}