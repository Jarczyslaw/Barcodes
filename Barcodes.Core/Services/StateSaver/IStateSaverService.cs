using Barcodes.Core.ViewModels.AdditionalInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.Services.StateSaver
{
    public interface IStateSaverService
    {
        void Save<TViewModel, TState>(TViewModel viewModel);
        void Load<TViewModel, TState>(TViewModel viewModel);
    }
}
