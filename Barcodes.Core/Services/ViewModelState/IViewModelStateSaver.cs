using Barcodes.Core.ViewModels.AdditionalInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.Services.ViewModelState
{
    public interface IViewModelStateSaver
    {
        void SaveState(Ean128ProductViewModel viewModel);
        void LoadState(Ean128ProductViewModel viewModel);
        void SaveState(NmvsProductViewModel viewModel);
        void LoadState(NmvsProductViewModel viewModel);
    }
}
