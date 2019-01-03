using Barcodes.Core.ViewModels.AdditionalInput;
using Nelibur.ObjectMapper;

namespace Barcodes.Core.Services.ViewModelState
{
    public class ViewModelStateSaver : IViewModelStateSaver
    {
        private Ean128ProductViewModelState ean128ProductViewModelState;
        private NmvsProductViewModelState nmvsProductViewModelState;

        public void SaveState(Ean128ProductViewModel viewModel)
        {
            ean128ProductViewModelState = TinyMapper.Map<Ean128ProductViewModelState>(viewModel);
        }

        public void LoadState(Ean128ProductViewModel viewModel)
        {
            if (ean128ProductViewModelState != null)
            {
                TinyMapper.Map(ean128ProductViewModelState, viewModel);
            }
        }

        public void SaveState(NmvsProductViewModel viewModel)
        {
            nmvsProductViewModelState = TinyMapper.Map<NmvsProductViewModelState>(viewModel);
        }

        public void LoadState(NmvsProductViewModel viewModel)
        {
            if (nmvsProductViewModelState != null)
            {
                TinyMapper.Map(nmvsProductViewModelState, viewModel);
            }
        }
    }
}
